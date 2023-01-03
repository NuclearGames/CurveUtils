using System;
using System.Numerics;
using System.Threading.Tasks;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers
{
    internal sealed class CircleRenderer : IRenderer {
        internal Vector2 Position => _getPosition();
        internal float Radius => _getRadius();
        internal string Color => _getColor();

        private readonly Func<Vector2> _getPosition;
        private readonly Func<float> _getRadius;
        private readonly Func<string> _getColor;

        public CircleRenderer(Func<Vector2> getPosition, Func<float> getRadius, Func<string> getColor) {
            _getPosition = getPosition;
            _getRadius = getRadius;
            _getColor = getColor;
        }

        public async ValueTask Render(CanvasRenderContext context) {
            var posSS = context.Transformer.Point(Position);
            var radiusSS = context.Transformer.Size(Radius);

            await context.Canvas.BeginPathAsync();
            await context.Canvas.ArcAsync(posSS.X, posSS.Y, radiusSS, 0f, 360f);
            await context.Canvas.SetFillStyleAsync(Color);
            await context.Canvas.FillAsync();
        }
    }
}
