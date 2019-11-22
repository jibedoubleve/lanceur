using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;

namespace Probel.Lanceur.Core.Services
{
    public interface IDatabaseService
    {
        #region Methods

        void Clear();

        void Create(Shortcut shortcut);

        void Delete(Shortcut shortcut);

        IEnumerable<ShortcutName> GetNamesOf(Shortcut shortcut);

        IEnumerable<ShortcutSession> GetSessions();

        Shortcut GetShortcut(string name);

        IEnumerable<string> GetShortcutNames(long sessionId);

        IEnumerable<Shortcut> GetShortcuts(long sessionId);

        void SetUsage(Shortcut shortcut);

        void SetUsage(long idShortcut);

        void Update(Shortcut shortcut);

        void Update(IEnumerable<ShortcutName> names);

        ShortcutSession GetSession(long sessionId);
        
        void Update(ShortcutSession session);
        
        void Delete(ShortcutSession session);

        #endregion Methods
    }
}