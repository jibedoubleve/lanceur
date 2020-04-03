using Probel.Lanceur.Core.Entities;
using System;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{

    public interface IReservedKeywordService
    {
        #region Methods

        void Bind(string keyword, Action<string> bindedAction);

        void ExecuteActionFor(string name, string arg);

        IEnumerable<AliasText> GetKeywords();

        bool IsReserved(string cmd);

        #endregion Methods
    }
}