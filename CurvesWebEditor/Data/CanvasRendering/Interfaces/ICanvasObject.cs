using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Interfaces {
    internal interface ICanvasObject {
        bool CheckInbound(Vector2 positionWS);
        IEnumerable<IRenderer> GetRenderers();
    }
}
