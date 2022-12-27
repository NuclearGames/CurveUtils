using Blazor.Extensions.Canvas.Canvas2D;
using Curves;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    internal sealed class CurveRenderer : IRenderer {
        internal ICurve? Curve { get; set; }
        internal float FromX { get; set; } = 0f;
        internal float ToX { get; set; } = 1f;
        internal float Step { get; set; } = 0.025f;
        internal float Width => _getWidth();
        internal string Color => _getColor();

        private readonly Func<float> _getWidth;
        private readonly Func<string> _getColor;

        public CurveRenderer(Func<float> getWidth, Func<string> getColor) {
            _getWidth = getWidth;
            _getColor = getColor;
        }

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

            float widthSS = context.Transformer.Size(Width);
            await context.Canvas.SetLineWidthAsync(widthSS);
            await context.Canvas.SetLineCapAsync(LineCap.Round);
            await context.Canvas.SetStrokeStyleAsync(Color);
            await context.Canvas.StrokeAsync();
        }
    }
}
