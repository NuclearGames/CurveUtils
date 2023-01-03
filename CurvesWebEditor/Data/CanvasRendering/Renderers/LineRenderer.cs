using System.Numerics;
using System.Threading.Tasks;
using System;
using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers
{
    internal class LineRenderer : IRenderer {
        internal Vector2 From => _getFrom();
        internal Vector2 To => _getTo();
        internal float Width => _getWidth();
        internal string Color => _getColor();

        private readonly Func<Vector2> _getFrom;
        private readonly Func<Vector2> _getTo;
        private readonly Func<float> _getWidth;
        private readonly Func<string> _getColor;

        public LineRenderer(Func<Vector2> getFrom, Func<Vector2> getTo, Func<float> getWidth, Func<string> getColor) {
            _getFrom = getFrom;
            _getTo = getTo;
            _getWidth = getWidth;
            _getColor = getColor;
        }

        public async ValueTask Render(CanvasRenderContext context) {
            var fromSS = context.Transformer.Point(From);
            var toSS = context.Transformer.Point(To);
            var widthSS = context.Transformer.Size(Width);

            await context.Canvas.BeginPathAsync();
            await context.Canvas.MoveToAsync(fromSS.X, fromSS.Y);
            await context.Canvas.LineToAsync(toSS.X, toSS.Y);
            await context.Canvas.SetLineWidthAsync(widthSS);
            await context.Canvas.SetLineCapAsync(LineCap.Round);
            await context.Canvas.SetStrokeStyleAsync(Color);
            await context.Canvas.StrokeAsync();
        }
    }
}
