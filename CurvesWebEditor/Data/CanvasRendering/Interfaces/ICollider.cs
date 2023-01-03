using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Interfaces {
    internal interface ICollider {
        bool CheckInbound(Vector2 pointWS);
    }
}
