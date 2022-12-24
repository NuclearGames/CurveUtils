using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System.Collections.Generic;

namespace CurvesWebEditor.Data.CanvasRendering.Views {
    internal interface IView {
        IEnumerable<IRenderer> GetRenderers();
    }
}
