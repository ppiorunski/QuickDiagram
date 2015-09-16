﻿using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Codartis.SoftVis.Diagramming;
using Codartis.SoftVis.Rendering.Wpf;
using Codartis.SoftVis.VisualStudioIntegration.ImageExport;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Codartis.SoftVis.VisualStudioIntegration.Diagramming
{
    /// <summary>
    /// Implements a Visual Studio tool window that displays a diagram.
    /// </summary>
    [Guid("02d1f8b9-d0a0-4ccb-9687-e6f0f781ad9e")]
    public class DiagramToolWindow : ToolWindowPane
    {
        private readonly DiagramViewerControl _diagramViewerControl;

        public Dpi ImageExportDpi { get; set; }

        public DiagramToolWindow() : base(null)
        {
            _diagramViewerControl = new DiagramViewerControl();

            Caption = "Diagram";
            ToolBar = new CommandID(VsctConstants.SoftVisCommandSetGuid, VsctConstants.ToolWindowToolbar);
            Content = _diagramViewerControl;
            ImageExportDpi = Dpi.Default;
        }

        internal void Initialize(Diagram diagram)
        {
            _diagramViewerControl.Diagram = diagram;
        }

        public int FontSize
        {
            get { return (int)_diagramViewerControl.FontSize; }
            set { _diagramViewerControl.FontSize = value; }
        }

        public void Show()
        {
            var windowFrame = (IVsWindowFrame)Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        public void FitDiagramToView()
        {
            _diagramViewerControl.FitDiagramToView();
        }

        public BitmapSource GetDiagramAsBitmap()
        {
            return _diagramViewerControl.GetDiagramAsBitmap(ImageExportDpi.Value);
        }
    }
}