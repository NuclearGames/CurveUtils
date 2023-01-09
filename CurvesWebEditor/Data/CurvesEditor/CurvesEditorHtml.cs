using Curves;
using Microsoft.AspNetCore.Components.Forms;
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
        internal FlagValue LockXAxis = new FlagValue(true);

        public CurvesEditorHtml(ICurveEditorPage page) {
            _page = page;
        }

        internal async Task UploadFile(InputFileChangeEventArgs e) {
            if (e.FileCount != 1) {
                return;
            }
            foreach (var file in e.GetMultipleFiles(1)) {
                using var stream = file.OpenReadStream();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                CreateCurveFromStream(ms);
            }
        }

        internal async Task DownloadResults() {
            var stream = GetDownloadStream();
            string filename = "TangentBaseCurve.json";
            var streamRef = new DotNetStreamReference(stream);
            await _page.InvokeJSVoid("downloadFileFromStream", filename, streamRef);
        }

        private void CreateCurveFromStream(MemoryStream stream) {
            var json = Encoding.UTF8.GetString(stream.ToArray());
            var data = TangentBasedCurveData.FromJson(json);
            _page.Render!.ObjectsContext.CurveEditor.CreateCurveFromData(data);
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

        Vector2 ICurveEditorHtml.AxisAspects {
            get => AxisAspects.Value;
            set {
                AxisAspects.Value = value;
                _page.Refresh();
            }
        }

        event Action ICurveEditorHtml.onDrawScaledCurveChanged {
            add => DrawScaledCurve.onChanged += value;
            remove => DrawScaledCurve.onChanged -= value;
        }

        bool ICurveEditorHtml.DrawScaledCurve => DrawScaledCurve.Value;

        event Action ICurveEditorHtml.onSelectedVertexValuesChanged {
            add => SelectedVertexValues.onChanged += value;
            remove => SelectedVertexValues.onChanged -= value;
        }

        event Action ICurveEditorHtml.onLockXAxisChanged {
            add => LockXAxis.onChanged += value;
            remove => LockXAxis.onChanged -= value;
        }

        Vector2 ICurveEditorHtml.SelectedVertexPosition {
            get => SelectedVertexValues.PositionExternal; 
            set {
                SelectedVertexValues.PositionExternal = value;
                _page.Refresh();
            }
        }
        float ICurveEditorHtml.SelectedVertexAngle {
            get => SelectedVertexValues.Angle; 
            set {
                SelectedVertexValues.Angle = value;
                _page.Refresh();
            }
        }

        bool ICurveEditorHtml.LockXAxis { 
            get => LockXAxis.Value; 
            set {
                LockXAxis.Value = value;
                _page.Refresh();
            }
       }

#endregion
    }
}
