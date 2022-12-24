using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Utils {
    public sealed class CameraData {
        internal Vector2 PositionWS { get; set; } = Vector2.Zero;
        internal float Scale { get; set; } = 1f;
    }
}
