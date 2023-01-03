using System;
using System.Numerics;

namespace TransformStructures {
    public struct Matrix3x3 : IEquatable<Matrix3x3> {
        public float M11, M12, M13;
        public float M21, M22, M23;
        public float M31, M32, M33;

        public Matrix3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33) {
            M11 = m11; M12 = m12; M13 = m13;
            M21 = m21; M22 = m22; M23 = m23;
            M31 = m31; M32 = m32; M33 = m33;
        }

        public Matrix3x3 Transpose() {
            return new Matrix3x3(
                M11, M21, M31,
                M12, M22, M32,
                M13, M23, M33);
        }

        public Matrix3x3 Inverse() {
            float det =
                 M11 * M22 * M33 + M21 * M32 * M13 + M12 * M23 * M31
                - M31 * M22 * M13 - M21 * M12 * M33 - M32 * M23 * M11;

            if (det == 0) {
                return this;
            }

            var adj = new Matrix3x3(
                 M22 * M33 - M32 * M23, -(M21 * M33 - M31 * M23), M21 * M32 - M31 * M22,
                -(M21 * M33 - M32 * M13), M11 * M33 - M31 * M13, -(M11 * M32 - M31 * M12),
                 M12 * M23 - M22 * M13, -(M11 * M23 - M21 * M12), M11 * M22 - M21 * M12);

            return adj.Transpose() * (1 / det);
        }

        public bool Equals(Matrix3x3 other) {
            return M11 == other.M11 && M12 == other.M12 && M13 == other.M13
                && M21 == other.M21 && M22 == other.M22 && M23 == other.M23
                && M31 == other.M31 && M32 == other.M32 && M33 == other.M33;
        }

        public override string ToString() {
            return $"<[{M11}, {M12}, {M13}] [{M21}, {M22}, {M23}] [{M31}, {M32}, {M33}]>";
        }

        public static Matrix3x3 TRS(Vector2 t, float r, Vector2 s) {
            return Scale(s) * Rotation(r) * Translation(t);
        }

        public static Matrix3x3 TranslationScale(Vector2 t, Vector2 s) {
            return new Matrix3x3(
                s.X, 0, t.X,
                0, s.Y, t.Y,
                0, 0, 1);
        }

        public static Matrix3x3 Translation(Vector2 v) {
            return new Matrix3x3(
                1, 0, v.X,
                0, 1, v.Y,
                0, 0, 1);
        }

        public static Matrix3x3 Rotation(float r) {
            r *= MathConstants.Deg2Rad;
            float sin = MathF.Sin(r);
            float cos = MathF.Cos(r);

            return new Matrix3x3(
                cos, -sin, 0,
                sin, cos, 0,
                0, 0, 1);
        }

        public static Matrix3x3 Scale(Vector2 v) {
            return new Matrix3x3(
                v.X, 0, 0,
                0, v.Y, 0,
                0, 0, 1);
        }

        public static Matrix3x3 operator *(Matrix3x3 m, float c) {
            return new Matrix3x3(
                m.M11 * c, m.M12 * c, m.M13 * c,
                m.M21 * c, m.M22 * c, m.M23 * c,
                m.M31 * c, m.M32 * c, m.M33 * c);
        }

        public static Vector3 operator *(Matrix3x3 m, Vector3 v) {
            return new Vector3(
                m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
                m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
                m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z);
        }

        public static Matrix3x3 operator *(Matrix3x3 m, Matrix3x3 o) {
            return new Matrix3x3(
                o.M11 * m.M11 + o.M12 * m.M21 + o.M13 * m.M31,
                o.M11 * m.M12 + o.M12 * m.M22 + o.M13 * m.M32,
                o.M11 * m.M13 + o.M12 * m.M23 + o.M13 * m.M33,

                o.M21 * m.M11 + o.M22 * m.M21 + o.M23 * m.M31,
                o.M21 * m.M12 + o.M22 * m.M22 + o.M23 * m.M32,
                o.M21 * m.M13 + o.M22 * m.M23 + o.M23 * m.M33,

                o.M31 * m.M11 + o.M32 * m.M21 + o.M33 * m.M31,
                o.M31 * m.M12 + o.M32 * m.M22 + o.M33 * m.M32,
                o.M31 * m.M13 + o.M32 * m.M23 + o.M33 * m.M33);
        }
    }
}
