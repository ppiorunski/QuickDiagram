﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Codartis.SoftVis.Common;
using Codartis.SoftVis.Diagramming;
using Codartis.SoftVis.Diagramming.Graph;
using Codartis.SoftVis.Modeling;
using Codartis.SoftVis.UI.Common;
using Codartis.SoftVis.UI.Extensibility;

namespace Codartis.SoftVis.UI.Wpf.ViewModel
{
    /// <summary>
    /// Tracks the change events of a diagram and creates/modifies diagram shape viewmodels accordingly.
    /// </summary>
    public class DiagramViewportViewModel : DiagramViewModelBase
    {
        private readonly Map<DiagramShape, DiagramShapeViewModelBase> _diagramShapeToViewModelMap;
        private readonly DiagramShapeViewModelFactory _diagramShapeViewModelFactory;
        private readonly DiagramButtonCollectionViewModel _diagramButtonCollectionViewModel;
        private readonly Viewport _viewport;

        private double _viewportZoom;
        private TransitionedTransform _transitionedViewportTransform;
        private DiagramNodeViewModel _focusedDiagramNode;
        private bool _isFocusPinned;
        private DiagramNodeViewModel _pinnedFocusedDiagramNode;

        public ObservableCollection<DiagramShapeViewModelBase> DiagramShapeViewModels { get; }
        public double MinZoom { get; }
        public double MaxZoom { get; }
        public Viewport.ResizeCommand ViewportResizeCommand { get; }
        public Viewport.PanCommand ViewportPanCommand { get; }
        public Viewport.ZoomToContentCommand ViewportZoomToContentCommand { get; }
        public Viewport.ZoomCommand ViewportZoomCommand { get; }

        public event Action ViewportChanged;
        public event EntitySelectorRequestedEventHandler EntitySelectorRequested;

        public DiagramViewportViewModel(IModel model, Diagram diagram, IDiagramBehaviourProvider diagramBehaviourProvider,
            double minZoom, double maxZoom, double initialZoom)
            : base(model, diagram)
        {
            _diagramShapeToViewModelMap = new Map<DiagramShape, DiagramShapeViewModelBase>();
            _diagramShapeViewModelFactory = new DiagramShapeViewModelFactory(model, diagram, diagram.ConnectorTypeResolver);
            _diagramButtonCollectionViewModel = new DiagramButtonCollectionViewModel(model, diagram, diagramBehaviourProvider);
            _viewport = new Viewport(minZoom, maxZoom, initialZoom);
            _transitionedViewportTransform = TransitionedTransform.Identity;
            _focusedDiagramNode = null;
            _isFocusPinned = false;

            DiagramShapeViewModels = new ObservableCollection<DiagramShapeViewModelBase>();
            MinZoom = minZoom;
            MaxZoom = maxZoom;
            ViewportResizeCommand = new Viewport.ResizeCommand(_viewport);
            ViewportPanCommand = new Viewport.PanCommand(_viewport);
            ViewportZoomToContentCommand = new Viewport.ZoomToContentCommand(_viewport);
            ViewportZoomCommand = new Viewport.ZoomCommand(_viewport);

            SubscribeToViewportEvents();
            SubscribeToDiagramEvents();
            SubscribeToDiagramButtonEvents();
            AddDiagram(diagram);
        }

        public ObservableCollection<DiagramButtonViewModelBase> DiagramButtonViewModels
            => _diagramButtonCollectionViewModel.DiagramButtonViewModels;

        public double ViewportZoom
        {
            get { return _viewportZoom; }
            set
            {
                _viewportZoom = value;
                OnPropertyChanged();
            }
        }

        public TransitionedTransform TransitionedViewportTransform
        {
            get { return _transitionedViewportTransform; }
            set
            {
                _transitionedViewportTransform = value;
                OnPropertyChanged();
            }
        }

        public DiagramNodeViewModel FocusedDiagramNode
        {
            get { return _pinnedFocusedDiagramNode; }
            set
            {
                _pinnedFocusedDiagramNode = value;
                OnPropertyChanged();
            }
        }

        public void ZoomToContent(TransitionSpeed transitionSpeed = TransitionSpeed.Slow)
            => _viewport.ZoomToContent(transitionSpeed);

        public void PinFocus()
        {
            _isFocusPinned = true;
        }

        public void UnpinFocus()
        {
            _isFocusPinned = false;
            ChangeFocusTo(_focusedDiagramNode);
        }

        private void SubscribeToViewportEvents()
        {
            _viewport.LinearZoomChanged += OnViewportLinearZoomChanged;
            _viewport.TransitionedTransformChanged += OnViewportTransitionedTransformChanged;
        }

