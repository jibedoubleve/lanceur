using Probel.Lanceur.Plugin.Evernote.Services;
using Probel.Lanceur.Plugin.Evernote.Views;
using System;
using System.Linq;

namespace Probel.Lanceur.Plugin.Evernote
{
    public class Plugin : PluginBase
    {
        #region Properties

        private SettingsView View { get; set; }

        #endregion Properties

        #region Methods

        public override void Execute(Cmdline cmd)
        {
            try
            {
                var s = SettingsService.Get();
                var exec = new CmdExecutor(s);

                if (s.IsEmpty() || DoShowConfig(cmd)) { MainView.SetPluginArea(View); }
                else if (exec.IsList(cmd.Parameters))
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

        protected override void Initialise() => View = new SettingsView();

        private bool DoShowConfig(Cmdline cmd)
        {
            var hasArg = (from a in cmd.Parameters.Split(' ')
                          where a.ToLower() == "config"
                          select a).Any();
            return hasArg;
        }

        #endregion Methods
    }
}