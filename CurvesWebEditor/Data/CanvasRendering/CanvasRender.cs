using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class CanvasRender {
        private readonly CanvasRenderContext _context;

        public CanvasRender(Canvas2DContext canvas, int viewportWidth, int viewportHeight) {
            _context = new CanvasRenderContext() {
                Canvas = canvas,
                ViewportWidth = viewportWidth,
                ViewportHeight = viewportHeight
            };
        }

        public void Resize(int viewportWidth, int viewportHeight) {
            _context.ViewportWidth = viewportWidth;
            _context.ViewportHeight = viewportHeight;
        }

        public async ValueTask Render(float deltaTime) {
            await _context.Canvas.BeginBatchAsync();
            await _context.Canvas.ClearRectAsync(0, 0, _context.ViewportWidth, _context.ViewportHeight);

            await _context.Canvas.SetFillStyleAsync("#ffe6e6");
            await _context.Canvas.FillRectAsync(0, 0, _context.ViewportWidth, _context.ViewportHeight);

            await _context.Canvas.SetStrokeStyleAsync("#1306c7");

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

            await _context.Canvas!.EndBatchAsync();
        }

        public void OnPointerMove(int viewportX, int viewportY) {
            var position = new Vector2(
                (viewportX - _context.ViewportWidth) / (float)_context.ViewportWidth,
                viewportY / (float)_context.ViewportHeight);

          
        }

        public void OnPointerDown(int button, bool shift, bool alt) {
            Console.WriteLine($"Pointer");
        }

        public void OnPointerUp(int button, bool shift, bool alt) {

        }
    }
}