        private void SubscribeToDiagramEvents()
        {
            Diagram.ShapeAdded += OnShapeAdded;
            Diagram.ShapeMoved += OnShapeMoved;
            Diagram.ShapeRemoved += OnShapeRemoved;
            Diagram.Cleared += OnDiagramCleared;
        }

        private void SubscribeToDiagramButtonEvents()
        {
            foreach (var showRelatedNodeButtonViewModel in DiagramButtonViewModels.OfType<ShowRelatedNodeButtonViewModel>())
                showRelatedNodeButtonViewModel.EntitySelectorRequested += OnEntitySelectorRequested;
        }

        private void AddDiagram(Diagram diagram)
        {
            foreach (var diagramNode in diagram.Nodes)
                OnShapeAdded(null, diagramNode);

            foreach (var diagramConnector in diagram.Connectors)
                OnShapeAdded(null, diagramConnector);

            UpdateDiagramContentRect();
        }

        private void OnShapeAdded(object sender, DiagramShape diagramShape)
        {
            var diagramShapeViewModel = _diagramShapeViewModelFactory.CreateViewModel(diagramShape);
            diagramShapeViewModel.GotFocus += OnShapeFocused;
            diagramShapeViewModel.LostFocus += OnShapeUnfocused;
            diagramShapeViewModel.RemoveRequested += OnShapeRemoveRequested;

            DiagramShapeViewModels.Add(diagramShapeViewModel);
            _diagramShapeToViewModelMap.Set(diagramShape, diagramShapeViewModel);

            UpdateDiagramContentRect();
            RaiseViewportChanged();
        }

        private void OnShapeMoved(object sender, DiagramShape diagramShape)
        {
            var diagramShapeViewModel = _diagramShapeToViewModelMap.Get(diagramShape);
            diagramShapeViewModel.UpdateState();

            UpdateDiagramContentRect();
            RaiseViewportChanged();
        }

        private void OnShapeRemoved(object sender, DiagramShape diagramShape)
        {
            var diagramShapeViewModel = _diagramShapeToViewModelMap.Get(diagramShape);
            if (diagramShapeViewModel == null)
                return;

            OnShapeUnfocused(diagramShapeViewModel);
            diagramShapeViewModel.GotFocus -= OnShapeFocused;
            diagramShapeViewModel.LostFocus -= OnShapeUnfocused;
            diagramShapeViewModel.RemoveRequested -= OnShapeRemoveRequested;

            DiagramShapeViewModels.Remove(diagramShapeViewModel);
            _diagramShapeToViewModelMap.Remove(diagramShape);

            UpdateDiagramContentRect();
            RaiseViewportChanged();
        }

        private void OnDiagramCleared(object sender, EventArgs e)
        {
            DiagramShapeViewModels.Clear();
            _diagramShapeToViewModelMap.Clear();

            UpdateDiagramContentRect();
            RaiseViewportChanged();
        }

        private void OnShapeRemoveRequested(DiagramShape diagramShape)
        {
            Diagram.RemoveShape(diagramShape);
        }

        private void OnShapeFocused(FocusableViewModelBase focusableViewModel)
        {
            _focusedDiagramNode = focusableViewModel as DiagramNodeViewModel;
            if (_isFocusPinned)
                return;

            ChangeFocusTo(_focusedDiagramNode);
        }

        private void OnShapeUnfocused(FocusableViewModelBase focusableViewModel)
        {
            _focusedDiagramNode = null;
            if (_isFocusPinned)
                return;

            ChangeFocusTo(null);
        }

        private void ChangeFocusTo(DiagramNodeViewModel diagramNodeViewModel)
        {
            if (diagramNodeViewModel == null)
                _diagramButtonCollectionViewModel.HideButtons();
            else
                _diagramButtonCollectionViewModel.AssignButtonsTo(diagramNodeViewModel);

            FocusedDiagramNode = diagramNodeViewModel;
        }

        private void UpdateDiagramContentRect()
        {
            _viewport.UpdateContentRect(Diagram.ContentRect.ToWpf());
        }

        private void OnViewportLinearZoomChanged(double viewportZoom)
        {
            ViewportZoom = viewportZoom;
        }

        private void OnViewportTransitionedTransformChanged(TransitionedTransform transitionedTransform)
        {
            TransitionedViewportTransform = transitionedTransform;
            RaiseViewportChanged();
        }

        private void OnEntitySelectorRequested(Point attachPointInDiagramSpace,
            HandleOrientation handleOrientation, IEnumerable<IModelEntity> modelEntities)
        {
            var attachPointInScreenSpace = _viewport.ProjectFromDiagramSpaceToScreenSpace(attachPointInDiagramSpace);
            EntitySelectorRequested?.Invoke(attachPointInScreenSpace, handleOrientation, modelEntities);
        }

        private void RaiseViewportChanged()
        {
            ViewportChanged?.Invoke();
        }
    }
}
