﻿using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using CurvesWebEditor.Data.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using TransformStructures;

namespace CurvesWebEditor.Data.CanvasRendering.Objects {
    internal class CurveVertex : CanvasObject {
        private const float MAX_ANGLE = 89f;

        internal event Action? onMove;
        internal event Action? onRotate;

        internal Vector2 Position {get; private set;}
        internal float RotateRadius { get; private set; } = 0.08f;
        internal float Angle { get; private set; }

        private DraggableCircle? _center;
        private DraggableCircle? _left;
        private DraggableCircle? _right;
        private LineRenderer? _lineRenderer;

        public CurveVertex(Vector2 position, float angle) {
            Position = position;
            Angle = angle;
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            _center = Create(() => new DraggableCircle() { Radius = 0.025f });
            _left = Create(() => new DraggableCircle() { Radius = 0.01f });
            _right = Create(() => new DraggableCircle() { Radius = 0.01f });
            _lineRenderer = new LineRenderer(() => _left.Position, () => _right.Position, () => 0.005f, () => "#0000ff");

            _center.onDrag += OnDragCenter;
            _left.onDrag += OnDrag;
            _right.onDrag += OnDrag;

            _center.Position = Position;
            SetLeftRightPositions();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            Destroy(_center!);
            Destroy(_left!);
            Destroy(_right!);
        }


        internal void SetPosition(Vector2 newPosition) {
            Position = newPosition;
            _center!.Position = Position;
            SetLeftRightPositions();
        }

        private void SetLeftRightPositions() {
            var pointOS = new Vector2(RotateRadius, 0f);
            var rightPointOS = (Matrix3x3.Rotation(Angle) * pointOS.ToVector3(1f)).ToVector2();
            var leftPointOS = (Matrix3x3.Rotation(Angle + 180f) * pointOS.ToVector3(1f)).ToVector2();

            _right!.Position = Position + rightPointOS;
            _left!.Position = Position + leftPointOS;
        }

        private void OnDragCenter(Vector2 newPosition) {
            SetPosition(newPosition);
            onMove?.Invoke();
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
            onRotate?.Invoke();
        }

        public override bool CheckInbound(Vector2 positionWS) {
            return _center!.CheckInbound(positionWS);
        }

        public override IEnumerable<IRenderer> GetRenderers() {
            yield return _lineRenderer!;

            foreach (var x in _left!.GetRenderers()) {
                yield return x;
            }

            foreach (var x in _right!.GetRenderers()) {
                yield return x;
            }

            foreach (var x in _center!.GetRenderers()) {
                yield return x;
            }
        }
    }
}
