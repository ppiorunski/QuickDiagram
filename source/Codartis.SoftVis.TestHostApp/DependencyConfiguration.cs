﻿using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Codartis.SoftVis.Diagramming.Definition;
using Codartis.SoftVis.Diagramming.Definition.Layout;
using Codartis.SoftVis.Diagramming.Implementation;
using Codartis.SoftVis.Diagramming.Implementation.Layout.DirectConnector;
using Codartis.SoftVis.Diagramming.Implementation.Layout.Selection;
using Codartis.SoftVis.Diagramming.Implementation.Layout.Sugiyama;
using Codartis.SoftVis.Diagramming.Implementation.Layout.Vertical;
using Codartis.SoftVis.Modeling.Definition;
using Codartis.SoftVis.Modeling.Implementation;
using Codartis.SoftVis.Services;
using Codartis.SoftVis.Services.Plugins;
using Codartis.SoftVis.TestHostApp.Diagramming;
using Codartis.SoftVis.TestHostApp.Modeling;
using Codartis.SoftVis.TestHostApp.UI;
using Codartis.SoftVis.UI;
using Codartis.SoftVis.UI.Wpf;
using Codartis.SoftVis.UI.Wpf.View;
using Codartis.SoftVis.UI.Wpf.ViewModel;
using Codartis.Util.Ids;
using Codartis.Util.UI.Wpf.Resources;
using JetBrains.Annotations;

namespace Codartis.SoftVis.TestHostApp
{
    public static class DependencyConfiguration
    {
        [NotNull] private const string DiagramStylesXaml = "Resources/Styles.xaml";
        private const double ChildrenAreaPadding = 2;
        private const double GapBetweenNodes = 2;

        [NotNull]
        public static IContainer Create()
        {
            var builder = new ContainerBuilder();

            RegisterModelComponents(builder);
            RegisterDiagramComponents(builder);
            RegisterDiagramUiComponents(builder);
            RegisterDiagramPlugins(builder);

            builder.RegisterType<VisualizationService>().As<IVisualizationService>().SingleInstance();

            builder.RegisterType<MainWindowViewModel>();

            return builder.Build();
        }

        private static void RegisterModelComponents([NotNull] ContainerBuilder builder)
        {
            builder.RegisterType<SequenceGenerator>().As<ISequenceProvider>().SingleInstance();
            builder.RegisterType<DefaultModelNodeComparer>().As<IComparer<IModelNode>>();

            builder.RegisterType<ModelService>()
                .WithParameter("payloadEqualityComparer", null)
                .As<IModelService>()
                .As<IModelEventSource>()
                .SingleInstance();

            builder.RegisterType<TestRelatedNodeTypeProvider>().As<IRelatedNodeTypeProvider>();
            builder.RegisterType<ModelRelationshipFeatureProvider>().As<IModelRelationshipFeatureProvider>();
        }

        private static void RegisterDiagramComponents([NotNull] ContainerBuilder builder)
        {
            builder.RegisterType<DefaultDiagramNodeComparer>().As<IComparer<IDiagramNode>>();

            builder.RegisterType<DiagramService>()
                .As<IDiagramService>()
                .As<IDiagramEventSource>()
                .WithParameter("childrenAreaPadding", ChildrenAreaPadding)
                .SingleInstance();

            builder.RegisterType<TestConnectorTypeResolver>().As<IConnectorTypeResolver>();

            builder.RegisterType<SugiyamaLayoutAlgorithm>().AsSelf();
            builder.RegisterType<VerticalNodeLayoutAlgorithm>()
                .WithParameter("gapBetweenNodes", GapBetweenNodes)
                .AsSelf();

            builder.RegisterType<TestLayoutPriorityProvider>().As<ILayoutPriorityProvider>();
            builder.RegisterType<LayoutAlgorithmSelectionStrategy>().As<ILayoutAlgorithmSelectionStrategy>();
            builder.RegisterType<DirectConnectorRoutingAlgorithm>().As<IConnectorRoutingAlgorithm>();
        }

        private static void RegisterDiagramUiComponents([NotNull] ContainerBuilder builder)
        {
            builder.RegisterType<WpfDiagramUiService>().As<IDiagramUiService>();

            builder.RegisterType<DiagramViewModel>().As<IDiagramUi>();

            builder.RegisterType<DiagramViewportViewModel>().As<IDiagramViewportUi>()
                .WithParameter("minZoom", .2)
                .WithParameter("maxZoom", 5d)
                .WithParameter("initialZoom", 1d);

            builder.RegisterType<DiagramShapeViewModelFactory>().As<IDiagramShapeUiFactory>();
            builder.RegisterType<MiniButtonViewModelFactory>().As<IMiniButtonFactory>().SingleInstance();
            builder.RegisterType<MiniButtonPanelViewModel>().As<IMiniButtonManager>();

            builder.RegisterType<PayloadWrapperRelatedNodeItemViewModelFactory>().As<IRelatedNodeItemViewModelFactory>().SingleInstance();

            var resourceDictionary = ResourceHelpers.GetResourceDictionary(DiagramStylesXaml, Assembly.GetExecutingAssembly());

            builder.RegisterType<DiagramControl>()
                .WithParameter("additionalResourceDictionary", resourceDictionary);

            builder.RegisterType<DataCloningDiagramImageCreator>().As<IDiagramImageCreator>();
        }

        private static void RegisterDiagramPlugins([NotNull] ContainerBuilder builder)
        {
            builder.RegisterType<AutoLayoutDiagramPlugin>().As<IDiagramPlugin>();
            builder.RegisterType<ConnectorHandlerDiagramPlugin>().As<IDiagramPlugin>();
            builder.RegisterType<ModelTrackingDiagramPlugin>().As<IDiagramPlugin>();
        }
    }
}