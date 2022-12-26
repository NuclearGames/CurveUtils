using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Managers
{
    internal sealed class InteractableManager : IInputHandler {
        internal IDraggable? Pressed {
            get => pressed;
            private set {
                if (pressed != null) {
                    pressed.Pressed = false;
                }
                pressed = value;
                if (pressed != null) {
                    pressed.Pressed = true;
                }
            }
        }

        internal IDraggable? Selected {
            get => selected;
            private set {
                if (selected != null) {
                    selected.Selected = false;
                }
                selected = value;
                if (selected != null) {
                    selected.Selected = true;
                }
            }
        }

        private readonly HashSet<IDraggable> _interactables = new HashSet<IDraggable>();
        private IDraggable? pressed;
        private IDraggable? selected;

        internal void Add(IDraggable draggable) {
            _interactables.Add(draggable);
        }

        internal void Remove(IDraggable draggable) {
            _interactables.Remove(draggable);
        }

        public void OnPointerDown(CanvasRenderContext context, int button, bool shift, bool alt) {
            if(button != 0) {
                return;
            }

            foreach (var x in _interactables) {
                if (x.CheckInbound(context.UserInput.PointerPositionWS)) {
                    Pressed = Selected = x;
                    return;
                }
            }
        }

        public void OnPointerUp(CanvasRenderContext context, int button, bool shift, bool alt) {
            if(button != 0) {
                return;
            }

            Pressed = null;
        }

        public void OnPointerMove(Vector2 positionWS) {
            if (Pressed != null) {
                Pressed.DragTo(positionWS);
            }
        }
    }
}
