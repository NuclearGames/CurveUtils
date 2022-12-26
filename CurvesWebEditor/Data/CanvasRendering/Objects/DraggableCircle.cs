using CurvesWebEditor.Data.CanvasRendering.Colliders;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Objects
{
    internal sealed class DraggableCircle : CanvasObject, IDraggable {
        internal event Action<Vector2>? onDrag;

        internal Vector2 Position { get; set; }
        internal float Radius { get; set; }
        internal string Color { get; set; } = "#000000";

        public bool Pressed { get; set; }
        public bool Selected { get; set; }

        private readonly CircleCollider _collider;
        private readonly CircleRenderer _renderer;

        public DraggableCircle() {
            _collider = new CircleCollider(() => Position, () => Radius);
            _renderer = new CircleRenderer(() => Position, () => Radius, () => Color);
        }

        public void DragTo(Vector2 positionWS) {
            Position = positionWS;
            onDrag?.Invoke(Position);
        }

        public override IEnumerable<IRenderer> GetRenderers() {
            yield return _renderer;
        }

        public override bool CheckInbound(Vector2 pointWS) {
            return _collider.CheckInbound(pointWS);
        }
    }
}
