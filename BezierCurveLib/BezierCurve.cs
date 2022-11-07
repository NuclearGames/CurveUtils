using System.Numerics;

namespace BezierCurveLib {
    public class BezierCurve {
        public IReadOnlyList<Vector2> Points => _points;

        private readonly List<Vector2> _points;

        public BezierCurve(List<Vector2> pointsPoints) {
            _points = pointsPoints;
        }

        public List<Vector2> GetBezierCurve() {
            return _points;
        }



#region Calculation

        /// <summary>
        /// Вычисляет значение ординаты по значению абсциссы кривой
        /// </summary>
        public float Evaluate(float x) {
            for (int i = 1; i < _points.Count - 1; i++) {
                
                if (_points[i].X <= x && x <= _points[i + 1].X) {
                    float deltaX = x - _points[i].X;
                    float k = (_points[i + 1].Y - _points[i].Y) / (_points[i + 1].X - _points[i].X);

                    return (deltaX * k + _points[i].Y);
                }

                if (_points[i].X <= x && x <= _points[i - 1].X) {
                    float deltaX = x - _points[i].X;
                    float k = (_points[i].Y - _points[i - 1].Y) / (_points[i - 1].X - _points[i].X);

                    return deltaX * k + _points[i].Y;
                }
            }

            return 0;
        }

#endregion
    }
}