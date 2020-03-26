namespace Probel.Lanceur.Core.Services
{
    public interface IUpdateService
    {
        #region Methods

        bool DoNeedUpdate();

        void Update();

        #endregion Methods
    }
}