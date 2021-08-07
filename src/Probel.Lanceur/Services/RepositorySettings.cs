using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Repositories;

namespace Probel.Lanceur.Services
{
    public class RepositorySettings : IRepositorySettings
    {
        #region Fields

        private readonly ISettingsService _settings;

        #endregion Fields

        #region Constructors

        public RepositorySettings(ISettingsService settings)
        {
            _settings = settings;
        }

        #endregion Constructors

        #region Properties

        public float ScoreLimit => _settings.Get().RepositorySection.ScoreLimit;

        #endregion Properties
    }
}