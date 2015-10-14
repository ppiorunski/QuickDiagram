﻿using System.Collections.Generic;
using System.Linq;
using Codartis.SoftVis.Diagramming;
using Codartis.SoftVis.Geometry;
using Codartis.SoftVis.Graphs.Layout;
using Codartis.SoftVis.Modeling;
using Codartis.SoftVis.Rendering.Wpf.DiagramRendering;
using Codartis.SoftVis.Rendering.Wpf.DiagramRendering.ViewModels;

namespace Codartis.SoftVis.TestHostApp.TestData
{
    class TestDiagram : WpfDiagram
    {
        public List<IModelItem> ModelItems { get; }

        public TestDiagram(TestModel model)
        {
            ModelItems = model.Items.ToList();
        }

        protected override DiagramNode CreateDiagramNode(IModelEntity modelEntity)
        {
            var height = (int.Parse(modelEntity.Name) % 4) * 5 + 25;
            var size = new Size2D(((TestModelEntity)modelEntity).Size, height);
            return new DiagramNodeViewModel(modelEntity, Point2D.Zero, size);
        }

        private void LayoutTree()
        {
            Layout(LayoutType.Tree);
        }
    }
}
