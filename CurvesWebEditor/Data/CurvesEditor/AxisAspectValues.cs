using CurvesWebEditor.Data.Utils;
using System;
using System.Globalization;
using System.Numerics;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal class AxisAspectValues {
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

        internal Vector2 Value {
            get => new Vector2(X, Y);
            set {
                _x = value.X;
                _y = value.Y;
            }
        }

        private float _x;
        private float _y;

        internal AxisAspectValues(float x, float y) {
            X = x;
            Y = y;
        }
    }
}
