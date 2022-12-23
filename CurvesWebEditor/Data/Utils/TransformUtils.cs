using CurvesWebEditor.Data.Utils.Extensions;
using System;
using System.Numerics;

namespace CurvesWebEditor.Data.Utils {
    public static class TransformUtils {
        public const float Rad2Deg = 180f / MathF.PI;
        public const float Deg2Rad = MathF.PI / 180f;

        public static Matrix4x4 TRS(Vector2 position, float rotation, Vector2 scale) {
            var t = Matrix4x4.CreateTranslation(position.ToVector3());
            var r = Matrix4x4.CreateRotationZ(rotation * Rad2Deg);
            var s = Matrix4x4.CreateScale(scale.ToVector3());
            return s * r * t;
        }

        public static Matrix4x4 ZoomTo(Vector2 position, float scale) {
            var t = Matrix4x4.CreateTranslation(position.ToVector3());
            var s = Matrix4x4.CreateScale((Vector2.One * scale).ToVector3());
            var tinv = Matrix4x4.CreateTranslation((-position).ToVector3());
            return t * s * tinv;
        }

        public static Vector2 Transform(Vector2 vector, Matrix4x4 matrix) {
            return Vector4.Transform(vector, matrix).ToVector2();
        }

        public static Vector2 Scale(Vector2 vector, Matrix4x4 matrix) {
            return Vector4.Transform(new Vector4(vector.X, vector.Y, 0, 0), matrix).ToVector2();
        }

        public static float Scale(float value, Matrix4x4 matrix) {
            return Vector4.Transform(new Vector4(value, 0, 0, 0), matrix).X;
        }
    }
}
