using Curves;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System.Collections.Generic;
using System.Numerics;
using Curves.Interfaces;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal class CurveView : CanvasObject {
        private readonly CurveRenderer _renderer;
        internal float Width { get; set; } = 0.005f;
        internal string Color { get; set; } = "#0000FF";

        public CurveView() {
            _renderer = new CurveRenderer(() => Width, () => Color);
        }

        internal void SetCurve(ICurve? curve, float fromX, float toX) {
            _renderer.Curve = curve;
            _renderer.FromX = fromX;
            _renderer.ToX = toX;
        }

        public override bool CheckInbound(Vector2 positionWS) {
            return false;
        }

        public override IEnumerable<IRenderer> GetRenderers() {
            yield return _renderer;
        }
    }
}
