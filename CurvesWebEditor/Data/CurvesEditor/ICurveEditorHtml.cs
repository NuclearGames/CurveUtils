using System;
using System.Numerics;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal interface ICurveEditorHtml {
        event Action onAxisAspectsChanged;
        event Action onDrawScaledCurveChanged;
        Vector2 AxisAspects { get; }
        bool DrawScaledCurve { get; }
    }
}
