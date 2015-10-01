﻿using System;
using System.Diagnostics;
using System.Linq;

namespace Codartis.SoftVis.Diagramming
{
    /// <summary>
    /// Represents a rectangle with a given position and size.
    /// </summary>
    [DebuggerDisplay("( {TopLeft.X}, {TopLeft.Y} )  ( {Size.Width}, {Size.Height} )")]
    public struct DiagramRect
    {
        public static readonly DiagramRect Empty = new DiagramRect(0, 0, 0, 0);

        public DiagramPoint TopLeft { get; }
        public DiagramSize Size { get; }

        public DiagramRect(DiagramPoint topLeft, DiagramSize size)
        {
            TopLeft = topLeft;
            Size = size;
        }

        public DiagramRect(DiagramPoint topLeft, DiagramPoint bottomRight)
            : this(topLeft, new DiagramSize(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y))
        {
        }

        public DiagramRect(double left, double top, double right, double bottom)
            : this(new DiagramPoint(left, top), new DiagramPoint(right, bottom))
        {
        }

        public double Top => TopLeft.Y;
        public double Bottom => TopLeft.Y + Size.Height;
        public double Left => TopLeft.X;
        public double Right => TopLeft.X + Size.Width;
        public double Width => Size.Width;
        public double Height => Size.Height;

        public DiagramPoint BottomRight => new DiagramPoint(Right, Bottom);
        public DiagramPoint Center => new DiagramPoint(Left + Size.Width / 2, Top + Size.Height / 2);
        public DiagramPoint Position => TopLeft;

        public static DiagramRect CreateFromCenterAndSize(DiagramPoint center, DiagramSize size)
        {
            var halfWidth = size.Width / 2;
            var halfHeight = size.Height / 2;
            return new DiagramRect(center.X - halfWidth, center.Y - halfHeight, center.X + halfWidth, center.Y + halfHeight);
        }

        public static DiagramRect Union(DiagramRect rect1, DiagramRect rect2)
        {
            var left = Math.Min(rect1.Left, rect2.Left);
            var top = Math.Min(rect1.Top, rect2.Top);

            var right = Math.Max(rect1.Right, rect2.Right);
            var bottom = Math.Max(rect1.Bottom, rect2.Bottom);

            return new DiagramRect(new DiagramPoint(left, top), new DiagramPoint(right, bottom));
        }

        public static DiagramRect Intersect(DiagramRect rect1, DiagramRect rect2)
        {
            var left = Math.Max(rect1.Left, rect2.Left);
            var top = Math.Max(rect1.Top, rect2.Top);

            var right = Math.Min(rect1.Right, rect2.Right);
            var bottom = Math.Min(rect1.Bottom, rect2.Bottom);

            return left >= right || top >= bottom
                ? Empty
                : new DiagramRect(new DiagramPoint(left, top), new DiagramPoint(right, bottom));
        }

        public DiagramPoint GetAttachPointToward(DiagramPoint targetPoint)
        {
            var center = Center;
            var sides = new[]
            {
                (Left - targetPoint.X)/(center.X - targetPoint.X),
                (Top - targetPoint.Y)/(center.Y - targetPoint.Y),
                (Right - targetPoint.X)/(center.X - targetPoint.X),
                (Bottom - targetPoint.Y)/(center.Y - targetPoint.Y)
            };

            var fi = Math.Max(0, sides.Where(i => i <= 1).Max());

            return targetPoint + fi * (center - targetPoint);
        }

        public bool Equals(DiagramRect other)
        {
            return TopLeft.Equals(other.TopLeft) && Size.Equals(other.Size);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DiagramRect && Equals((DiagramRect)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TopLeft.GetHashCode() * 397) ^ Size.GetHashCode();
            }
        }

        public static bool operator ==(DiagramRect left, DiagramRect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DiagramRect left, DiagramRect right)
        {
            return !left.Equals(right);
        }

        public DiagramRect WithMargin(double horizontalMargin, double verticalMargin)
        {
            return new DiagramRect(Left - horizontalMargin, Top - verticalMargin, Right + horizontalMargin, Bottom + verticalMargin);
        }

        public DiagramRect WithMarginX(double horizontalMargin) => WithMargin(horizontalMargin, 0);
        public DiagramRect WithMarginY(double verticalMargin) => WithMargin(0, verticalMargin);
    }
}
