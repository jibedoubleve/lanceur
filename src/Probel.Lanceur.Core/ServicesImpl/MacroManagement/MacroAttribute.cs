using System;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    internal class MacroAttribute : Attribute
    {
        private readonly string _name;
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