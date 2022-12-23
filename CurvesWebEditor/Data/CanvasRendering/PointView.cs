using CurvesWebEditor.Data.CanvasRendering.Renderers;
using CurvesWebEditor.Data.Utils;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class PointView : IRenderer {
        private const float RADIUS = 50f;
        private const float RADIUS_SQR = RADIUS * RADIUS;

        internal Vector2 Position { get; set; }
        internal bool Selected { get; set; }

        internal bool CheckPointInsize(Vector2 point) {
            var localPos = point - Position;
            float sqrDist = localPos.LengthSquared();
            Console.WriteLine($"CPI {Position} ::: {point} [{localPos}] ({sqrDist}/{RADIUS_SQR})");
            return sqrDist <= RADIUS_SQR;
        }

        public async ValueTask Render(CanvasRenderContext context) {
            var posVS = TransformUtils.Transform(Position, context.Camera.WorldToViewMatrix);
            var radiusVS = TransformUtils.Scale(RADIUS, context.Camera.WorldToViewMatrix);

            await context.Canvas.BeginPathAsync();

            await context.Canvas.ArcAsync(posVS.X, posVS.Y, radiusVS, 0f, 360f);

            await context.Canvas.SetFillStyleAsync(Selected ? "#00ff00" : "#0fff00");
            await context.Canvas.FillAsync();
        }
    }
}
