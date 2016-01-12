﻿using System.Linq;
using System.Windows;
using System.Windows.Input;
using Codartis.SoftVis.Diagramming.Graph;
using Codartis.SoftVis.Modeling;
using Codartis.SoftVis.UI.Extensibility;
using Codartis.SoftVis.UI.Wpf.Commands;
using Codartis.SoftVis.UI.Wpf.DiagramRendering.Viewport.Modification.MiniButtons;
using Codartis.SoftVis.UI.Wpf.ViewModel;

namespace Codartis.SoftVis.UI.Wpf
{
    /// <summary>
    /// The view model of a DiagramViewerControl.
    /// </summary>
    public class DiagramViewerViewModel
    {
        public IModel Model { get; }
        public Diagram Diagram { get; }
        public IDiagramBehaviourProvider DiagramBehaviourProvider { get; }

        public DiagramViewportViewModel DiagramViewportViewModel { get; }
        public EntitySelectorViewModel RelatedEntitySelectorViewModel { get; }
        public ICommand ShowRelatedEntitySelectorCommand { get; }
        public ICommand HideRelatedEntitySelectorCommand { get; }

        public DiagramViewerViewModel(IModel model, Diagram diagram, IDiagramBehaviourProvider diagramBehaviourProvider)
        {
            Model = model;
            Diagram = diagram;
            DiagramBehaviourProvider = diagramBehaviourProvider;

            DiagramViewportViewModel = new DiagramViewportViewModel(diagram);
            RelatedEntitySelectorViewModel = new EntitySelectorViewModel(new Size(200, 100));
            ShowRelatedEntitySelectorCommand = new DelegateCommand(i => ShowRelationshipSelector((MiniButtonActivatedEventArgs)i));
            HideRelatedEntitySelectorCommand = new DelegateCommand(i => HideRelationshipSelector());
        }

        private void ShowRelationshipSelector(MiniButtonActivatedEventArgs e)
        {
            var relatedEntities = Model.GetRelatedEntities(e.ModelEntity, e.RelationshipSpecification).ToList();
            RelatedEntitySelectorViewModel.Show(e.AttachPoint, e.HandleOrientation, relatedEntities);
        }

        private void HideRelationshipSelector()
        {
            RelatedEntitySelectorViewModel.Hide();
        }
    }
}