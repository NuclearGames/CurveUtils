using System.Numerics;
using System.Text.Json;

namespace Curves {
    /// <summary>
    /// Набор данных для построения кривой.
    /// </summary>
    public class TangentBasedCurveData {
        internal CurveVertex[]? Vertexes { get; init; }
        internal float XAspect { get; init; }
        internal float YAspect { get; init; }

        internal struct CurveVertex {
            internal Vector2 Position { get; }
            internal float Tangent { get; }
        }

        public static TangentBasedCurveData FromJson(string json) {
            return JsonSerializer.Deserialize<TangentBasedCurveData>(json)!;
        }

        public static string ToJson(TangentBasedCurveData data) {
            return JsonSerializer.Serialize(data);
        }
    }
}
