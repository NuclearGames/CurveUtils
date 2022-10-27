using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BezierCurveLib {

    public class Bezier {

        private List<Vector2> _curve = new List<Vector2>();
        private List<Vector2> _nodes = new List<Vector2>();

        private float accuracy;

        public Bezier(BezierNode nodes, float accuracy) {

            _nodes = nodes.Nodes;
            this.accuracy = accuracy;
            CalculateBezierCurve();

        }

        public List<Vector2> GetBezierCurve() {
            return _curve;
        }

        #region Calculation

        public float Evaluate(float x) {

            for (int i = 0; i < _curve.Count - 1; i++) {

                if (_curve[i].X <= x && x <= _curve[i + 1].X) {

                    float deltaX = x - _curve[i].X;
                    float k = ((_curve[i + 1].Y - _curve[i].Y) / (_curve[i + 1].X - _curve[i].X));

                    return (float)Math.Round(deltaX * k + _curve[i].Y);

                }

            }

            return 0;
        }

        private List<Vector2> CalculateBezierCurve() {

            if(_nodes.Count == 0) {
                return new List<Vector2>();
            }

            List<Vector2> result = new List<Vector2>();

            _curve.AddRange(_nodes);

            for (float t = 0; t < 1; t += accuracy) {
                result.Add(GetPoint(_curve.Count - 1, t));
            }

            _curve.Clear();
            _curve.AddRange(result);

            return _curve;
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