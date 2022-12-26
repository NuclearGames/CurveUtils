using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Managers {
    internal sealed class RenderersManager {
        private readonly ObjectsContext _context;

        public RenderersManager(ObjectsContext context) {
            _context = context;
        }

        internal async ValueTask Render(CanvasRenderContext context) {
            await context.Canvas.BeginBatchAsync();

            await context.Canvas.ClearRectAsync(0, 0, context.Viewport.Width, context.Viewport.Height);
            await context.Canvas.SetFillStyleAsync("#ffe6e6");
            await context.Canvas.FillRectAsync(0, 0, context.Viewport.Width, context.Viewport.Height);

            foreach (var obj in _context.Objects) {
                foreach (var renderer in obj.GetRenderers()) {
                    await renderer.Render(context);
                }
            }

            await context.Canvas.EndBatchAsync();
        }
    }
}
