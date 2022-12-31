using System.Text.Json;

namespace Curves {
    /// <summary>
    /// Набор данных для построения кривой.
    /// </summary>
    public class TangentBasedCurveData {
        /// <summary>
        /// "Вершини" кривой
        /// </summary>
        public CurveVertex[]? Vertexes { get; init; }
        
        /// <summary>
        /// Скейл по Х 
        /// </summary>
        public float XAspect { get; init; }
        
        /// <summary>
        /// Скейл по Y
        /// </summary>
        public float YAspect { get; init; }

        /// <summary>
        /// Данные о "вершине" кривой
        /// </summary>
        public struct CurveVertex {
            /// <summary>
            /// Х "вершины"
            /// </summary>
            public float PositionX { get; init; }
            
            /// <summary>
            /// Y "вершины"
            /// </summary>
            public float PositionY { get; init; }
            
            /// <summary>
            /// Тангенс касательной в точке
            /// </summary>
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
