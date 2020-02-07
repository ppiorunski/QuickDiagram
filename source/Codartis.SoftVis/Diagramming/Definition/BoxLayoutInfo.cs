﻿using Codartis.SoftVis.Geometry;
using JetBrains.Annotations;

namespace Codartis.SoftVis.Diagramming.Definition
{
    public sealed class BoxLayoutInfo 
    {
        [NotNull] public string ShapeId { get; }
        public Point2D TopLeft { get; }
        public Size2D HeaderSize { get; }
        public Size2D ChildrenAreaSize { get; }
        [CanBeNull] public GroupLayoutInfo ChildGroup { get; set; }

        public BoxLayoutInfo(
            [NotNull] string shapeId,
            Point2D topLeft,
            Size2D headerSize,
            Size2D childrenAreaSize,
            GroupLayoutInfo childGroup = null)
        {
            ShapeId = shapeId;
            TopLeft = topLeft;
            HeaderSize = headerSize;
            ChildrenAreaSize = childrenAreaSize;
            ChildGroup = childGroup;
        }

        public Rect2D Rect => new Rect2D(TopLeft, Size2D.StackVertically(HeaderSize, ChildrenAreaSize));
    }
}