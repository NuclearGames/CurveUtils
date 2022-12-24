using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Views {
    internal sealed class PointView : IRenderer {
        private const float RADIUS = 0.25f;
        private const float RADIUS_SQR = RADIUS * RADIUS;

        internal Vector2 Position { get; set; }
        internal bool Selected { get; set; }

        internal bool CheckInbound(Vector2 point) {
            var localPos = point - Position;
            float sqrDist = localPos.LengthSquared();
            Console.WriteLine($"CPI {Position} ::: {point} [{localPos}] ({sqrDist}/{RADIUS_SQR})");
            return sqrDist <= RADIUS_SQR;
        }

        public async ValueTask Render(CanvasRenderContext context) {
            var posSS = context.Transformer.Point(Position);
            var radiusSS = context.Transformer.Size(RADIUS);

            await context.Canvas.BeginPathAsync();

            await context.Canvas.ArcAsync(posSS.X, posSS.Y, radiusSS, 0f, 360f);

            await context.Canvas.SetFillStyleAsync(Selected ? "#00ff00" : "#0fff00");
            await context.Canvas.FillAsync();
        }
    }
}
