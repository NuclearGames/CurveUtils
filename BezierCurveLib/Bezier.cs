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

            for (float x = 0; x < 1; x += step) {

                Vector2 point = new Vector2();

                for (int i = 0; i < points.Count; i++) { // проходим по каждой точке

                    point += GetPoint(i, points[i], points.Count, x);

                }

                //result.Add(new Vector2((float)Math.Round(point.X),(float)Math.Round(point.Y)));
                result.Add(point);
            }
            Points = result;
            return result;
        }

        public float GetYFromBezier(float x, List<Vector2> points) {
            for (int i = 0; i < points.Count-1; i++) {
                if (points[i].X <= x && x <= points[i + 1].X) {
                    float deltaX = x - points[i].X;
                    float k = ((points[i + 1].Y - points[i].Y) / (points[i + 1].X - points[i].X));
                    return (float)Math.Round(deltaX * k + points[i].Y);
                }
            }
            return 0;
        }
        private Vector2 GetPoint(int index ,Vector2 point, int pointCount, float x) {

            float bezie = GetPolinom(index, pointCount - 1, x); // вычисляем наш многочлен(полином) Бернштейна
            point *= bezie; 

            return point;
        }

        private BigInteger GetFactorial(int n) {
            int result = 1;
            
            for (int i = 1; i <= n; i++) {
                result *= i;
            }

            return result;
        }

        //называется многочленом(полиномом) Бернштейна степени n
        private float GetPolinom(int k, int n, float x) {

            //называются коэффициентами Бернштейна или коэффициентами Безье
            float bezierK = (float)((GetFactorial(n) / (GetFactorial(k) * GetFactorial(n - k))));
            float result = bezierK * (float)Math.Pow(x, k) * (float)Math.Pow(1 - x, n - k);

            return result;
        }

        #endregion
    }
}