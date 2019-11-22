using Probel.Lanceur.Core.Entities.Settings;

namespace Probel.Lanceur.Core.Services
{
    public interface ISettingsService
    {
        #region Methods

        AppSettings Get();

        void Save(AppSettings settings);

        #endregion Methods
    }
}