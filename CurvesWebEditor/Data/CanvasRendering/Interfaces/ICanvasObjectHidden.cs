namespace CurvesWebEditor.Data.CanvasRendering.Interfaces {
    internal interface ICanvasObjectHidden {
        void DestroyEvent();
        void Initialize(IObjectsContext context);
    }
}
