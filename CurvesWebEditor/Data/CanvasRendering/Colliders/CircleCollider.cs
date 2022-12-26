using System;
using System.Numerics;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;

namespace CurvesWebEditor.Data.CanvasRendering.Colliders
{
    internal class CircleCollider : ICollider {
        internal Vector2 Position => _getPosition();
        internal float Radius => _getRadius();

        private readonly Func<Vector2> _getPosition;
        private readonly Func<float> _getRadius;

        public CircleCollider(Func<Vector2> getPosition, Func<float> getRadius) {
            _getPosition = getPosition;
            _getRadius = getRadius;
        }

        public bool CheckInbound(Vector2 pointWS) {
            var localPos = pointWS - Position;
            float sqrDist = localPos.LengthSquared();
            return sqrDist <= Radius * Radius;
        }
    }
}
