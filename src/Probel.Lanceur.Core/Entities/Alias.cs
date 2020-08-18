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

        public static string GetUniqueIdentifiyerTemplate(this AliasText src)
        {
            return $"{Alias.PackagePrefix}{src.UniqueIdentifier}";
        }

        #endregion Methods
    }

    public class Alias : BaseAlias
    {
        #region Fields

        internal const string PackagePrefix = "package:";

        #endregion Fields

        #region Properties

        public long IdSession { get; set; }

        public bool IsEmpty => Id == 0;

        public bool IsPackaged => FileName?.StartsWith(PackagePrefix) ?? false;

        public string Name { get; set; } = string.Empty;

        public string UniqueIdentifier
        {
            get
            {
                return (string.IsNullOrEmpty(FileName))
                    ? string.Empty
                    : FileName.Replace(PackagePrefix, "");
            }
        }

        #endregion Properties

        #region Methods

        public static Alias Empty(string name = "") => new Alias()
        {
            Id = 0,
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