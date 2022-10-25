using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BezierCurveLib {

    public class Bezier {
        
        public List<Vector2> Points = new List<Vector2>();

        private List<Vector2> _curve = new List<Vector2>();

        public Bezier(List<Vector2> points) {
            Points = points;
        }

        #region Calculation

        public List<Vector2> GetBezierCurve(float step) {

            if(Points.Count == 0) {
                return new List<Vector2>();
            }

            List<Vector2> result = new List<Vector2>();

            _curve.AddRange(Points);

            for (float t = 0; t < 1; t += step) {
                result.Add(GetPoint(_curve.Count - 1, t));
            }

            _curve.Clear();

            return result;
        }

        public float GetY(float x, List<Vector2> points) {

            for (int i = 0; i < points.Count-1; i++) {

                if (points[i].X <= x && x <= points[i + 1].X) {

                    float deltaX = x - points[i].X;
                    float k = ((points[i + 1].Y - points[i].Y) / (points[i + 1].X - points[i].X));

                    return (float)Math.Round(deltaX * k + points[i].Y);

                }

            }

            return 0;
        }

        private Vector2 GetPoint(int uBound, float t) {

            if (uBound == 0) {
                return _curve[0];
            }

            for (int i = 0; i < uBound; i++) {
                Vector2 q = Vector2.Lerp(_curve[i], _curve[i + 1], t);
                _curve[i] = q;
            }

            return GetPoint(uBound - 1, t);
        }

        #endregion
    }
}