using Blazor.Extensions.Canvas.Canvas2D;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class CanvasRenderContext {
#nullable disable
        internal Canvas2DContext Canvas { get; init; }
        internal int ViewportWidth { get; set; }
        internal int ViewportHeight { get; set; }
#nullable restore
    }
}
