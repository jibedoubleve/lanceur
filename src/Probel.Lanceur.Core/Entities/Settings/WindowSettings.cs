namespace Probel.Lanceur.Core.Entities.Settings
{
    public class WindowSettings
    {
        #region Properties

        public string Colour { get; set; } = "#424242";

        public int ExpirationTimeMessage { get; set; } = 8;

        public double Opacity { get; set; } = 1;

        public PositionSettings Position { get; set; } = new PositionSettings();

        #endregion Properties
    }
}