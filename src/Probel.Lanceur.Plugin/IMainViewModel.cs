using System.Collections.Generic;

namespace Probel.Lanceur.Plugin
{
    public interface IMainViewModel
    {
        #region Properties

        IEnumerable<object> Results { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Replace the results of the ViewModel with specified results.
        /// The results will be handle in the UI. 
        /// </summary>
        /// <param name="source">The results that replace the old one</param>
        /// <param name="keepalive">Indicate whether the result should be visible
        /// after <c>ENTER</c> was pressed</param>
        void SetResult(IEnumerable<object> source, bool keepalive = false);

        #endregion Methods
    }
}