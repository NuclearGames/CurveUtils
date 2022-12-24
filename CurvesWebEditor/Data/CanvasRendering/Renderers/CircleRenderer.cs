using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    internal sealed class CircleRenderer : IRenderer {
        internal Vector2 Position { get; set; }
        internal float Radius { get; set; }
        internal string Color { get; set; } = "#000000";

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
