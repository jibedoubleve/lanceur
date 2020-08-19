using Caliburn.Micro;

namespace Probel.Lanceur.Models.Settings
{
    public class WindowSettingsModel : PropertyChangedBase
    {
        #region Fields

        private string _colour;
        private int _expirationTimeMessage;
        private string _notificationType;
        private double _opacity;
        private PositionSettingsModel _position;

        private bool _showAtStartup;

        #endregion Fields

        #region Constructors

        public WindowSettingsModel()
        {
            Position = new PositionSettingsModel();
            Colour = "#FF1E1E1E";
            Opacity = 0.85;
        }

        #endregion Constructors

        #region Properties

        public string Colour
        {
            get => _colour;
            set => Set(ref _colour, value, nameof(Colour));
        }

        public int ExpirationTimeMessage
        {
            get => _expirationTimeMessage;
            set => Set(ref _expirationTimeMessage, value, nameof(ExpirationTimeMessage));
        }

        public string NotificationType
        {
            get => _notificationType;
            set => Set(ref _notificationType, value);
        }

        public double Opacity
        {
            get => _opacity;
            set => Set(ref _opacity, value, nameof(Opacity));
        }

        public PositionSettingsModel Position
        {
            get => _position;
            set => Set(ref _position, value, nameof(Position));
        }

        public bool ShowAtStartup
        {
            get => _showAtStartup;
            set => Set(ref _showAtStartup, value, nameof(ShowAtStartup));
        }

        #endregion Properties
    }
}