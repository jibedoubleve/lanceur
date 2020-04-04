namespace Probel.Lanceur.Core.Entities.Settings
{
    public class DatabaseSettings
    {
        #region Properties

        public string DatabaseName { get; set; } = @"data.db";
        public string DatabasePath { get; set; } = @"%appdata%\probel\Lanceur\";

        #endregion Properties
    }
}