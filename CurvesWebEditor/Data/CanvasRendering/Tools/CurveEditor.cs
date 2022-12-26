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
        private readonly CurveVertex _leftVertex;
        private readonly CurveVertex _rightVertex;
        private float _xAspect, _yAspect;

        private readonly CurveView _curveObject;
        private ICurve? _curve;

        internal CurveEditor(IObjectsContext context) {
            _context = context;
            _curveObject = _context.Create(() => new CurveView());
            _leftVertex = CreateVertex(new Vector2(0f, 0f), 45f);
            _rightVertex = CreateVertex(new Vector2(1f, 1f), 45f);
            UpdateCurve();
        }

        internal TangentBasedCurveData CreateData() {
            var vertexes = _vertexes
                .Where(v => v.Position.X >= 0f && v.Position.X <= 1f)
                .OrderBy(v => v.Position.X)
                .Select(v => new TangentBasedCurveData.CurveVertex() { 
                    PositionX = v.Position.X,
                    PositionY = v.Position.Y,
                    Tangent = GetTangent(v.Angle)
                })
                .ToArray();

            return new TangentBasedCurveData() { 
                Vertexes = vertexes,
                XAspect = _xAspect,
                YAspect = _yAspect
            };
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
                TryRemoveVertex(vertexToRemove);
            } else {
                CreateVertexAndUpdate(context.UserInput.PointerPositionWS, 45f);
            }
        }

        public void OnPointerMove(Vector2 positionWS) { }

        public void OnPointerUp(CanvasRenderContext context, int button, bool shift, bool alt) { }

        private void UpdateCurve() {
            // Первый и последний закрепляются на X = 0 и 1.
            _leftVertex.SetPosition(new Vector2(0f, _leftVertex.Position.Y));
            _rightVertex.SetPosition(new Vector2(1f, _rightVertex.Position.Y));

            var ordered = _vertexes.OrderBy(x => x.Position.X);
            var points = ordered.Select(x => x.Position).ToArray();
            var tangentAspects = ordered.Select(x => GetTangent(x.Angle)).ToArray();

            _curve = TangentBasedCurve.FromBasePoints(points, tangentAspects);
            _curveObject.SetCurve(_curve, _leftVertex.Position.X, _rightVertex.Position.X);
        }

        private void CreateVertexAndUpdate(Vector2 position, float angle) {
            CreateVertex(position, angle);
            UpdateCurve();
        }

        private CurveVertex CreateVertex(Vector2 position, float angle) {
            var instance = _context.Create(() => new CurveVertex(position, angle));
            instance.onMove += UpdateCurve;
            instance.onRotate += UpdateCurve;
            _vertexes.Add(instance);
            return instance;
        }

        private void TryRemoveVertex(CurveVertex instance) {
            if(_vertexes.Count <= 2) {
                return;
            }

            _context.Destroy(instance);
            instance.onMove -= UpdateCurve;
            instance.onRotate -= UpdateCurve;
            _vertexes.Remove(instance);
            UpdateCurve();
        }

        private float GetTangent(float angle) {
            return MathF.Tan(angle * MathConstants.Deg2Rad);
        }
    }
}
