namespace Probel.Lanceur.Core.Entities
{
    public static class AliasExtension
    {
        #region Methods

        public static Alias Clone(this Alias src)
        {
            var ret = new Alias()
            {
                Arguments = src.Arguments,
                FileName = src.FileName,
                Id = src.Id,
                IdSession = src.IdSession,
                IsExecutable = src.IsExecutable,
                Name = src.Name,
                Notes = src.Notes,
                RunAs = src.RunAs,
                StartMode = src.StartMode,
                WorkingDirectory = src.WorkingDirectory,
            };
            return ret;
        }

        public static void Normalise(this Alias src)
        {
            var t = new char[] { '"' };
            src.Arguments = src.Arguments?.Trim(t);
            src.FileName = src.FileName?.Trim(t);
        }

        #endregion Methods
    }

    public class Alias : BaseAlias
    {
        #region Fields

        private const string PackageTemplate = "package:";

        #endregion Fields

        #region Properties

        public long IdSession { get; set; }

        public bool IsPackaged => FileName?.StartsWith(PackageTemplate) ?? false;

        public string Name { get; set; } = string.Empty;

        public string UniqueIdentifier
        {
            get
            {
                return (string.IsNullOrEmpty(FileName))
                    ? string.Empty
                    : FileName.Replace(PackageTemplate, "");
            }
        }

        #endregion Properties

        #region Methods

        public static Alias Empty(string name = "") => new Alias()
        {
            FileName = string.Empty,
            Name = name.ToUpper(),
            IsExecutable = false
        };

        public static explicit operator Alias(string name) => new Alias() { FileName = name };

        public static explicit operator Alias(long id) => new Alias() { Id = id };

        public static Alias Reserved(string name) => new Alias
        {
            FileName = "__RESERVED_KEYWORD__",
            Name = name.ToUpper(),
            IsExecutable = false
        };

        #endregion Methods
    }
}