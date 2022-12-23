using CurvesWebEditor.Data.Utils;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Utils {
    public sealed class CameraData {
        internal Vector2 Position { get; set; }
        internal float Scale { get; set; } = 1f;
        internal Matrix4x4 WorldToViewMatrix { get; private set; }
        internal Matrix4x4 ViewToWorldMatrix { get; private set; }
        internal Vector2 LeftTopWS { get; private set; }
        internal Vector2 RightBottomWS { get; private set; }

        internal void UpdateParams(CanvasRenderContext context) {
            UpdateTRS();
            UpdateBounds(context);
        }

        private void UpdateTRS() {
            WorldToViewMatrix = TransformUtils.TRS(-Position, 0f, Vector2.One * Scale);
            ViewToWorldMatrix = TransformUtils.TRS(Position, 0f, Vector2.One / Scale);
        }

        private void UpdateBounds(CanvasRenderContext context) {
            LeftTopWS = TransformUtils.Transform(Vector2.Zero, ViewToWorldMatrix);
            RightBottomWS = TransformUtils.Transform(new Vector2(context.Viewport.Width, context.Viewport.Height), ViewToWorldMatrix);
        }
    }
}
