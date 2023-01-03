using CurvesWebEditor.Data.CanvasRendering;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal interface ICurveEditorPage {
        CanvasRender? Render { get; }

        ValueTask InvokeJSVoid(string name, params object[] args);
        void Refresh();
    }
}
