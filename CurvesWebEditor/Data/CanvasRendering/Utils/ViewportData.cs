namespace CurvesWebEditor.Data.CanvasRendering.Utils {
    public sealed class ViewportData {
        internal int Width { get; set; }
        internal int Height { get; set; }
        internal float Aspect => Width / (float)Height;
    }
}
