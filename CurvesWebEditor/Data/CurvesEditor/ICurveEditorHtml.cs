using System;
using System.Numerics;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal interface ICurveEditorHtml {
        event Action onAxisAspectsChanged;
        event Action onDrawScaledCurveChanged;
        event Action onSelectedVertexValuesChanged;
        Vector2 AxisAspects { get; }
        bool DrawScaledCurve { get; }
        Vector2 SelectedVertexPosition { get; set; }
        float SelectedVertexAngle { get; set; }
    }
}
