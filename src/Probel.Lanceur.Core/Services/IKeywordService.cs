using Probel.Lanceur.Core.Constants;
using System;

namespace Probel.Lanceur.Core.Services
{
    public interface IKeywordService
    {
        #region Methods

        void Bind(Keywords cmd, Action<string> bindedAction);

        void ExecuteActionFor(string name, string arg);

        bool IsReserved(string cmd);

        #endregion Methods
    }
}