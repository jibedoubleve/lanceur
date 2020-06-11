using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    /// <summary>
    /// Manage all the macro of the application. A Macro is a special 
    /// reserved word. It is used for instance launch multiple alias
    /// in one keyword
    /// </summary>
    public interface IMacroRunner
    {
        #region Methods

        void Execute(Alias cmd);

        bool Exists(string name);

        #endregion Methods
    }
}