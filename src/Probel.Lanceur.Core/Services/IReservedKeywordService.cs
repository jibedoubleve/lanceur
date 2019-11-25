using Probel.Lanceur.Core.Constants;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IReservedKeywordService
    {
        #region Methods

        void Bind(Keywords cmd, Action<string> bindedAction);

        void ExecuteActionFor(string name, string arg);

        IEnumerable<string> GetReservedKeywords();

        bool IsReserved(string cmd);

        #endregion Methods
    }
}