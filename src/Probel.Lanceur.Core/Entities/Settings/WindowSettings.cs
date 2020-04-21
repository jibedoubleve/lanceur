namespace Probel.Lanceur.Core.Entities.Settings
{
    public class WindowSettings
    {
        #region Properties

        public string Colour { get; set; } = "#FF1E1E1E";

        public int ExpirationTimeMessage { get; set; } = 8;

        public double Opacity { get; set; } = 0.80;

        public PositionSettings Position { get; set; } = new PositionSettings();

        #endregion Properties
    }
}