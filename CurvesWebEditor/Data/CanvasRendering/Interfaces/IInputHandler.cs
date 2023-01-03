using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Interfaces {
    internal interface IInputHandler {
        void OnPointerDown(CanvasRenderContext context, int button, bool shift, bool alt);
        void OnPointerMove(Vector2 positionWS);
        void OnPointerUp(CanvasRenderContext context, int button, bool shift, bool alt);
    }
}
