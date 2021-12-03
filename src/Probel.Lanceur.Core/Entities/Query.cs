using Probel.Lanceur.Core.Services;
using System;

namespace Probel.Lanceur.Core.Entities
{
    public class Query
    {
        #region Fields

        private string _name;

        #endregion Fields

        #region Properties

        public long ExecutionCount { get; set; }
        public string FileName { get; set; }
        public string Icon { get; set; }
        public long Id { get; set; }

        /// <summary>
        /// Indicates whether the alias can be executed as is or
        /// Further information is needed to execute it. For instance
        /// MagicWords are not executable as you need to retrieve
        /// information into the DB to execute it.
        /// </summary>
        public bool IsExecutable { get; set; } = true;

        /// <summary>
        /// Indicates whether the alias is a packaged application (UWP)
        /// </summary>
        public bool IsPackaged { get; set; }

        public virtual string Kind { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NameLowercase = value.ToLower();
            }
        }

        public string NameLowercase { get; private set; }

        public string Parameters { get; private set; }
        public double SearchScore { get; set; }

        /// <summary>
        /// Represents a unique id of the UWP packaged application. If
        /// <see cref="IsPackaged"/> is set to <c>False</c> then, this
        /// property should be <c>NULL</c>
        /// </summary>
        public string UniqueIdentifier { get; set; }

        #endregion Properties

        #region Methods

        private static (string, string) Split(string cmdline)
        {
            if (string.IsNullOrEmpty(cmdline)) { throw new ArgumentException($"The command line should NOT be empty or null", nameof(cmdline)); }

            var index = cmdline.IndexOf(' ');

            var @params = string.Empty;
            var cmd = cmdline;

            if (index > 0)
            {
                cmd = cmdline.Substring(0, index);
                @params = cmdline.Substring(index, cmdline.Length - index)?.Trim();
            }

            return (cmd, @params);
        }

        public static Query FromTextToCommandLine(string cmdline)
        {
            var (cmd, @params) = Split(cmdline);

            return new Query
            {
                FileName = cmd,
                Name = $@"Executing '{cmd}'",
                Parameters = @params,
                IsExecutable = true,
                Kind = "Console"
            };
        }
        public static Query FromText(string cmdline)
        {
            var (cmd, @params) = Split(cmdline);

            return new Query
            {
                FileName = cmd,
                Name = cmd,
                Parameters = @params,
            };
        }

        public static implicit operator string(Query alias) => alias?.ToString() ?? string.Empty;

        public static Query ReservedKeyword(ActionWord word)
        {
            return new Query
            {
                Name = word.Name.ToLower(),
                ExecutionCount = 0,
                FileName = $"{word.Description}",
                IsExecutable = false,
                Kind = "Settings",
            };
        }

        public void SetParameters(string cmdline)
        {
            if (string.IsNullOrEmpty(cmdline)) { throw new ArgumentException($"The command line should NOT be empty or null", nameof(cmdline)); }

            var (_, @params) = Split(cmdline);

            Parameters = string.IsNullOrEmpty(@params)
                ? cmdline.Replace(Name.Trim(), "")
                : @params;
        }

        public override string ToString() => $"{(Name ?? string.Empty)} {Parameters}";

        #endregion Methods
    }
}