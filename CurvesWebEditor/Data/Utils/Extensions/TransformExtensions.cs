using System.Numerics;

namespace CurvesWebEditor.Data.Utils.Extensions {
    public static class TransformExtensions {
        public static Vector3 ToVector3(this Vector2 v) {
            return new Vector3(v.X, v.Y, 0f);
        }

        public static Vector3 ToVector3(this Vector2 v, float z) {
            return new Vector3(v.X, v.Y, z);
        }

        public static Vector2 ToVector2(this Vector4 v) {
            return new Vector2(v.X, v.Y);
        }

        public static Vector2 ToVector2(this Vector3 v) {
            return new Vector2(v.X, v.Y);
        }
    }
}
