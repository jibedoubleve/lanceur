using System;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    /// <summary>
    /// A macro if a list of alias to be run sequentially.
    /// </summary>
    internal sealed class MacroAttribute : Attribute
    {
        #region Fields

        private readonly string _name;

        #endregion Fields

        #region Constructors

        public MacroAttribute(string name)
        {
            _name = name;
        }

        #endregion Constructors

        #region Properties

        public string Name => $"@{_name.Replace("@", "").ToUpper()}@";

        #endregion Properties
    }
}