using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    public interface IParameterResolver
    {
        #region Methods

        Shortcut Resolve(Shortcut cmd, string parameters);

        Cmdline Split(string cmdline);

        #endregion Methods
    }
}