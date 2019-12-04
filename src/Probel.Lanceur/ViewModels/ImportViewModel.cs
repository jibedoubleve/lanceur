using Caliburn.Micro;
using Probel.Lanceur.Core.Services;
using System;

namespace Probel.Lanceur.ViewModels
{
    public class ImportViewModel : Screen
    {
        #region Fields

        private readonly ISettingsService _settings;

        private string _colour;
        private string _output;
        private long _progress;

        #endregion Fields

        #region Constructors

        public ImportViewModel(ISettingsService settings)
        {
            _settings = settings;
        }

        #endregion Constructors

        #region Properties

        public string Colour
        {
            get => _colour;
            set => Set(ref _colour, value, nameof(Colour));
        }

        public string Output
        {
            get => _output;
            set => Set(ref _output, value, nameof(Output));
        }

        public long Progress
        {
            get => _progress;
            set => Set(ref _progress, value, nameof(Progress));
        }

        #endregion Properties

        #region Methods

        public void LoadSettings() => Colour = _settings.Get().WindowSection.Colour;


        public void Update(long progress, string text)
        {
            Output += $"{text}{Environment.NewLine}";
            Progress = progress;
        }

        #endregion Methods
    }
}