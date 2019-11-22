using Caliburn.Micro;
using System.Diagnostics;

namespace Probel.Lanceur.Models.Settings
{
    [DebuggerDisplay("Left:{Left} - Top:{Top}")]
    public class PositionSettingsModel : PropertyChangedBase
    {
        #region Fields

        private double _left;
        private double _top;

        #endregion Fields

        #region Properties

        public double Left
        {
            get => _left;
            set => Set(ref _left, value, nameof(Left));
        }

        public double Top
        {
            get => _top;
            set => Set(ref _top, value, nameof(Top));
        }

        #endregion Properties
    }
}