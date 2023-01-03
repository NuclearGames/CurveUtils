using System;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Utils {
    public sealed class CameraData {
        private const float MIN_SCALE = 0.05f;
        private const float MAX_SCALE = 10f;

        internal Vector2 PositionWS { get; set; } = new Vector2(0.5f, 0.5f);
        internal float Scale {
            get => _scale; 
            set {
                _scale = MathF.Max(MIN_SCALE, MathF.Min(MAX_SCALE, value));
            }
        }

        private float _scale = 1f;
    }
}
