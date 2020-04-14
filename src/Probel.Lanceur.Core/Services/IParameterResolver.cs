using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Plugin;

namespace Probel.Lanceur.Core.Services
{
    public interface IParameterResolver
    {
        #region Methods

        /// <summary>
        /// Take command from <paramref name="cmdline1"/> and merge it with
        /// parameters from <paramref name="cmdline2"/>.
        /// </summary>
        /// <param name="cmdline1">Command line with the command to use</param>
        /// <param name="cmdline2">Command line with the parameters to use</param>
        /// <returns>A merged command line</returns>
        Cmdline Merge(string cmdline1, string cmdline2);

        Alias Resolve(Alias cmd, string parameters);

        Cmdline Split(string cmdline);

        #endregion Methods
    }
}