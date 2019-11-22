namespace Probel.Lanceur.Core.Services
{
    public interface ISlickRunImporterService
    {
        #region Methods

        void Import(string sessionName = null, string fileName = null);

        #endregion Methods
    }
}