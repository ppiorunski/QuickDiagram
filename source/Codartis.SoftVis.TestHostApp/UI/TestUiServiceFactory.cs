﻿using Codartis.SoftVis.Diagramming.Definition;
using Codartis.SoftVis.Modeling.Definition;
using Codartis.SoftVis.UI;
using Codartis.SoftVis.UI.Wpf;
using Codartis.SoftVis.UI.Wpf.ViewModel;

namespace Codartis.SoftVis.TestHostApp.UI
{
    /// <summary>
    /// Creates the main view model of the WPF UI.
    /// </summary>
    public class TestUiServiceFactory : IUiServiceFactory
    {
        public IUiService Create(
            IModelService modelService,
            IDiagramService diagramService,
            IRelatedNodeTypeProvider relatedNodeTypeProvider,
            double minZoom,
            double maxZoom,
            double initialZoom)
        {
            var diagramShapeUiFactory = new DiagramShapeUiFactory(relatedNodeTypeProvider);

            var diagramViewModel = new DiagramViewModel(modelService, diagramService, diagramShapeUiFactory, minZoom, maxZoom, initialZoom);

            return new WpfUiService(diagramViewModel);
        }
    }
}