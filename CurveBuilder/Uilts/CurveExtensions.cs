using System.Linq;
using BezierCurveLib;

namespace CurveBuilder.Uilts; 

internal static class CurveExtensions {
    internal static float MaxX(this BezierCurve curve) => curve.Points.Max(v => v.X);
    internal static float MinX(this BezierCurve curve) => curve.Points.Min(v => v.X);
}