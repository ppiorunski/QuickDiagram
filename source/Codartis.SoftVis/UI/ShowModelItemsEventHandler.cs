﻿using System.Collections.Generic;
using Codartis.SoftVis.Modeling.Definition;

namespace Codartis.SoftVis.UI
{
    public delegate void ShowModelItemsEventHandler(IReadOnlyList<ModelNodeId> modelNodeIds, bool followNewDiagramNodes);
}
