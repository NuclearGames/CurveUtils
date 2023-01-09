using System;
using System.Numerics;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal interface ICurveEditorHtml {
        event Action onAxisAspectsChanged;
        event Action onDrawScaledCurveChanged;
        event Action onSelectedVertexValuesChanged;
        event Action onLockXAxisChanged;
        Vector2 AxisAspects { get; set; }
        bool DrawScaledCurve { get; }
        Vector2 SelectedVertexPosition { get; set; }
        float SelectedVertexAngle { get; set; }
        bool LockXAxis { get; set; }
    }
}
