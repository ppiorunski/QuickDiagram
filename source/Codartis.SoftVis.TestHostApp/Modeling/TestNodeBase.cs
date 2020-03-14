﻿using JetBrains.Annotations;

namespace Codartis.SoftVis.TestHostApp.Modeling
{
    internal abstract class TestNodeBase : ITestNode
    {
        public string Name { get; }

        protected TestNodeBase([NotNull] string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        public string FullName => $"Full name of {Name}";
    }
}