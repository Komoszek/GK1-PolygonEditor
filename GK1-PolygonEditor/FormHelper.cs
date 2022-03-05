using System;
using System.Collections.Generic;
using System.Text;

namespace GK1_PolygonEditor
{
    enum EditorState
    {
        Select,
        AddPolygon,
        AddCircle,
        RemoveVertex,
        SplitEdge,
        RemoveConstraint,
        AddConstraint
    }

    enum HoveredObjectType
    {
        None,
        Vertex,
        CircleCenter,
        Polygon,
        Circle,
        Ring,
        Edge

    }

    enum ConstraintType
    {
        None,
        ConstCircleCenter,
        ConstRadius,
        TangentCircle,
        ConstEdgeLength,
        EqualEdges,
        ParallelEdges,
    }
}
