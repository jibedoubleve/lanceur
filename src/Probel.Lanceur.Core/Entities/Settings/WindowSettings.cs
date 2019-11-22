namespace Probel.Lanceur.Core.Entities.Settings
{
    public class WindowSettings
    {
        #region Properties

        public double Opacity { get; set; } = 0.80;

        public PositionSettings Position { get; set; } = new PositionSettings();

        public string Colour { get; set; } = "#FF1E1E1E";
        #endregion Properties
    }
}