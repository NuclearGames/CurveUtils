namespace Curves.Interfaces {
    /// <summary>
    /// Кривая, представляющая функцию y(x).
    /// </summary>
    public interface ICurve {
        /// <summary>
        /// Возвращает значение y по значению x.
        /// </summary>
        float Evaluate(float x);
    }
}
