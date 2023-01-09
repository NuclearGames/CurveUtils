using Curves;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;
using CurvesWebEditor.Data.CanvasRendering.Managers;
using CurvesWebEditor.Data.CanvasRendering.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TransformStructures;

namespace CurvesWebEditor.Data.CanvasRendering.Tools {
    internal sealed class CurveEditor : IInputHandler {
        private readonly ObjectsContext _context;
        private readonly HashSet<CurveVertex> _vertexes = new HashSet<CurveVertex>();
        private readonly CurveView _curveObject;
        private readonly CurveView _scaledCurveObject;
        private ICurve? _curve;
        private CurveVertex? _selectedVertex;
        private float? _selectedXLock;

        internal CurveEditor(ObjectsContext context) {
            _context = context;
            _scaledCurveObject = _context.Create(() => new CurveView() { Width = 0.01f, Color = "#00eaff" });
            _curveObject = _context.Create(() => new CurveView() { Width = 0.005f, Color = "#0000FF" });
            CreateVertex(new Vector2(0f, 0f), 45f);
            CreateVertex(new Vector2(1f, 1f), 45f);

            _context.Html.onAxisAspectsChanged += UpdateCurve;
            _context.Html.onDrawScaledCurveChanged += UpdateCurve;
            _context.InteractableManager.onSelectedChanged += OnSelectedChanged;
            _context.Html.onSelectedVertexValuesChanged += OnSelectedVertexValuesChanged;
            _context.Html.onLockXAxisChanged += OnLockXAxisChanged; 

            UpdateCurve();
        }

        internal void CreateCurveFromData(TangentBasedCurveData data) {
            if(data.Vertexes!.Length < 2) {
                return;
            }

            while(_vertexes.Count > 0) {
                TryRemoveVertex(_vertexes.First());
            }

            var fixedVerts = _vertexes.ToArray();

            for(int i = 0; i < data.Vertexes!.Length; i++) {
                var position = new Vector2(data.Vertexes[i].PositionX, data.Vertexes[i].PositionY);
                float angle = MathF.Atan(data.Vertexes[i].Tangent) * MathConstants.Rad2Deg;

                if (i < fixedVerts.Length) {
                    fixedVerts[i].SetPositionAndAngle(position, angle);
                } else {
                    CreateVertex(position, angle);
                }
            }

            _context.Html.AxisAspects = new Vector2(data.XAspect, data.YAspect);
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
                XAspect = _context.Html.AxisAspects.X,
                YAspect = _context.Html.AxisAspects.Y
            };
        }

        internal void InverseVertices() {
            _context.Html.LockXAxis = false;
            var prevAspects = _context.Html.AxisAspects;
            _context.Html.AxisAspects = new Vector2(prevAspects.Y, prevAspects.X);

            foreach (var v in _vertexes) {
                //  float newAngle = 90f - MathF.Abs(v.Angle) * (Math.Sign(v.Angle) == 0 ? 1 : Math.Sign(v.Angle));
                float newAngle = v.Angle;
                v.SetPositionAndAngle(new Vector2(v.Position.Y, v.Position.X), newAngle);
            }

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
                TryRemoveVertex(vertexToRemove);
            } else {
                CreateVertexAndUpdate(context.UserInput.PointerPositionWS, 45f);
            }
        }

        public void OnPointerMove(Vector2 positionWS) { }

        public void OnPointerUp(CanvasRenderContext context, int button, bool shift, bool alt) { }

        private void UpdateCurve() {
            ClampLeftRightX();

            var ordered = _vertexes.OrderBy(x => x.Position.X);
            var points = ordered.Select(x => x.Position).ToArray();
            var tangentAspects = ordered.Select(x => GetTangent(x.Angle)).ToArray();

            float minX = _vertexes.Min(v => v.Position.X);
            float maxX = _vertexes.Max(v => v.Position.X);

            _curve = TangentBasedCurve.FromBasePoints(points, tangentAspects);
            _curveObject.SetCurve(_curve, minX, maxX);

            if (_context.Html.DrawScaledCurve) {
                var scaledCurve = TangentBasedCurve.FromBasePoints(points, tangentAspects, _context.Html.AxisAspects.X, _context.Html.AxisAspects.Y);
                _scaledCurveObject.SetCurve(scaledCurve, minX, maxX * _context.Html.AxisAspects.X);
            } else {
                _scaledCurveObject.SetCurve(null, 0f, 0f);
            }
        }

        private void ClampLeftRightX() {
            if (_context.Html.LockXAxis) {
                if (_selectedVertex != null && _selectedXLock != null) {
                    _selectedVertex.SetPosition(new Vector2(_selectedXLock.Value, _selectedVertex.Position.Y));
                }
            }
        }

        private void CreateVertexAndUpdate(Vector2 position, float angle) {
            CreateVertex(position, angle);
            UpdateCurve();
        }

        private CurveVertex CreateVertex(Vector2 position, float angle) {
            var instance = _context.Create(() => new CurveVertex(position, angle));
            instance.onMove += OnVertexUpdated;
            instance.onRotate += OnVertexUpdated;
            _vertexes.Add(instance);
            return instance;
        }

        private void TryRemoveVertex(CurveVertex instance) {
            if(_vertexes.Count <= 2) {
                return;
            }

            _context.Destroy(instance);
            instance.onMove -= OnVertexUpdated;
            instance.onRotate -= OnVertexUpdated;
            _vertexes.Remove(instance);
            UpdateCurve();
        }

        private float GetTangent(float angle) {
            return MathF.Tan(angle * MathConstants.Deg2Rad);
        }

        private void TryUpdateVetrexHtmlView() {
            if (_context.InteractableManager.Selected != null && _context.InteractableManager.Selected is CurveVertexNode node) {
                UpdateVetrexHtmlView(node.Parent);
            }
        }

        private void UpdateVetrexHtmlView(CurveVertex vertex) {
            _context.Html.SelectedVertexPosition = vertex.Position;
            _context.Html.SelectedVertexAngle = vertex.Angle;
        }

        private void OnVertexUpdated() {
            TryUpdateVetrexHtmlView();
            UpdateCurve();
        }

        private void OnSelectedVertexValuesChanged() {
            if (_context.InteractableManager.Selected != null && _context.InteractableManager.Selected is CurveVertexNode node) {
                node.Parent.SetPositionAndAngle(_context.Html.SelectedVertexPosition, _context.Html.SelectedVertexAngle);
                UpdateCurve();
            }
        }

        private void OnSelectedChanged(IDraggable? obj) {
            if(obj != null && obj is CurveVertexNode node) {
                _selectedVertex = node.Parent;
                _selectedXLock = _selectedVertex == _vertexes.MinBy(v => v.Position.X)
                    ? 0f 
                    : (_selectedVertex == _vertexes.MaxBy(v => v.Position.X) ? 1f : null);

                UpdateVetrexHtmlView(node.Parent);
            }
        }

        private void OnLockXAxisChanged() {
            UpdateCurve();
        }
    }
}
