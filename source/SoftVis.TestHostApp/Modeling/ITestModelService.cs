﻿using Codartis.SoftVis.Modeling;

namespace Codartis.SoftVis.TestHostApp.Modeling
{
    internal interface ITestModelService : IModelService
    {
        TestModelStore TestModelStore { get; }
    }
}
