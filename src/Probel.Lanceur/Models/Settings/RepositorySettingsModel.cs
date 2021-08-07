using Caliburn.Micro;

namespace Probel.Lanceur.Models.Settings
{
    public class RepositorySettingsModel : PropertyChangedBase
    {
        #region Fields

        private float _scoreLimit;

        #endregion Fields

        #region Properties

        public float ScoreLimit
        {
            get => _scoreLimit;
            set => Set(ref _scoreLimit, value);
        }

        #endregion Properties
    }
}