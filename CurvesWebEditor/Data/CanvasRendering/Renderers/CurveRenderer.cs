using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.Utils;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    public sealed class CurveRenderer : IRenderer {
        private readonly List<Vector2> _points;

        public CurveRenderer() {
            _points = new List<Vector2>();
            _points.Add(new Vector2(0, 0));
            _points.Add(new Vector2(500, 500));
            _points.Add(new Vector2(500, 600));
            _points.Add(new Vector2(800, 600));
        }

        public async ValueTask Render(CanvasRenderContext context) {
            if(_points.Count < 2) {
                return;
            }
            
            await context.Canvas.BeginPathAsync();

            var point = TransformUtils.Transform(_points[0], context.Camera.WorldToViewMatrix);
            await context.Canvas.MoveToAsync(point.X, point.Y);
            for(int i = 1; i < _points.Count; i++) {
                point = TransformUtils.Transform(_points[i], context.Camera.WorldToViewMatrix);
                await context.Canvas.LineToAsync(point.X, point.Y);
            }

            await context.Canvas.SetLineWidthAsync(2);
            await context.Canvas.SetLineCapAsync(LineCap.Round);
            await context.Canvas.SetStrokeStyleAsync("#FF0000");
            await context.Canvas.StrokeAsync();
        }
    }
}
