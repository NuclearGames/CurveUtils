using CurvesWebEditor.Data.Utils;
using System.Globalization;
using System.Numerics;
using System;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal class SelectedVertexValues {
        internal event Action? onChanged;

        internal string XString {
            get => X.ToString();
            set {
                if (float.TryParse(value, NumberStyles.Any, FloatCultureInfo.Value, out float f)) {
                    X = f;
                }
            }
        }

        internal string YString {
            get => Y.ToString();
            set {
                if (float.TryParse(value, NumberStyles.Any, FloatCultureInfo.Value, out float f)) {
                    Y = f;
                }
            }
        }

        internal string AngleString {
            get => Angle.ToString();
            set {
                if (float.TryParse(value, NumberStyles.Any, FloatCultureInfo.Value, out float f)) {
                    Angle = f;
                }
            }
        }

        internal float X {
            get => _x;
            set {
                _x = value;
                onChanged?.Invoke();
            }
        }

        internal float Y {
            get => _y;
            set {
                _y = value;
                onChanged?.Invoke();
            }
        }

        internal float Angle {
            get => _angle;
            set {
                _angle = value;
                onChanged?.Invoke();
            }
        }

        internal Vector2 PositionExternal {
            get => new Vector2(X, Y);
            set {
                _x = value.X;
                _y = value.Y;
            }
        }

        internal float AngleExternal {
            get => Angle;
            set => _angle = value;
        }

        private float _x;
        private float _y;
        private float _angle;

        internal SelectedVertexValues(float x, float y, float angle) {
            _x = x;
            _y = y;
            _angle = angle;
        }
    }
}
