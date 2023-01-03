namespace CurvesWebEditor.Data.CanvasRendering.Utils {
    public sealed class ViewportData {
        internal int Width { get; private set; }
        internal int Height { get; private set; }
        internal float Aspect { get; private set; }

        internal void Set(int width, int height) {
            Width = width;
            Height = height;
            Aspect = height / (float)width;
        }
    }
}
