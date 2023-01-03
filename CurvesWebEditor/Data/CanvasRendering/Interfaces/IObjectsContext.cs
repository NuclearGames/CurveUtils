using CurvesWebEditor.Data.CanvasRendering.Objects;
using System;

namespace CurvesWebEditor.Data.CanvasRendering.Interfaces {
    internal interface IObjectsContext {
        T Create<T>(Func<T> create) where T : CanvasObject;
        void Destroy(CanvasObject obj);
    }
}
