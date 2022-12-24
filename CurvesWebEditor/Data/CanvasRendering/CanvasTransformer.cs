using CurvesWebEditor.Data.Utils.Extensions;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering {
    internal sealed class CanvasTransformer {
        private readonly CanvasRenderContext _context;

        internal CanvasTransformer(CanvasRenderContext context) {
            _context = context;
        }

        internal Vector2 Point(Vector2 pointWS) {
            return (_context.Input.WorldToScreen * new Vector3(pointWS.X, pointWS.Y, 1)).ToVector2();
        }

        internal Vector2 Size(Vector2 pointWS) {
            return (_context.Input.WorldToScreen * new Vector3(pointWS.X, pointWS.Y, 0)).ToVector2();
        }

        internal float Size(float pointWS) {
            return Size(new Vector2(pointWS, 0f)).X;
        }

        /// <summary>
        /// Возвращает точку в экранных координатах.
        /// </summary>
        /// <param name="pointWS">Точка в мировых координатах.</param>
        /// <returns></returns>
        internal Vector2 PointWSToSS(Vector2 pointWS) {
            var pointVS3 = _context.Input.WorldToView * new Vector3(pointWS.X, pointWS.Y, 1f);
            var pointVS = new Vector2(pointVS3.X, pointVS3.Y);
            var pointCS = (pointVS - new Vector2(1, _context.Viewport.Aspect)) * 0.5f;
            var pointSS = new Vector2(
                pointCS.X * _context.Viewport.Width,
                (1 - pointCS.Y) * _context.Viewport.Height);
            return pointSS;
        }

        /// <summary>
        /// Возвращает размер в экранных координатах.
        /// </summary>
        /// <param name="sizeWS">Размер в мировых координатах.</param>
        /// <returns></returns>
        internal Vector2 SizeWSToSS(Vector2 sizeWS) {
            var pointVS3 = _context.Input.WorldToView * new Vector3(sizeWS.X, sizeWS.Y, 0f);
            var pointVS = new Vector2(pointVS3.X, pointVS3.Y);
            var pointCS = pointVS * 0.5f;
            var pointSS = new Vector2(
                pointCS.X * _context.Viewport.Width,
                pointCS.Y * _context.Viewport.Height);
            return pointSS;
        }

        /// <summary>
        /// Возвращает размер в экранных координатах.
        /// </summary>
        /// <param name="sizeWS">Размер в мировых координатах.</param>
        /// <returns></returns>
        internal float SizeWSToSS(float sizeWS) {
            return SizeWSToSS(new Vector2(sizeWS, 0)).X;
        }
    }
}
