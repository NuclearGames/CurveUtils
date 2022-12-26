using System.Text.Json;

namespace Curves {
    /// <summary>
    /// Набор данных для построения кривой.
    /// </summary>
    public class TangentBasedCurveData {
        public CurveVertex[]? Vertexes { get; init; }
        public float XAspect { get; init; }
        public float YAspect { get; init; }

        public struct CurveVertex {
            public float PositionX { get; init; }
            public float PositionY { get; init; }
            public float Tangent { get; init; }
        }

        public static TangentBasedCurveData FromJson(string json) {
            return JsonSerializer.Deserialize<TangentBasedCurveData>(json)!;
        }

        public static string ToJson(TangentBasedCurveData data) {
            return JsonSerializer.Serialize(data);
        }
    }
}
