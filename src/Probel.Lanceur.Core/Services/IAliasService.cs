﻿using Probel.Lanceur.Core.Entities;
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
        bool Execute(string param);

        IEnumerable<AliasText> GetAliasNames(long sessionId);

        IEnumerable<AliasText> GetAliasNames(long sessionId, string criterion);

        #endregion Methods
    }
}