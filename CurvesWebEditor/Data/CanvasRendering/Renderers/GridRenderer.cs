using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.Utils;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    public sealed class GridRenderer : IRenderer {
        public async ValueTask Render(CanvasRenderContext context) {
            const float gridStep = 50f;
            var cameraTRS = context.Camera.WorldToViewMatrix;
            Vector2 point;

            float startX = -MathF.Ceiling(MathF.Abs(context.Camera.LeftTopWS.X) / gridStep) * gridStep;
            float startY = -MathF.Ceiling(MathF.Abs(context.Camera.LeftTopWS.Y) / gridStep) * gridStep;
            float endX = MathF.Ceiling(MathF.Abs(context.Camera.RightBottomWS.X) / gridStep) * gridStep;
            float endY = MathF.Ceiling(MathF.Abs(context.Camera.RightBottomWS.Y) / gridStep) * gridStep;
           // Console.WriteLine($"Grid: {context.Camera.Position} : {context.Camera.LeftTopWS}; {context.Camera.RightBottomWS};");

            await context.Canvas.BeginPathAsync();
            for (float x = startX; x <= endX; x += gridStep) {
                point = TransformUtils.Transform(new Vector2(x, startY), cameraTRS);
                await context.Canvas.MoveToAsync(point.X, point.Y);

                point = TransformUtils.Transform(new Vector2(x, endY), cameraTRS);
                await context.Canvas.LineToAsync(point.X, point.Y);
            }

            for (float y = startY; y <= endY; y += gridStep) {
                point = TransformUtils.Transform(new Vector2(startX, y), cameraTRS);
                await context.Canvas.MoveToAsync(point.X, point.Y);

                point = TransformUtils.Transform(new Vector2(endX, y), cameraTRS);
                await context.Canvas.LineToAsync(point.X, point.Y);
            }

            await context.Canvas.SetLineWidthAsync(1f);
            await context.Canvas.SetLineCapAsync(LineCap.Butt);
            await context.Canvas.SetStrokeStyleAsync("#8a8a8a");
            await context.Canvas.StrokeAsync();
        }
    }
}
