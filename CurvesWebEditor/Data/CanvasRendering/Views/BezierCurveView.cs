using Curves;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System.Linq;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Views {
    internal class BezierCurveView : IRenderer {
        private readonly BezierCurve _curve;
        private readonly float _minX;
        private readonly float _maxX;

        public BezierCurveView(BezierCurve curve) {
            _curve = curve;
            _minX = _curve.Points.Min(x => x.X);
            _maxX = _curve.Points.Max(x => x.X);
        }

        public async ValueTask Render(CanvasRenderContext context) {
            /*  if (_curve.Points.Count < 2) {
                  return;
              }
              await context.Canvas.BeginPathAsync();

              Vector2 point = TransformUtils.Transform(new Vector2(_minX, _curve.Evaluate(_minX)), context.Camera.WorldToViewMatrix);
              await context.Canvas.MoveToAsync(point.X, point.Y);
              for (float x = _minX + 0.1f; x <= _maxX; x += 0.1f) {
                  point = TransformUtils.Transform(new Vector2(x, _curve.Evaluate(x)), context.Camera.WorldToViewMatrix);
                  await context.Canvas.LineToAsync(point.X, point.Y);
              }

              await context.Canvas.SetLineWidthAsync(3);
              await context.Canvas.SetLineCapAsync(LineCap.Round);
              await context.Canvas.SetStrokeStyleAsync("#FF0000");
              await context.Canvas.StrokeAsync();*/
        }
    }
}
