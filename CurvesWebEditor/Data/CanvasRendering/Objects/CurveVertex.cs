using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using CurvesWebEditor.Data.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using TransformStructures;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal class CurveVertex : CanvasObject {
        private const float MAX_ANGLE = 89f;

        internal event Action<Vector2>? onMove;

        internal Vector2 Position => _center.Position;
        internal float RotateRadius { get; private set; } = 0.1f;
        internal float Angle { get; private set; }

        private readonly DraggableCircle _center;
        private readonly DraggableCircle _left;
        private readonly DraggableCircle _right;
        private readonly LineRenderer _lineRenderer;

        public CurveVertex(Vector2 position, float angle) {
            Angle = angle;
            _center = new DraggableCircle() { Radius = 0.05f };
            _left = new DraggableCircle() { Radius = 0.025f };
            _right = new DraggableCircle() { Radius = 0.025f };
            _lineRenderer = new LineRenderer(() => _left.Position, () => _right.Position, () => 0.001f, () => "#0000ff");

            _center.onDrag += OnDragCenter;
            _left.onDrag += OnDrag;
            _right.onDrag += OnDrag;

            _center.Position = position;
            SetLeftRightPositions();
        }

        private void SetLeftRightPositions() {
            var pointOS = new Vector2(RotateRadius, 0f);
            var rightPointOS = (Matrix3x3.Rotation(Angle) * pointOS.ToVector3(1f)).ToVector2();
            var leftPointOS = (Matrix3x3.Rotation(Angle + 180f) * pointOS.ToVector3(1f)).ToVector2();

            _right.Position = Position + rightPointOS;
            _left.Position = Position + leftPointOS;
        }

        private void OnDragCenter(Vector2 newPosition) {
            SetLeftRightPositions();
            onMove?.Invoke(newPosition);
        }

        private void OnDrag(Vector2 newPosition) {
            // Вычисление позиции дочерних кружков на радиусе.
            var localPos = newPosition - Position;
            float localYdivX = localPos.Y / localPos.X;

            float x = MathF.Sqrt(RotateRadius * RotateRadius / (1 + localYdivX * localYdivX));
            float y = x * localYdivX;

            var intersection0 = new Vector2(x, y);
            var intersection1 = new Vector2(-x, -y);

            var rightPoint = intersection1.X >= 0 ? intersection1 : intersection0;

            Angle = MathF.Atan2(rightPoint.Y, rightPoint.X) * MathConstants.Rad2Deg;
            Angle = MathF.Max(-MAX_ANGLE, Angle);
            Angle = MathF.Min(MAX_ANGLE, Angle);

            SetLeftRightPositions();
        }

        public override bool CheckInbound(Vector2 positionWS) {
            return _center.CheckInbound(positionWS);
        }

        public override IEnumerable<IRenderer> GetRenderers() {
            yield return _lineRenderer;

            foreach (var x in _left.GetRenderers()) {
                yield return x;
            }

            foreach (var x in _right.GetRenderers()) {
                yield return x;
            }

            foreach (var x in _center.GetRenderers()) {
                yield return x;
            }
        }
    }
}
