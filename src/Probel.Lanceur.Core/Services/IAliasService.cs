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
        /// <param name="sessionId">ID of the current session</param>
        /// <returns><c>True</c> if execution succeeded. Otherwise <c>False</c></returns>
        ExecutionResult Execute(Query alias, long sessionId);

        IEnumerable<Query> GetAliasNames(long sessionId, string keyword);

        string GetSession(long id);

        #endregion Methods
    }

    public class ExecutionResult
    {
        #region Constructors

        private ExecutionResult(bool isError, bool keepShowing, string error = null)
        {
            Error = error;
            IsError = isError;
            KeepShowing = keepShowing;
        }

        #endregion Constructors

        #region Properties

        public static ExecutionResult None => new ExecutionResult(false, false);
        public static ExecutionResult SuccessHide => new ExecutionResult(false, false);

        public static ExecutionResult SuccesShow => new ExecutionResult(false, true);

        public string Error { get; }

        public bool IsError { get; }

        public bool KeepShowing { get; }

        #endregion Properties

        #region Methods

        public static ExecutionResult Failure(string error = "An error occured") => new ExecutionResult(true, true, error);

        #endregion Methods
    }
}