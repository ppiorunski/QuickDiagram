﻿using Codartis.SoftVis.Diagramming;
using Codartis.SoftVis.Modeling;
using Codartis.SoftVis.UI;

namespace Codartis.SoftVis.VisualStudioIntegration.UI
{
    /// <summary>
    /// Creates UI service instaces for roslyn-based diagrams.
    /// </summary>
    internal class RoslynUiServiceFactory : IUiServiceFactory
    {
        private readonly IHostUiServices _hostUiServices;
        private readonly bool _initialIsDescriptionVisible;

        public RoslynUiServiceFactory(IHostUiServices hostUiServices, bool initialIsDescriptionVisible)
        {
            _hostUiServices = hostUiServices;
            _initialIsDescriptionVisible = initialIsDescriptionVisible;
        }

        public IUiService Create(IReadOnlyModelStore modelStore, IReadOnlyDiagramStore diagramStore, 
            double minZoom, double maxZoom, double initialZoom)
        {
            var diagramViewModel = new RoslynDiagramViewModel(modelStore, diagramStore, 
                _initialIsDescriptionVisible, minZoom, maxZoom, initialZoom);

            return new RoslynUiService(_hostUiServices, diagramViewModel);
        }
    }
}
