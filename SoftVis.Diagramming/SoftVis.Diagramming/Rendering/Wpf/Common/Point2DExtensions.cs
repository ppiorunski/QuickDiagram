﻿using System.Windows;
using Codartis.SoftVis.Geometry;

namespace Codartis.SoftVis.Rendering.Wpf.Common
{
    public static class Point2DExtensions
    {
        public static Point ToWpf(this Point2D point2D)
        {
            return new Point(point2D.X, point2D.Y);
        }
    }
}
