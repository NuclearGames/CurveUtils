using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal class BackgroundView : CanvasObject {
        private readonly AxisRenderer _axisRenderer = new AxisRenderer("#000000", 0.0075f);
        private readonly GridRenderer _gridRenderer = new GridRenderer(0.1f, "#8a8a8a", 0.0025f);
        private readonly GridRenderer _gridWholeRenderer = new GridRenderer(1f, "#4a4848", 0.005f);

        private readonly TextRenderer _xTextRenderer = new TextRenderer() { 
            Position = new Vector2(1 - 0.05f, -0.05f),
            Text = "1"
        };
        private readonly TextRenderer _yTextRenderer = new TextRenderer() {
            Position = new Vector2(-0.05f, 1f - 0.05f),
            Text = "1"
        };
        private readonly TextRenderer _zeroTextRenderer = new TextRenderer() {
            Position = new Vector2(-0.05f, -0.05f),
            Text = "0"
        };

        public override bool CheckInbound(Vector2 positionWS) {
            return false;
        }

        public override IEnumerable<IRenderer> GetRenderers() {
            yield return _gridRenderer;
            yield return _gridWholeRenderer;
            yield return _axisRenderer;
            yield return _xTextRenderer;
            yield return _yTextRenderer;
            yield return _zeroTextRenderer;
        }
    }
}
