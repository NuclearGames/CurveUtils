using Blazor.Extensions.Canvas.Canvas2D;
using Curves;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    internal sealed class CurveRenderer : IRenderer {
        private const float XERROR = 0.001f;

        internal ICurve? Curve { get; set; }
        internal float FromX { get; set; } = 0f;
        internal float ToX { get; set; } = 1f;
        internal float Step { get; set; } = 0.05f;

        public async ValueTask Render(CanvasRenderContext context) {
            if(Curve == null) {
                return;
            }

            await context.Canvas.BeginPathAsync();

            Task Draw(float x, bool first = false) {
                float y = Curve.Evaluate(x);
                var point = context.Transformer.Point(new Vector2(x, y));

                if (first) {
                    return context.Canvas.MoveToAsync(point.X, point.Y);
                } else {
                    return context.Canvas.LineToAsync(point.X, point.Y);
                }
            }

            await Draw(FromX, true);
            for(float x = FromX + Step; x < ToX; x += Step) {
                await Draw(x);
            }
            await Draw(ToX);

            await context.Canvas.SetLineWidthAsync(2);
            await context.Canvas.SetLineCapAsync(LineCap.Round);
            await context.Canvas.SetStrokeStyleAsync("#0000FF");
            await context.Canvas.StrokeAsync();
        }
    }
}
