using Caliburn.Micro;

namespace Probel.Lanceur.Plugin.Calculator.Models
{
    public class ValueItem : PropertyChangedBase
    {
        #region Fields

        private string _expression;

        private bool _isReadOnly;
        private bool _isResult;

        #endregion Fields

        #region Properties

        public string Expression
        {
            get => _expression;
            set => Set(ref _expression, value, nameof(Expression));
        }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => Set(ref _isReadOnly, value, nameof(IsReadOnly));
        }

        public bool IsResult
        {
            get => _isResult;
            set => Set(ref _isResult, value, nameof(IsResult));
        }

        #endregion Properties

        #region Methods

        internal static ValueItem Calculation() => new ValueItem() { IsResult = false };

        internal static ValueItem Result(string value) => new ValueItem() { Expression = value, IsResult = true };

        #endregion Methods
    }
}