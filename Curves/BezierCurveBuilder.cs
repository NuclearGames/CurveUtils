using System.Collections.Generic;
using System.Numerics;

namespace Curves {
    public sealed class BezierCurveBuilder {
        /// <summary>
        /// Собирает объект кривой из объекта с опорными точками кривой
        /// </summary>
        public static BezierCurve Build(BezierCurveSourceModel model, in float accuracy) {
            return new BezierCurve(CalculateBezierCurve(model.Nodes, in accuracy));
        }

        /// <summary>
        /// Собирает точки кривой исходя из опорных точек
        /// </summary>
        private static List<Vector2> CalculateBezierCurve(IReadOnlyCollection<Vector2> sourcePoints, in float accuracy) {

            if (sourcePoints.Count == 0) {
                return new List<Vector2>();
            }

            List<Vector2> result = new List<Vector2>();
            List<Vector2> sourceCopy = new List<Vector2>(sourcePoints.Count);

            for (float t = 0; t < 1; t += accuracy) {
                sourceCopy.AddRange(sourcePoints);

                result.Add(GetPoint(sourceCopy, in t));

                sourceCopy.Clear();
            }

            return result;
        }

        /// <summary>
        /// Вычисляет конкретную точку кривой на конкртном шаге
        /// </summary>
        private static Vector2 GetPoint(IList<Vector2> sourceCopy, in float t) {
            int uBound = sourceCopy.Count - 1;
            while (uBound > 0) {
                for (int i = 0; i < uBound; i++) {
                    sourceCopy[i] = Vector2.Lerp(sourceCopy[i], sourceCopy[i + 1], t);
                }

                uBound -= 1;
            }

            return sourceCopy[0];
        }
    }
}
