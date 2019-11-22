namespace Probel.Lanceur.Core.Entities
{
    public static class ShortcutExtension
    {
        #region Methods

        public static Shortcut Clone(this Shortcut src)
        {
            var ret = new Shortcut()
            {
                Arguments = src.Arguments,
                FileName = src.FileName,
                Id = src.Id,
                IsExecutable = src.IsExecutable,
                Name = src.Name,
                Notes = src.Notes,
                RunAs = src.RunAs,
                StartMode = src.StartMode,
            };
            return ret;
        }

        #endregion Methods
    }

    public class Shortcut : BaseShortcut
    {
        #region Properties

        public long IdSession { get; set; }
        public string Name { get; set; } = string.Empty;

        #endregion Properties

        #region Methods

        public static Shortcut Empty(string name) => new Shortcut() { Name = name.ToUpper(), IsExecutable = false };

        public static implicit operator Shortcut(string name) => new Shortcut() { FileName = name };

        #endregion Methods
    }
}