using Probel.Lanceur.Core.Entities;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{

    public interface IKeywordService
    {
        #region Methods

        void Bind(string keyword, Func<string, ExecutionResult> bindedAction);

        ExecutionResult ExecuteActionFor(string name, string arg);

        IEnumerable<AliasText> GetKeywords();

        bool IsReserved(string cmd);

        #endregion Methods
    }
}