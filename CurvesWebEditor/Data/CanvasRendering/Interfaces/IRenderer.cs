using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering.Interfaces {
    internal interface IRenderer {
        ValueTask Render(CanvasRenderContext context);
    }
}
