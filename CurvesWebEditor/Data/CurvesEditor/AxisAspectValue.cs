namespace CurvesWebEditor.Data.CurvesEditor {
    public class AxisAspectValues {
        public string XString {
            get => X.ToString();
            set {
                if(float.TryParse(value, out float f)) {
                    X = f;
                }
            }
        }

        public string YString {
            get => Y.ToString();
            set {
                if (float.TryParse(value, out float f)) {
                    Y = f;
                }
            }
        }

        public float X { get; set; }
        public float Y { get; set; }

        public AxisAspectValues(float x, float y) {
            X = x;
            Y = y;
        }
    }
}
