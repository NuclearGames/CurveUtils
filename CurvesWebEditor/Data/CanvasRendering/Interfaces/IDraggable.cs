using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Interfaces {
    internal interface IDraggable : ICanvasObject {
        bool Pressed { get; set; }
        bool Selected { get; set; }

        void DragTo(Vector2 positionWS);
    }
}
