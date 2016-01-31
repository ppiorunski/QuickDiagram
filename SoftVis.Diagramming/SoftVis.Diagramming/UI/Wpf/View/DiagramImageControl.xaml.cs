﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Codartis.SoftVis.UI.Wpf.Common;
using Codartis.SoftVis.UI.Wpf.ImageExport;

namespace Codartis.SoftVis.UI.Wpf.View
{
    /// <summary>
    /// Interaction logic for DiagramImageControl.xaml
    /// </summary>
    public partial class DiagramImageControl : UserControl
    {
        private readonly ResourceDictionary _additionalResourceDictionary;

        public static readonly DependencyProperty DiagramFillProperty =
            DiagramVisual.DiagramFillProperty.AddOwner(typeof(DiagramImageControl));

        public static readonly DependencyProperty DiagramStrokeProperty =
            DiagramVisual.DiagramStrokeProperty.AddOwner(typeof(DiagramImageControl));

        public DiagramImageControl(ResourceDictionary additionalResourceDictionary)
        {
            _additionalResourceDictionary = additionalResourceDictionary;

            InitializeComponent();
        }

        public Brush DiagramFill
        {
            get { return (Brush)GetValue(DiagramFillProperty); }
            set { SetValue(DiagramFillProperty, value); }
        }

        public Brush DiagramStroke
        {
            get { return (Brush)GetValue(DiagramStrokeProperty); }
            set { SetValue(DiagramStrokeProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            if (_additionalResourceDictionary != null)
                this.AddResourceDictionary(_additionalResourceDictionary);

            base.OnApplyTemplate();
        }

        public BitmapSource GetImage(Rect bounds, double dpi)
        {
            EnsureUpToDateDiagramForExport();
            return ImageRenderer.RenderUIElementToBitmap(this, bounds, dpi);
        }

        private void EnsureUpToDateDiagramForExport()
        {
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(DesiredSize));
            this.EnsureThatDelayedRenderingOperationsAreCompleted();
        }
    }
}
