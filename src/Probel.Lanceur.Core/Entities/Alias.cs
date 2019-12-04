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
                IsExecutable = src.IsExecutable,
                Name = src.Name,
                Notes = src.Notes,
                RunAs = src.RunAs,
                StartMode = src.StartMode,
                WorkingDirectory = src.WorkingDirectory,
            };
            return ret;
        }

        #endregion Methods
    }

    public class Alias : BaseAlias
    {
        #region Properties

        public long IdSession { get; set; }
        public string Name { get; set; } = string.Empty;

        #endregion Properties

        #region Methods

        public static Alias Empty(string name) => new Alias()
        {
            FileName = "__RESERVED_KEYWORD__",
            Name = name.ToUpper(),
            IsExecutable = false
        };

        public static implicit operator Alias(string name) => new Alias() { FileName = name };

        #endregion Methods
    }
}