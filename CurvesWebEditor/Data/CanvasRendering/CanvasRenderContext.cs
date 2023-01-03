using Blazor.Extensions.Canvas.Canvas2D;
using CurvesWebEditor.Data.CanvasRendering.Utils;

namespace CurvesWebEditor.Data.CanvasRendering {
    internal sealed class CanvasRenderContext {
        internal Canvas2DContext Canvas { get; }
        internal RenderInput Input { get; }
        internal CanvasTransformer Transformer { get; }
        internal UserInputData UserInput { get; }
        internal ViewportData Viewport { get; } = new ViewportData();
        internal CameraData Camera { get; } = new CameraData();

        internal CanvasRenderContext(Canvas2DContext canvas) {
            Canvas = canvas;
            Input = new RenderInput(this);
            Transformer = new CanvasTransformer(this);
            UserInput = new UserInputData(this);
        }
    }
}
