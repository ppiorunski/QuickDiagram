﻿using System;
using Codartis.Util;

namespace Codartis.SoftVis.Geometry
{
    /// <summary>
    /// Represents a width + height pair.
    /// </summary>
    public struct Size2D
    {
        public static readonly Size2D Undefined = new Size2D(double.NaN, double.NaN);
        public static readonly Size2D Zero = new Size2D(0, 0);

        public double Width { get; }
        public double Height { get; }

        public Size2D(double width, double height)
        {
            if (width.IsDefined() && width < 0)
                throw new ArgumentOutOfRangeException(nameof(width), width, "Should be >= 0.");
            if (height.IsDefined() && height < 0)
                throw new ArgumentOutOfRangeException(nameof(height), height, "Should be >= 0.");

            Width = width;
            Height = height;
        }

        public bool IsDefined => !IsUndefined;

        public bool IsUndefined
            => double.IsNaN(Width) ||
               double.IsNaN(Height) ||
               double.IsInfinity(Width) ||
               double.IsInfinity(Height);

        public static Size2D operator +(Size2D size, Size2D otherSize) => new Size2D(size.Width + otherSize.Width, size.Height + otherSize.Height);
        public static Size2D operator *(Size2D size, double factor) => new Size2D(size.Width * factor, size.Height * factor);
        public static Size2D operator /(Size2D size, double factor) => new Size2D(size.Width / factor, size.Height / factor);

        public static bool Equals(Size2D size1, Size2D size2)
        {
            return size1.IsEqualWithTolerance(size2);
        }

        public bool Equals(Size2D other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Size2D))
                return false;

            var value = (Size2D) obj;
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        public static bool operator ==(Size2D left, Size2D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Size2D left, Size2D right)
        {
            return !left.Equals(right);
        }

        public bool IsEqualWithTolerance(Size2D otherSize)
        {
            return Width.IsEqualWithTolerance(otherSize.Width) && Height.IsEqualWithTolerance(otherSize.Height);
        }

        public override string ToString()
        {
            return $"({Width:0.##}x{Height:0.##})";
        }
    }
}