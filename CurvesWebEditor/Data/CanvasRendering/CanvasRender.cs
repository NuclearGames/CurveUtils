using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Managers;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class CanvasRender {
        private readonly CanvasRenderContext _context;
        private readonly ObjectsContext _objectsContext;
        private bool _moveCamera;

        public CanvasRender(Canvas2DContext canvas, int viewportWidth, int viewportHeight) {
            _context = new CanvasRenderContext(canvas);
            _objectsContext = new ObjectsContext();
            _context.Viewport.Set(viewportWidth, viewportHeight);
        }

        public void Resize(int viewportWidth, int viewportHeight) {
            _context.Viewport.Set(viewportWidth, viewportHeight);
        }

        public async ValueTask Render(float deltaTime) {
            _context.Input.Setup();
            await _objectsContext.RenderersManager.Render(_context);
        }

        public void OnPointerMove(int viewportX, int viewportY) {
            _context.UserInput.SetPointerPositionSS(new Vector2(viewportX, viewportY));

            if (_moveCamera) {
                _context.Camera.PositionWS -= _context.UserInput.PointerDeltaWS;
            }

            _objectsContext.InteractableManager.OnPointerMove(_context.UserInput.PointerPositionWS);
            _objectsContext.CurveEditor.OnPointerMove(_context.UserInput.PointerPositionWS);
        }

        public void OnPointerDown(int button, bool shift, bool alt) {
            switch (button) {
                case 1:
                    _moveCamera = true;
                    break;
            }

            _objectsContext.InteractableManager.OnPointerDown(_context, button, shift, alt);
            _objectsContext.CurveEditor.OnPointerDown(_context, button, shift, alt);
        }

        public void OnPointerUp(int button, bool shift, bool alt) {
            switch (button) {
                case 1:
                    _moveCamera = false;
                    break;
            }

            _objectsContext.InteractableManager.OnPointerUp(_context, button, shift, alt);
            _objectsContext.CurveEditor.OnPointerUp(_context, button, shift, alt);
        }

        public void OnWheel(float deltaY, bool shift, bool alt) {
            float delta = deltaY * 0.0005f;
            float scale = 1 + delta;

            _context.Camera.Scale *= scale;
        }
    }
}
