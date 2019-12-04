﻿using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Services;
using System;
using System.Linq;
using System.Reflection;

namespace Probel.Lanceur.Core.ServicesImpl.MacroManagement
{
    public class MacroService : IMacroService
    {
        #region Fields

        private readonly ILogService _log;
        private ICommandRunner _cmdrunner;
        private IAliasService _aliasService;

        #endregion Fields

        #region Constructors

        public MacroService(ILogService logservice)
        {
            _log = logservice;
        }

        #endregion Constructors

        #region Methods

        public void Handle(Alias cmd)
        {
            var types = from t in Assembly.GetAssembly(typeof(IMacroAction)).GetTypes()
                        where t.GetCustomAttribute<MacroAttribute>() != null
                        && t.GetCustomAttribute<MacroAttribute>().Name == cmd.FileName.ToUpper()
                        select t;

            foreach (var type in types)
            {
                _log.Trace($"Found maro action of type '{type}'");
                var action = (IMacroAction)Activator.CreateInstance(type);
                action.With(_cmdrunner, _log, _aliasService)
                      .Execute(cmd);
            }
        }

        public bool Has(string name) => Macros.Has(name);
        public IMacroService With(ICommandRunner cmdrunner, IAliasService aliasService)
        {
            _cmdrunner = cmdrunner;
            _aliasService = aliasService;
            return this;
        }

        #endregion Methods
    }
}