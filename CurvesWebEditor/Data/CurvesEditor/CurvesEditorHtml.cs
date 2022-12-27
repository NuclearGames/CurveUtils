using Curves;
using Microsoft.JSInterop;
using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal sealed class CurvesEditorHtml : ICurveEditorHtml {
        private readonly ICurveEditorPage _page;
        internal AxisAspectValues AxisAspects = new AxisAspectValues(1f, 1f);
        internal FlagValue DrawScaledCurve = new FlagValue(false);
        internal SelectedVertexValues SelectedVertexValues = new SelectedVertexValues(0f, 0f, 45f);

        public CurvesEditorHtml(ICurveEditorPage page) {
            _page = page;
        }

        internal async Task DownloadResults() {
            var stream = GetDownloadStream();
            string filename = "TangentBaseCurve.json";
            var streamRef = new DotNetStreamReference(stream);
            await _page.InvokeJS<object>("downloadFileFromStream", filename, streamRef);
        }

        private Stream GetDownloadStream() {
            var data = _page.Render!.ObjectsContext.CurveEditor.CreateData();
            var json = TangentBasedCurveData.ToJson(data);
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

#region ICurveEditorHtml

        event Action ICurveEditorHtml.onAxisAspectsChanged {
            add => AxisAspects.onChanged += value;
            remove => AxisAspects.onChanged -= value;
        }

        Vector2 ICurveEditorHtml.AxisAspects => AxisAspects.Value;

        event Action ICurveEditorHtml.onDrawScaledCurveChanged {
            add => DrawScaledCurve.onChanged += value;
            remove => DrawScaledCurve.onChanged -= value;
        }

        bool ICurveEditorHtml.DrawScaledCurve => DrawScaledCurve.Value;

        event Action ICurveEditorHtml.onSelectedVertexValuesChanged {
            add => SelectedVertexValues.onChanged += value;
            remove => SelectedVertexValues.onChanged -= value;
        }

        Vector2 ICurveEditorHtml.SelectedVertexPosition {
            get => SelectedVertexValues.PositionExternal; set {
                SelectedVertexValues.PositionExternal = value;
                _page.Refresh();
            }
        }
        float ICurveEditorHtml.SelectedVertexAngle {
            get => SelectedVertexValues.Angle; set {
                SelectedVertexValues.Angle = value;
                _page.Refresh();
            }
        }

        #endregion
    }
}
