using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal class BackgroundView : CanvasObject {
        private readonly AxisRenderer _axisRenderer = new AxisRenderer();
        private readonly GridRenderer _gridRenderer = new GridRenderer();

        public override bool CheckInbound(Vector2 positionWS) {
            return false;
        }

        public override IEnumerable<IRenderer> GetRenderers() {
            yield return _gridRenderer;
            yield return _axisRenderer;
        }
    }
}
