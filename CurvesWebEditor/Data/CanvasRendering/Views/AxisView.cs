using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Views {
    internal class AxisView : IRenderer {
        public async ValueTask Render(CanvasRenderContext context) {
            var fromX = context.Transformer.Point(new Vector2(context.Input.LeftBottomWS.X, 0));
            var toX = context.Transformer.Point(new Vector2(context.Input.RightTopWS.X, 0f));
            var fromY = context.Transformer.Point(new Vector2(0f, context.Input.LeftBottomWS.Y));
            var toY = context.Transformer.Point(new Vector2(0f, context.Input.RightTopWS.Y));

            await context.Canvas.BeginPathAsync();
            await context.Canvas.MoveToAsync(fromX.X, fromX.Y);
            await context.Canvas.LineToAsync(toX.X, toX.Y);
            await context.Canvas.MoveToAsync(fromY.X, fromY.Y);
            await context.Canvas.LineToAsync(toY.X, toY.Y);

            await context.Canvas.SetLineWidthAsync(2f);
            await context.Canvas.SetLineCapAsync(LineCap.Butt);
            await context.Canvas.SetStrokeStyleAsync("#000000");
            await context.Canvas.StrokeAsync();
        }
    }
}
