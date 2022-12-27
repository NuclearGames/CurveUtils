using System.Globalization;

namespace CurvesWebEditor.Data.Utils {
    internal static class FloatCultureInfo {
        internal static CultureInfo Value { get; }

        static FloatCultureInfo() {
            Value = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            Value.NumberFormat.CurrencyDecimalSeparator = ".";
        }
    }
}
