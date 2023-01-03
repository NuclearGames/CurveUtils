using System;

namespace CurvesWebEditor.Data.CurvesEditor {
    internal class FlagValue {
        internal event Action? onChanged;

        internal bool Value {
            get => _value;
            set {
                _value = value;
                onChanged?.Invoke();
            }
        }

        private bool _value;

        public FlagValue(bool value) {
            _value = value;
        }
    }
}
