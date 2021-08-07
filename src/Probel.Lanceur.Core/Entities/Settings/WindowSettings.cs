namespace Probel.Lanceur.Core.Entities.Settings
{
    public class WindowSettings
    {
        #region Fields

        private string _notificationType = "classic";

        #endregion Fields

        #region Properties

        public string Colour { get; set; } = "#424242";
        public int ExpirationTimeMessage { get; set; } = 8;
        public string NotificationType { get => _notificationType; set => _notificationType = value.ToLower(); }
        public double Opacity { get; set; } = 1;
        public PositionSettings Position { get; set; } = new PositionSettings();
        public bool ShowAtStartup { get; set; } = true;

        #endregion Properties
    }
}