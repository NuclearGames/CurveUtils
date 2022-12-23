using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    public interface IRenderer {
        ValueTask Render(CanvasRenderContext context);
    }
}
