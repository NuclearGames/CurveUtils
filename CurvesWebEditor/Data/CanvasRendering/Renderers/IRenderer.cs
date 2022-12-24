using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers {
    internal interface IRenderer {
        ValueTask Render(CanvasRenderContext context);
    }
}
