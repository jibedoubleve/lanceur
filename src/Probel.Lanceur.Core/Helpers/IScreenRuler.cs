namespace Probel.Lanceur.Core.Helpers
{
    public interface IScreenRuler
    {
        #region Properties

        double MaxLeft { get; }

        double MaxTop { get; }

        #endregion Properties

        #region Methods

        double GetWindowHeight();

        double GetWindowWidth();

        Coordinate StickTo(double left, double top);

        #endregion Methods
    }
}