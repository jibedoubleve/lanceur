using Probel.Lanceur.Core.Entities;

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
        /// <param name="sessionId">Id the the session where the search is done</param>
        /// <returns>A merged command line</returns>
        Cmdline Merge(string cmdline1, string cmdline2, long sessionId);

        Alias Resolve(Alias cmd, string parameters);

        string Resolve(string source, string parameters);

        Cmdline Split(string cmdline, long sessionId);

        #endregion Methods
    }
}