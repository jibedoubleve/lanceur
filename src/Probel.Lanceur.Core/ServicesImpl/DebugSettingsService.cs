using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Infrastructure;

namespace Probel.Lanceur.Core.ServicesImpl
{
    public class DebugSettingsService : ISettingsService
    {
        #region Fields

        private static AppSettings _appSettings = null;

        private readonly ILogService _logger;

        #endregion Fields

        #region Constructors

        public DebugSettingsService(ILogService logger)
        {
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public AppSettings Get()
        {
            if (_appSettings == null)
            {
                _appSettings = new AppSettings
                {
                    DatabasePath = @"%appdata%\probel\Lanceur\debug_data.db",
                    SessionId = 1,
                    WindowSection = new WindowSettings()
                    {
                        Colour = "#DC143C",
                        Position = new PositionSettings()
                        {
                            Left = 600,
                            Top = 266
                        },
                        ShowAtStartup = true,
                    }
                };
            }
            return _appSettings;
        }

        public void Save(AppSettings settings) => _logger.Trace($"[DEBUG MODE] Saving settings");

        public void SavePosition(AppSettings src) => _logger.Trace($"[DEBUG MODE] Saving position");

        #endregion Methods
    }
}