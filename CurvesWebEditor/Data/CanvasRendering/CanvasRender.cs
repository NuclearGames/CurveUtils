using Blazor.Extensions.Canvas.Canvas2D;
using Curves;
using CurvesWebEditor.Data.CanvasRendering.Renderers;
using CurvesWebEditor.Data.CanvasRendering.Views;
using CurvesWebEditor.Data.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace CurvesWebEditor.Data.CanvasRendering {
    public sealed class CanvasRender {
        private readonly CanvasRenderContext _context;
        private GridView _grid = new GridView();
        private CurveRenderer _curve = new CurveRenderer();
        private BezierCurveView _bezier;
        private AxisView _axis;
        private bool _moveCamera;

        private List<PointView> _points = new List<PointView>();
        private TextRenderer _testText = new TextRenderer();

        public CanvasRender(Canvas2DContext canvas, int viewportWidth, int viewportHeight) {
            _context = new CanvasRenderContext(canvas);
            _context.Viewport.Set(viewportWidth, viewportHeight);

            _axis = new AxisView();
            _bezier = new BezierCurveView(new BezierCurve(new List<Vector2>() { 
                new Vector2(0, 0),
                new Vector2(200, 400),
                new Vector2(500, 500)
            }));

            _points.Add(new PointView());
            _points.Add(new PointView());

            _testText.Position = new Vector2(0.5f, -0.25f);
            _testText.Text = "0.5";
        }

        public void Resize(int viewportWidth, int viewportHeight) {
            _context.Viewport.Set(viewportWidth, viewportHeight);
        }

        public void Test() {
            _context.Camera.PositionWS = new Vector2(0.5f, 0f);
            _context.Camera.Scale = 2f;
            _context.Input.Setup();
            Console.WriteLine($"\n\nTest data");
            Console.WriteLine($"Camera: PosWS={_context.Camera.PositionWS}; Scale={_context.Camera.Scale}");
            Console.WriteLine($"Viewport: W={_context.Viewport.Width}; H={_context.Viewport.Height}; Aspect={_context.Viewport.Aspect}");
            Console.WriteLine($"Input:");
            Console.WriteLine($"    WorldToView={_context.Input.WorldToView}");
            Console.WriteLine($"    ViewToWorld={_context.Input.ViewToWorld}");
            Console.WriteLine($"    ViewToScreen={_context.Input.ViewToScreen}");
            Console.WriteLine($"    ScreenToView={_context.Input.ScreenToView}");
            Console.WriteLine($"        WorldToScreen={_context.Input.WorldToScreen}");
            Console.WriteLine($"        ScreenToWorld={_context.Input.ScreenToWorld}");
            Console.WriteLine($"    LeftTopWS={_context.Input.LeftBottomWS}");
            Console.WriteLine($"    RightBottomWS={_context.Input.RightTopWS}");
            Console.WriteLine();

            var v1 = new Vector2(0, 0);
            Console.WriteLine($"Vector: {v1}");
            Console.WriteLine($"    *WorldToScreen={_context.Input.WorldToScreen * v1.ToVector3(1)}");

            var v2 = new Vector2(-1, _context.Viewport.Aspect);
            Console.WriteLine($"Vector: {v2}");
            Console.WriteLine($"    *WorldToScreen={_context.Input.WorldToScreen * v2.ToVector3(1)}");

            var v3 = new Vector2(-210f, 0);
            Console.WriteLine($"Vector: {v3}");
            Console.WriteLine($"    *ScreenToWorld={_context.Input.ScreenToWorld * v3.ToVector3(1)}");
        }

        public async ValueTask Render(float deltaTime) {
            // return;
            _context.Input.Setup();

            await _context.Canvas.BeginBatchAsync();

            await _context.Canvas.ClearRectAsync(0, 0, _context.Viewport.Width, _context.Viewport.Height);
            await _context.Canvas.SetFillStyleAsync("#ffe6e6");
            await _context.Canvas.FillRectAsync(0, 0, _context.Viewport.Width, _context.Viewport.Height);

            await _grid.Render(_context);
            await _axis.Render(_context);
            await _curve.Render(_context);
            await _bezier.Render(_context);
            await _testText.Render(_context);

            for (int i = 0; i < _points.Count; i++) {
                foreach(var x in _points[i].GetRenderers()) {
                    await x.Render(_context);
                } 
            }

            await _context.Canvas.EndBatchAsync();
        }

        public void OnPointerMove(int viewportX, int viewportY) {
            _context.UserInput.SetPointerPositionSS(new Vector2(viewportX, viewportY));

            if (_moveCamera) {
                _context.Camera.PositionWS -= _context.UserInput.PointerDeltaWS;
                Console.WriteLine($"{_context.Camera.PositionWS}; {_context.UserInput.PointerPositionWS}");
            }

            if(_activePoint != null) {
                _activePoint.Position = _context.UserInput.PointerPositionWS;
            }
        }

        private PointView? _activePoint;

        public void OnPointerDown(int button, bool shift, bool alt) {
            switch (button) {
                case 0:
                    foreach (var point in _points) {
                        if (point.CheckInbound(_context.UserInput.PointerPositionWS)) {
                            _activePoint = point;
                            _activePoint.Selected = true;
                            break;
                        }
                    }
                    break;

                case 1:
                    _moveCamera = true;
                    break;

                case 2:
                    _context.Camera.PositionWS += new Vector2(0.25f, 0.25f);
                    break;
            }
        }

        public void OnPointerUp(int button, bool shift, bool alt) {
            switch (button) {
                case 0:
                    if (_activePoint != null) {
                        _activePoint.Selected = false;
                    }
                    _activePoint = null;
                    break;

                case 1:
                    _moveCamera = false;
                    break;
            }
        }

        public void OnWheel(float deltaY, bool shift, bool alt) {
            float delta = deltaY * 0.0005f;
            float scale = 1 + delta;

            _context.Camera.Scale *= scale;

            /*var trs = TransformUtils.ZoomTo(_context.Camera.PositionWS, scale);
            _context.Camera.Scale *= scale;
            _context.Camera.PositionWS = TransformUtils.Transform(_context.Camera.PositionWS, trs);


            _context.Camera.Scale = MathF.Max(0.05f, _context.Camera.Scale);
            _context.Camera.Scale = MathF.Min(10f, _context.Camera.Scale);

            Console.WriteLine($"Wheel: {deltaY}; {_context.Camera.Scale}");*/
        }
    }
}
