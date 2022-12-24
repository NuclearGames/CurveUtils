using CurvesWebEditor.Data.Utils.Extensions;
using System.Numerics;
using TransformStructures;

namespace CurvesWebEditor.Data.CanvasRendering.Utils {
    internal sealed class RenderInput {
        internal Matrix3x3 WorldToView { get; private set; }
        internal Matrix3x3 ViewToWorld { get; private set; }
        internal Matrix3x3 ViewToScreen { get; private set; }
        internal Matrix3x3 ScreenToView { get; private set; }
        internal Matrix3x3 WorldToScreen { get; private set; }
        internal Matrix3x3 ScreenToWorld { get; private set; }

        internal Vector2 LeftTopWS { get; private set; }
        internal Vector2 RightBottomWS { get; private set; }

        private readonly CanvasRenderContext _context;

        internal RenderInput(CanvasRenderContext context) {
            _context = context;
        }

        internal void Setup() {
            ViewToWorld = Matrix3x3.TranslationScale(_context.Camera.PositionWS, new Vector2(_context.Camera.Scale, _context.Camera.Scale));
            WorldToView = ViewToWorld.Inverse();

            ViewToScreen = Matrix3x3.TranslationScale(
                new Vector2(
                    0.5f * _context.Viewport.Width, 
                    0.5f * _context.Viewport.Height), 

                new Vector2(
                    0.5f * _context.Viewport.Width, 
                    -0.5f * _context.Viewport.Height / _context.Viewport.Aspect));

            ScreenToView = ViewToScreen.Inverse();

            // Должны быть (-1; aspect)
            var leftTopVS = ScreenToView * new Vector3(0, 0, 1);
            // (1; -aspect).
            var rightBottomVS = ScreenToView * new Vector3(_context.Viewport.Width, _context.Viewport.Height, 1);

            LeftTopWS = (ViewToWorld * leftTopVS).ToVector2();
            RightBottomWS = (ViewToWorld * rightBottomVS).ToVector2();

            WorldToScreen = WorldToView * ViewToScreen;
            ScreenToWorld = WorldToScreen.Inverse(); // должен быть равен ViewToWorld * ScreenToView.
        }
    }
}
