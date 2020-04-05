using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IAliasService
    {
        #region Methods

        /// <summary>
        /// Executes the command line.
        /// </summary>
        /// <param name="cmdline">The command line to execute.
        /// That's the alias and the arguments (which are NOT
        /// mandatory)</param>
        /// <returns><c>True</c> if execution succeeded. Otherwise <c>False</c></returns>
        ExecutionResult Execute(string param);

        IEnumerable<AliasText> GetAliasNames(long sessionId);

        IEnumerable<AliasText> GetAliasNames(long sessionId, string criterion);

        string GetSession(long id);

        #endregion Methods
    }

    public class ExecutionResult
    {
        #region Constructors

        private ExecutionResult(bool isError, bool keepShowing)
        {
            IsError = isError;
            KeepShowing = keepShowing;
        }

        #endregion Constructors

        #region Properties

        public static ExecutionResult Failure => new ExecutionResult(true, true);
        public static ExecutionResult SuccessHide => new ExecutionResult(false, false);
        public static ExecutionResult SuccesShow => new ExecutionResult(false, true);
        public bool IsError { get; }
        public bool KeepShowing { get; }

        #endregion Properties
    }
}