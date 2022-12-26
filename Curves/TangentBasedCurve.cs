using System;
using System.Collections.Generic;
using System.Numerics;

namespace Curves {
    /// <summary>
    /// Кривая, основанная на положениях вершин и направлениях касательных в них.
    /// </summary>
    public sealed class TangentBasedCurve : ICurve {
        private readonly Segment[] _sections;
        private float _xAspect, _yAspect;

        internal TangentBasedCurve(Segment[] sections, float xAspect, float yAspect) {
            _sections = sections;
            _xAspect = xAspect;
            _yAspect = yAspect;
        }

        public float Evaluate(float x) {
            return EvaluateRaw(x / _xAspect) * _yAspect;
        }

        private float EvaluateRaw(float x) {
            // Если x правее самого правого сегмента,
            // используем правую сегмент.
            if (x >= _sections[_sections.Length - 1].RX) {
                return _sections[_sections.Length - 1].Evaluate(x);
            }

            // Идем по каждому сегменту и смотрим:
            // если x <= правой опорной точки, то используем этот сегмент.
            for (int i = 0; i < _sections.Length; i++) {
                if (x <= _sections[i].RX) {
                    return _sections[i].Evaluate(x);
                }
            }

            // Этого наступить не должно.
            throw new ArgumentException();
        }

        /// <summary>
        /// Создает кривую по набору вершин и набору коэффициентов касательной в этих точках.
        /// Одинаковые касательные слева и справа от вершин.
        /// </summary>
        public static TangentBasedCurve FromBasePoints(IReadOnlyList<Vector2> basePoints, IReadOnlyList<float> baseTangents, float xAspect = 1f, float yAspect = 1f) {
            if (basePoints.Count != baseTangents.Count) {
                throw new ArgumentException();
            }

            var sections = new Segment[basePoints.Count-1];

            for(int i = 0; i < sections.Length; i++) {
                var leftPoint = basePoints[i];
                var rightPoint = basePoints[i + 1];
                float leftTangent = baseTangents[i];
                float rightTangent = baseTangents[i + 1];
                sections[i] = Segment.Fit(leftPoint, rightPoint, leftTangent, rightTangent);
            }

            return new TangentBasedCurve(sections, xAspect, yAspect);
        }

        /// <summary>
        /// Создает кривую по набору точек и набору коэффициентов касательной в этих точках.
        /// Разные касательные слева и справа от вершин.
        /// </summary>
        public static TangentBasedCurve FromBasePoints(IReadOnlyList<Vector2> basePoints, IReadOnlyList<Vector2> baseTangents, float xAspect = 1f, float yAspect = 1f) {
            if (basePoints.Count != baseTangents.Count) {
                throw new ArgumentException();
            }

            var sections = new Segment[basePoints.Count - 1];

            for (int i = 0; i < sections.Length; i++) {
                var leftPoint = basePoints[i];
                var rightPoint = basePoints[i + 1];
                float leftTangent = baseTangents[i].Y;
                float rightTangent = baseTangents[i + 1].X;
                sections[i] = Segment.Fit(leftPoint, rightPoint, leftTangent, rightTangent);
            }

            return new TangentBasedCurve(sections, xAspect, yAspect);
        }

        /// <summary>
        /// Создает кривую по объекту данных.
        /// </summary>
        internal static TangentBasedCurve FromData(TangentBasedCurveData data) {
            if(data.Vertexes == null) {
                throw new ArgumentException();
            }

            var sections = new Segment[data.Vertexes.Length - 1];

            for (int i = 0; i < sections.Length; i++) {
                var leftPoint = new Vector2(data.Vertexes[i].PositionX, data.Vertexes[i].PositionY);
                var rightPoint = new Vector2(data.Vertexes[i+1].PositionX, data.Vertexes[i+1].PositionY);
                float leftTangent = data.Vertexes[i].Tangent;
                float rightTangent = data.Vertexes[i + 1].Tangent;
                sections[i] = Segment.Fit(leftPoint, rightPoint, leftTangent, rightTangent);
            }

            return new TangentBasedCurve(sections, data.XAspect, data.YAspect);
        }
        
        /// <summary>
        /// Создает кривую из джейсон строки.
        /// </summary>
        public static TangentBasedCurve FromJson(string json) {
            return FromData(TangentBasedCurveData.FromJson(json));
        }

        public struct Segment {
            // Полином 3 степени.
            public float A, B, C, D;

            // X правой опорной точки.
            public float RX;

            public Segment(float a, float b, float c, float d, float rx) {
                A = a;
                B = b;
                C = c;
                D = d;
                RX = rx;
            }

            public float Evaluate(float x) {
                // Уравнение кубического полинома.
                float xmX = x - RX;
                return A + B * xmX + C * xmX * xmX + D * xmX * xmX * xmX; 
            }

            /// <summary>
            /// Вычисляет коэффициенты кривой.
            /// </summary>
            /// <param name="p0">Левая опорная точка</param>
            /// <param name="p1">Правая опорная точка</param>
            /// <param name="k0">Коэффициент левой касательной</param>
            /// <param name="k1">Коэффициент правой касательной</param>
            /// <returns></returns>
            public static Segment Fit(Vector2 p0, Vector2 p1, float k0, float k1) {
                float invH = p0.X - p1.X;

                // А находится из уравнения полинома при y(p1.X) = p1.Y.
                float a = p1.Y;

                // B находится из уравнения правой касательной.
                // (Уравнение касательной - производная уравнения полинома)
                float b = k1;

                // C и D находятся из системы:
                // C выражается из уравнения полинома при y(p0.X) = p0.Y.
                // D находится из уравнения левой касательной, куда подставляется C.
                float d = (k0 - b - (2*(p0.Y - a - b * invH)/invH)) / (invH * invH);
                float c = (p0.Y - a - b * invH - d * invH * invH * invH)/(invH*invH);

                return new Segment(a, b, c, d, p1.X);
            }
        }
    }
}
