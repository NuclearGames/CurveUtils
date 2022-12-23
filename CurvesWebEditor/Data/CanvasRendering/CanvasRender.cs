using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using CurvesWebEditor.Data.Utils;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class CanvasRender {
        private readonly CanvasRenderContext _context;
        private Vector2 _pointerPositionSS;
        private Vector2 _pointerPoisitionNDC;
        private GridRenderer _grid = new GridRenderer();
        private CurveRenderer _curve = new CurveRenderer();
        private bool _lmbPressed;

        public CanvasRender(Canvas2DContext canvas, int viewportWidth, int viewportHeight) {
            _context = new CanvasRenderContext(canvas);
            _context.Viewport.Width = viewportWidth;
            _context.Viewport.Height = viewportHeight;
        }

        public void Resize(int viewportWidth, int viewportHeight) {
            _context.Viewport.Width = viewportWidth;
            _context.Viewport.Height = viewportHeight;
        }

        public async ValueTask Render(float deltaTime) {
            _context.Camera.UpdateParams(_context);

            await _context.Canvas.BeginBatchAsync();

            await _context.Canvas.ClearRectAsync(0, 0, _context.Viewport.Width, _context.Viewport.Height);
            await _context.Canvas.SetFillStyleAsync("#ffe6e6");
            await _context.Canvas.FillRectAsync(0, 0, _context.Viewport.Width, _context.Viewport.Height);

            await _grid.Render(_context);
            await _curve.Render(_context);

            /*  await _context.Canvas.SetStrokeStyleAsync("#1306c7");

              await _context.Canvas.BeginPathAsync();
              await _context.Canvas.ArcAsync(200, 200, 100, 0, 2 * Math.PI, false);
              await _context.Canvas.SetFillStyleAsync("#000000");
              await _context.Canvas.FillAsync();
              await _context.Canvas.StrokeAsync();

              await _context.Canvas.SetLineWidthAsync(15);
              await _context.Canvas.SetLineCapAsync(LineCap.Round);

              await _context.Canvas.BeginPathAsync();
              await _context.Canvas.LineToAsync(0, 0);
              await _context.Canvas.LineToAsync(_context.ViewportWidth, _context.ViewportHeight);
              await _context.Canvas.SetFillStyleAsync("#c70606");
              await _context.Canvas.FillAsync();
              await _context.Canvas.StrokeAsync();

              */
            await _context.Canvas!.EndBatchAsync();
        }

        public void OnPointerMove(int viewportX, int viewportY) {
            var positionSS = new Vector2(viewportX, viewportY);

        /*    var positionNDC = new Vector2(
                (viewportX - _context.ViewportWidth) / (float)_context.ViewportWidth,
                viewportY / (float)_context.ViewportHeight);
*/

            var delta = new Vector2(
                positionSS.X - _pointerPositionSS.X,
                positionSS.Y - _pointerPositionSS.Y);

      /*      _pointerPoisitionNDC = positionNDC;*/
            _pointerPositionSS = positionSS;

            if (_lmbPressed) {
                _context.Camera.Position -= delta;
            }
        }

        public void OnPointerDown(int button, bool shift, bool alt) {
            Console.WriteLine($"Pointer");
            if(button == 0) {
                _lmbPressed = true;
            }
        }

        public void OnPointerUp(int button, bool shift, bool alt) {
            switch (button) {
                case 0:
                    _lmbPressed = false;
                    break;
            }
        }

        public void OnWheel(float deltaY, bool shift, bool alt) {
            float delta = deltaY * 0.0005f;
            float scale = 1 - delta;

            var trs = TransformUtils.ZoomTo(_context.Camera.Position, scale);
            _context.Camera.Scale *= scale;
            _context.Camera.Position = TransformUtils.Transform(_context.Camera.Position, trs);


            _context.Camera.Scale = MathF.Max(0.05f, _context.Camera.Scale);
            _context.Camera.Scale = MathF.Min(10f, _context.Camera.Scale);

            Console.WriteLine($"Wheel: {deltaY}; {_context.Camera.Scale}");
        }
    }
}
