namespace Probel.Lanceur.Core.Helpers
{
    public interface IScreenRuler
    {
        #region Properties

        double MaxLeft { get; }

        double MaxTop { get; }

        #endregion Properties

        #region Methods

        Coordinate Center(double distanceFromTop);

        double GetWindowHeight();

        double GetWindowWidth();

        Coordinate StickTo(double left, double top);

        #endregion Methods
    }
}