using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Managers {
    internal sealed class RenderersManager {
        private readonly ObjectsContext _context;

        public RenderersManager(ObjectsContext context) {
            _context = context;
        }

        internal async ValueTask Render(CanvasRenderContext context) {
            foreach(var obj in _context.Objects) {
                foreach (var renderer in obj.GetRenderers()) {
                    await renderer.Render(context);
                }
            }
        }
    }
}
