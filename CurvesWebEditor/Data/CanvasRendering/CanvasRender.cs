using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Managers;
using CurvesWebEditor.Data.CanvasRendering.Objects;
using CurvesWebEditor.Data.Utils.Extensions;
using System;
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

            _objectsContext.Create(() => new BackgroundView());
        }

        public void Resize(int viewportWidth, int viewportHeight) {
            _context.Viewport.Set(viewportWidth, viewportHeight);
        }

        public void Test() {
            _context.Camera.PositionWS = new Vector2(0.5f, 0f);
            _context.Camera.Scale = 2f;
            _context.Input.Setup();
            Console.WriteLine($"\n\nTest data");
            Console.WriteLine($"Camera: PosWS={_context.Camera.PositionWS}; Scale={_context.Camera.Scale}");
            Console.WriteLine($"Viewport: W={_context.Viewport.Width}; H={_context.Viewport.Height}; Aspect={_context.Viewport.Aspect}");
            Console.WriteLine($"Input:");
            Console.WriteLine($"    WorldToView={_context.Input.WorldToView}");
            Console.WriteLine($"    ViewToWorld={_context.Input.ViewToWorld}");
            Console.WriteLine($"    ViewToScreen={_context.Input.ViewToScreen}");
            Console.WriteLine($"    ScreenToView={_context.Input.ScreenToView}");
            Console.WriteLine($"        WorldToScreen={_context.Input.WorldToScreen}");
            Console.WriteLine($"        ScreenToWorld={_context.Input.ScreenToWorld}");
            Console.WriteLine($"    LeftTopWS={_context.Input.LeftBottomWS}");
            Console.WriteLine($"    RightBottomWS={_context.Input.RightTopWS}");
            Console.WriteLine();

            var v1 = new Vector2(0, 0);
            Console.WriteLine($"Vector: {v1}");
            Console.WriteLine($"    *WorldToScreen={_context.Input.WorldToScreen * v1.ToVector3(1)}");

            var v2 = new Vector2(-1, _context.Viewport.Aspect);
            Console.WriteLine($"Vector: {v2}");
            Console.WriteLine($"    *WorldToScreen={_context.Input.WorldToScreen * v2.ToVector3(1)}");

            var v3 = new Vector2(-210f, 0);
            Console.WriteLine($"Vector: {v3}");
            Console.WriteLine($"    *ScreenToWorld={_context.Input.ScreenToWorld * v3.ToVector3(1)}");
        }

        public async ValueTask Render(float deltaTime) {
            // return;
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
