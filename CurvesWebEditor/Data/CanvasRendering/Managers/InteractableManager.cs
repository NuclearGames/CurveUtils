using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Managers
{
    internal sealed class InteractableManager : IInputHandler {
        internal event Action<IDraggable?>? onSelectedChanged;

        internal IDraggable? Pressed {
            get => _pressed;
            private set {
                if (_pressed != null) {
                    _pressed.Pressed = false;
                }
                _pressed = value;
                if (_pressed != null) {
                    _pressed.Pressed = true;
                }
            }
        }

        internal IDraggable? Selected {
            get => _selected;
            private set {
                if (_selected != null) {
                    _selected.Selected = false;
                }
                _selected = value;
                if (_selected != null) {
                    _selected.Selected = true;
                }
                onSelectedChanged?.Invoke(_selected);
            }
        }

        private readonly HashSet<IDraggable> _interactables = new HashSet<IDraggable>();
        private IDraggable? _pressed;
        private IDraggable? _selected;

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
