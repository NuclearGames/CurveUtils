using CurvesWebEditor.Data.CanvasRendering;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal interface ICurveEditorPage {
        CanvasRender? Render { get; }

        ValueTask<T> InvokeJS<T>(string name, params T[] args);
        void Refresh();
    }
}
