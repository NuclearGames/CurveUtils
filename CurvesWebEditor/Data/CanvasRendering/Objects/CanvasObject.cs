using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal abstract class CanvasObject : ICanvasObject, ICanvasObjectHidden {
        private IObjectsContext? _context;

        void ICanvasObjectHidden.Initialize(IObjectsContext context) {
            _context = context;
            OnInitialize();
        }

        void ICanvasObjectHidden.DestroyEvent() {
            OnDestroy();
        }

        internal T Create<T>(Func<T> create) where T : CanvasObject {
            return _context!.Create(create);
        }

        internal void Destroy(CanvasObject obj) {
            _context!.Destroy(obj);
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnDestroy() { }

        public abstract bool CheckInbound(Vector2 positionWS);

        public abstract IEnumerable<IRenderer> GetRenderers();
    }
}
