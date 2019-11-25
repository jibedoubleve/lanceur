using Probel.Lanceur.Core.Entities;

namespace Probel.Lanceur.Core.Services
{
    public interface IParameterResolver
    {
        #region Methods

        Alias Resolve(Alias cmd, string parameters);

        Cmdline Split(string cmdline);

        #endregion Methods
    }
}