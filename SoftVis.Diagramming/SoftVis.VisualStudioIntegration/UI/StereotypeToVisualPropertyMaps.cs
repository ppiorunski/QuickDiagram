﻿using System.Collections.Generic;
using System.Windows.Media;
using Codartis.SoftVis.VisualStudioIntegration.Modeling;

namespace Codartis.SoftVis.VisualStudioIntegration.UI
{
    /// <summary>
    /// Defines the visual property constants for the different model entity stereotypes (background brush, image uri).
    /// </summary>
    internal static class StereotypeToVisualPropertyMaps 
    {
        private const string ImagePathTemplate = "/UI/Images/{0}.png";

        public static readonly Dictionary<RoslynBasedModelEntityStereotype, string> StereotypeToImagePathMap =
            new Dictionary<RoslynBasedModelEntityStereotype, string>
            {
                { RoslynBasedModelEntityStereotype.Class, string.Format(ImagePathTemplate, "Class")},
                { RoslynBasedModelEntityStereotype.Interface, string.Format(ImagePathTemplate, "Interface")},
                { RoslynBasedModelEntityStereotype.Struct, string.Format(ImagePathTemplate, "Struct")},
                { RoslynBasedModelEntityStereotype.Enum, string.Format(ImagePathTemplate, "Enum")},
                { RoslynBasedModelEntityStereotype.Delegate, string.Format(ImagePathTemplate, "Delegate")}
            };

        public static readonly Dictionary<RoslynBasedModelEntityStereotype, Brush> StereotypeToBackgroundBrushMap =
            new Dictionary<RoslynBasedModelEntityStereotype, Brush>
            {
                { RoslynBasedModelEntityStereotype.Class, new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0xE3, 0xD6))},
                { RoslynBasedModelEntityStereotype.Interface, Brushes.LightGray},
                { RoslynBasedModelEntityStereotype.Struct, new SolidColorBrush(Color.FromArgb(0xFF, 0xD1, 0xEA, 0xF3))},
                { RoslynBasedModelEntityStereotype.Enum, Brushes.Gold},
                { RoslynBasedModelEntityStereotype.Delegate, Brushes.Lavender}
            };
    }
}
