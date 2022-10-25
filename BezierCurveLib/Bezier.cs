using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BezierCurveLib {

    public class Bezier {
        
        public List<Vector2> Points = new List<Vector2>();

        #region Calculation

        public List<Vector2> GetBezierCurve(List<Vector2> points, float step) {

            List<Vector2> result = new List<Vector2>();

            Points = points;

            for (float t = 0; t < 1; t += step) {
                result.Add(GetPoint(points.Count - 1, t));
            }

            Points = result;

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
                return Points[0];
            }

            for (int i = 0; i < uBound; i++) {
                Vector2 q = Vector2.Lerp(Points[i], Points[i + 1], t);
                Points[i] = q;
            }

            return GetPoint(uBound - 1, t);
        }

        #endregion
    }
}