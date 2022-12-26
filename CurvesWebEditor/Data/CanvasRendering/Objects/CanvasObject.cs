using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal abstract class CanvasObject : ICanvasObject {
        private IObjectsContext? _context;

        internal void Initialize(IObjectsContext context) {
            _context = context;
        }

        internal void Destroy(CanvasObject obj) {
            _context!.Destroy(obj);
        }

        public abstract bool CheckInbound(Vector2 positionWS);

        public abstract IEnumerable<IRenderer> GetRenderers();
    }
}
