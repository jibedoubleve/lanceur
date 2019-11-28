using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IAliasService
    {
        #region Methods

        void Execute(string param);
        IEnumerable<string> GetAliasNames(long sessionId);

        #endregion Methods
    }
}