using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Views {
    internal sealed class GridView : IRenderer {
        public async ValueTask Render(CanvasRenderContext context) {
            const float gridStep = 0.1f;
            Vector2 point;

            float startX = -MathF.Ceiling(MathF.Abs(context.Input.LeftBottomWS.X) / gridStep) * gridStep;
            float endX = MathF.Ceiling(MathF.Abs(context.Input.RightTopWS.X) / gridStep) * gridStep;
            float startY = -MathF.Ceiling(MathF.Abs(context.Input.LeftBottomWS.Y) / gridStep) * gridStep;
            float endY = MathF.Ceiling(MathF.Abs(context.Input.RightTopWS.Y) / gridStep) * gridStep;
            // Console.WriteLine($"Grid: {context.Camera.Position} : {context.Camera.LeftTopWS}; {context.Camera.RightBottomWS};");

            await context.Canvas.BeginPathAsync();
            for (float x = startX; x <= endX; x += gridStep) {
                point = context.Transformer.Point(new Vector2(x, startY));
                await context.Canvas.MoveToAsync(point.X, point.Y);

                point = context.Transformer.Point(new Vector2(x, endY));
                await context.Canvas.LineToAsync(point.X, point.Y);
            }

            for (float y = startY; y <= endY; y += gridStep) {
                point = context.Transformer.Point(new Vector2(startX, y));
                await context.Canvas.MoveToAsync(point.X, point.Y);

                point = context.Transformer.Point(new Vector2(endX, y));
                await context.Canvas.LineToAsync(point.X, point.Y);
            }

            await context.Canvas.SetLineWidthAsync(1f);
            await context.Canvas.SetLineCapAsync(LineCap.Butt);
            await context.Canvas.SetStrokeStyleAsync("#8a8a8a");
            await context.Canvas.StrokeAsync();
        }
    }
}
