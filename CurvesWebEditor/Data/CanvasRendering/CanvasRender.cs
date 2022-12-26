using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Managers;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class CanvasRender {
        internal readonly CanvasRenderContext RenderContext;
        internal readonly ObjectsContext ObjectsContext;
        private bool _moveCamera;

        public CanvasRender(Canvas2DContext canvas, int viewportWidth, int viewportHeight) {
            RenderContext = new CanvasRenderContext(canvas);
            ObjectsContext = new ObjectsContext();
            RenderContext.Viewport.Set(viewportWidth, viewportHeight);
        }

        public void Resize(int viewportWidth, int viewportHeight) {
            RenderContext.Viewport.Set(viewportWidth, viewportHeight);
        }

        public async ValueTask Render(float deltaTime) {
            RenderContext.Input.Setup();
            await ObjectsContext.RenderersManager.Render(RenderContext);
        }

        public void OnPointerMove(int viewportX, int viewportY) {
            RenderContext.UserInput.SetPointerPositionSS(new Vector2(viewportX, viewportY));

            if (_moveCamera) {
                RenderContext.Camera.PositionWS -= RenderContext.UserInput.PointerDeltaWS;
            }

            ObjectsContext.InteractableManager.OnPointerMove(RenderContext.UserInput.PointerPositionWS);
            ObjectsContext.CurveEditor.OnPointerMove(RenderContext.UserInput.PointerPositionWS);
        }

        public void OnPointerDown(int button, bool shift, bool alt) {
            switch (button) {
                case 1:
                    _moveCamera = true;
                    break;
            }

            ObjectsContext.InteractableManager.OnPointerDown(RenderContext, button, shift, alt);
            ObjectsContext.CurveEditor.OnPointerDown(RenderContext, button, shift, alt);
        }

        public void OnPointerUp(int button, bool shift, bool alt) {
            switch (button) {
                case 1:
                    _moveCamera = false;
                    break;
            }

            ObjectsContext.InteractableManager.OnPointerUp(RenderContext, button, shift, alt);
            ObjectsContext.CurveEditor.OnPointerUp(RenderContext, button, shift, alt);
        }

        public void OnWheel(float deltaY, bool shift, bool alt) {
            float delta = deltaY * 0.0005f;
            float scale = 1 + delta;

            RenderContext.Camera.Scale *= scale;
        }
    }
}
