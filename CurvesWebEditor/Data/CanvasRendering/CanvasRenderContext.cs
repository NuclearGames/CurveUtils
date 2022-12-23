using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Utils;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class CanvasRenderContext {
        internal Canvas2DContext Canvas { get; init; }
        internal ViewportData Viewport { get; } = new ViewportData();
        internal CameraData Camera { get; } = new CameraData();

        public CanvasRenderContext(Canvas2DContext canvas) {
            Canvas = canvas;
        }
    }
}
