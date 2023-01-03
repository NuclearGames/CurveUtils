using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using System.Numerics;
using System.Threading.Tasks;
using System;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    internal class GridRenderer : IRenderer {
        private readonly float _step;
        private readonly string _color;
        private readonly float _width;

        public GridRenderer(float step, string color, float width) {
            _step = step;
            _color = color;
            _width = width;
        }

        public async ValueTask Render(CanvasRenderContext context) {
            Vector2 point;

            float startX = -MathF.Ceiling(MathF.Abs(context.Input.LeftBottomWS.X) / _step) * _step;
            float endX = MathF.Ceiling(MathF.Abs(context.Input.RightTopWS.X) / _step) * _step;
            float startY = -MathF.Ceiling(MathF.Abs(context.Input.LeftBottomWS.Y) / _step) * _step;
            float endY = MathF.Ceiling(MathF.Abs(context.Input.RightTopWS.Y) / _step) * _step;
            // Console.WriteLine($"Grid: {context.Camera.Position} : {context.Camera.LeftTopWS}; {context.Camera.RightBottomWS};");

            await context.Canvas.BeginPathAsync();
            for (float x = startX; x <= endX; x += _step) {
                point = context.Transformer.Point(new Vector2(x, startY));
                await context.Canvas.MoveToAsync(point.X, point.Y);

                point = context.Transformer.Point(new Vector2(x, endY));
                await context.Canvas.LineToAsync(point.X, point.Y);
            }

            for (float y = startY; y <= endY; y += _step) {
                point = context.Transformer.Point(new Vector2(startX, y));
                await context.Canvas.MoveToAsync(point.X, point.Y);

                point = context.Transformer.Point(new Vector2(endX, y));
                await context.Canvas.LineToAsync(point.X, point.Y);
            }

            float widthSS = context.Transformer.Size(_width);
            await context.Canvas.SetLineWidthAsync(widthSS);
            await context.Canvas.SetLineCapAsync(LineCap.Butt);
            await context.Canvas.SetStrokeStyleAsync(_color);
            await context.Canvas.StrokeAsync();
        }
    }
}
