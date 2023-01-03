using CurvesWebEditor.Data.Utils.Extensions;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Utils {
    internal sealed class UserInputData {
        internal Vector2 PointerPositionSS => _pointerPositionSS ?? Vector2.Zero;
        internal Vector2 PointerPositionWS => _pointerPositionWS ?? Vector2.Zero;
        internal Vector2 PointerDeltaSS { get; private set; }
        internal Vector2 PointerDeltaWS { get; private set; }

        private Vector2? _pointerPositionSS;
        private Vector2? _pointerPositionWS;

        private readonly CanvasRenderContext _context;

        internal UserInputData(CanvasRenderContext context) {
            _context = context;
        }

        internal void SetPointerPositionSS(Vector2 positionSS) {
            var newSS = positionSS;
            var newWS = (_context.Input.ScreenToWorld * positionSS.ToVector3(1f)).ToVector2();
            if (_pointerPositionSS != null && _pointerPositionWS != null) {
                PointerDeltaSS = newSS - _pointerPositionSS.Value;
                PointerDeltaWS = (_context.Input.ScreenToWorld * PointerDeltaSS.ToVector3(0f)).ToVector2();
            }
            _pointerPositionSS = newSS;
            _pointerPositionWS = newWS;
        }
    }
}
