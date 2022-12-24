using CurvesWebEditor.Data.CanvasRendering.Renderers;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CurvesWebEditor.Data.CanvasRendering.Views {
    internal sealed class PointView : IView {
        private const float RADIUS = 0.25f;
        private const float RADIUS_SQR = RADIUS * RADIUS;

        internal Vector2 Position {
            get => position;
            set {
                position = value;
                _renderer.Position = value;
            }
        }

        internal bool Selected {
            get => selected;
            set {
                selected = value;
                _renderer.Color = selected ? "#ff0000" : "#00ff00";
            }
        }

        private readonly CircleRenderer _renderer;
        private Vector2 position;
        private bool selected;

        public PointView() {
            _renderer = new CircleRenderer();
            Position = Vector2.Zero;
            Selected = false;
            _renderer.Radius = RADIUS;
        }

        internal bool CheckInbound(Vector2 point) {
            var localPos = point - Position;
            float sqrDist = localPos.LengthSquared();
            Console.WriteLine($"CPI {Position} ::: {point} [{localPos}] ({sqrDist}/{RADIUS_SQR})");
            return sqrDist <= RADIUS_SQR;
        }

        public IEnumerable<IRenderer> GetRenderers() {
            yield return _renderer;
        }
    }
}
