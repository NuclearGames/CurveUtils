using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Objects;
using CurvesWebEditor.Data.CanvasRendering.Tools;
using CurvesWebEditor.Data.CurvesEditor;
using System;
using System.Collections.Generic;

namespace CurvesWebEditor.Data.CanvasRendering.Managers {
    internal sealed class ObjectsContext : IObjectsContext {
        internal ICurveEditorHtml Html { get; }
        internal InteractableManager InteractableManager { get; } = new InteractableManager();
        internal RenderersManager RenderersManager { get; }
        internal IReadOnlySet<ICanvasObject> Objects => _objects;
        internal CurveEditor CurveEditor { get; }

        private readonly HashSet<ICanvasObject> _objects = new HashSet<ICanvasObject>();

        public ObjectsContext(ICurveEditorHtml html) {
            Html = html;
            Create(() => new BackgroundView());
            RenderersManager = new RenderersManager(this);
            CurveEditor = new CurveEditor(this);
        }

        public T Create<T>(Func<T> create) where T : CanvasObject {
            var instance = create();
            ((ICanvasObjectHidden)instance).Initialize(this);
            Add(instance);
            return instance;
        }

        public void Destroy(CanvasObject obj) {
            ((ICanvasObjectHidden)obj).DestroyEvent();
            Remove(obj);
        }

        private void Add(ICanvasObject obj) {
            if (obj is IDraggable draggable) {
                InteractableManager.Add(draggable);
            }

            if (!_objects.Add(obj)) {
                throw new InvalidOperationException($"Object already added.");
            }
        }

        private void Remove(ICanvasObject obj) {
            if (obj is IDraggable draggable) {
                InteractableManager.Remove(draggable);
            }

            if (!_objects.Remove(obj)) {
                throw new InvalidOperationException($"Object not exists.");
            }
        }
    }
}
