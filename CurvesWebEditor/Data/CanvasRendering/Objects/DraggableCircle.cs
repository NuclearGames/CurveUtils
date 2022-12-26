using CurvesWebEditor.Data.CanvasRendering.Colliders;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal sealed class DraggableCircle : CanvasObject, IDraggable {
        internal event Action<Vector2>? onDrag;
        internal event Action? onSelectedStateChanged;

        internal bool Enabled { get; set; } = true;
        internal Vector2 Position { get; set; }
        internal float Radius { get; set; }
        internal string Color => Pressed ? "#ff0000" : "#006b1b";

        public bool Pressed { get; set; }
        public bool Selected {
            get => _selected;
            set {
                _selected = value;
                onSelectedStateChanged?.Invoke();
            }
        }

        private readonly CircleCollider _collider;
        private readonly CircleRenderer _renderer;
        private bool _selected;

        public DraggableCircle() {
            _collider = new CircleCollider(() => Position, () => Radius);
            _renderer = new CircleRenderer(() => Position, () => Radius, () => Color);
        }

        public void DragTo(Vector2 positionWS) {
            Position = positionWS;
            onDrag?.Invoke(Position);
        }

        public override IEnumerable<IRenderer> GetRenderers() {
            if (Enabled) {
                yield return _renderer;
            }
        }

        public override bool CheckInbound(Vector2 pointWS) {
            return Enabled && _collider.CheckInbound(pointWS);
        }
    }
}
