﻿using Probel.Lanceur.Plugin.Evernote.Services;
using System;

namespace Probel.Lanceur.Plugin.Evernote
{
    public class Plugin : PluginBase
    {
        #region Methods

        public override void Execute(Cmdline cmd)
        {
            try
            {                
                var exec = new CmdExecutor();

                if (exec.IsList(cmd.Parameters))
                {
                    var resul = exec.List();
                }
                else
                {
                    exec.Execute(cmd);
                    Notifyer.NotifyInfo($"Evernote: note created.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Cannot execute EN plugin: {ex.Message}", ex);
                Notifyer.NotifyError($"Cannot execute EN plugin: {ex.Message}");
            }
        }

        #endregion Methods
    }
}