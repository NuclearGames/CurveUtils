using Curves;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TransformStructures;

namespace CurvesWebEditor.Data.CanvasRendering.Tools {
    internal sealed class CurveEditor : IInputHandler {
        private readonly IObjectsContext _context;
        private readonly HashSet<CurveVertex> _vertexes = new HashSet<CurveVertex>();
        private readonly CurveView _curveObject;
        private ICurve? _curve;

        internal CurveEditor(IObjectsContext context) {
            _context = context;
            _curveObject = _context.Create(() => new CurveView());
            CreateVertex(new Vector2(0f, 0f), 45f);
            CreateVertex(new Vector2(1f, 1f), 45f);
            UpdateCurve();
        }

        public void OnPointerDown(CanvasRenderContext context, int button, bool shift, bool alt) {
            if (button != 0 || !shift) {
                return;
            }

            CurveVertex? vertexToRemove = null;
            foreach (var x in _vertexes) {
                if (x.CheckInbound(context.UserInput.PointerPositionWS)) {
                    vertexToRemove = x;
                    break;
                }
            }

            if (vertexToRemove != null) {
                RemoveVertex(vertexToRemove);
            } else {
                CreateVertex(context.UserInput.PointerPositionWS, 45f);
            }
        }

        public void OnPointerMove(Vector2 positionWS) { }

        public void OnPointerUp(CanvasRenderContext context, int button, bool shift, bool alt) { }

        private void UpdateCurve() {
            var ordered = _vertexes.OrderBy(x => x.Position.X);
            var points = ordered.Select(x => x.Position).ToArray();
            var tangentAspects = ordered.Select(x => MathF.Tan(x.Angle * MathConstants.Deg2Rad)).ToArray();

            _curve = TangentBasedCurve.FromBasePoints(points, tangentAspects);
            _curveObject.SetCurve(_curve);
        }

        private void CreateVertex(Vector2 position, float angle) {
            var instance = _context.Create(() => new CurveVertex(position, angle));
            instance.onMove += OnMoveVertex;
            _vertexes.Add(instance);
            UpdateCurve();
        }

        private void RemoveVertex(CurveVertex vertex) {
            _context.Destroy(vertex);
            vertex.onMove -= OnMoveVertex;
            _vertexes.Remove(vertex);
            UpdateCurve();
        }

        private void OnMoveVertex(Vector2 newPosition) {
            UpdateCurve();
        }
    }
}
