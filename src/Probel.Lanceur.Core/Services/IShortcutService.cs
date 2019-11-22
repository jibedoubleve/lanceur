using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IShortcutService
    {
        #region Methods

        void Execute(string param);
        IEnumerable<string> GetShortcutsNames(long sessionId);

        #endregion Methods
    }
}