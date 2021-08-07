using Probel.Lanceur.Plugin.Evernote.Models;
using System;
using System.Text.RegularExpressions;

namespace Probel.Lanceur.Plugin.Evernote.Services
{
    internal class CmdExecutor
    {
        #region Fields

        private readonly EvernoteService _evernoteService = new EvernoteService();

        #endregion Fields

        #region Methods

        public void Configure(string parameters)
        {
            var configurator = new Configurator(parameters);

            if (configurator.IsValid())
            {
                var s = new Settings
                {
                    Key = configurator.Key,
                    Server = configurator.Server,
                    Host = configurator.Host
                };

                Settings.Save(s);
            }
        }

        public void Execute(PluginCmdline cmd)
        {
            if (IsReminder(cmd.Arguments)) { CreateReminder(cmd.Arguments); }
            else if (IsNote(cmd.Arguments)) { CreateNote(cmd.Arguments); }
        }

        public bool IsConfiguration(string parameters)
        {
            var regex = new Regex(@"(-c|config).*");
            return regex.IsMatch(parameters);
        }

        internal bool IsList(string parameters)
        {
            var regex = new Regex(@"(-l|list).*");
            return regex.IsMatch(parameters);
        }

        internal object List() => _evernoteService.GetInboxNotes();

        private static void ThrowInvalid(string parameters) => throw new NotSupportedException($"Cannot execute this command on Evernote: '{parameters}'");

        private void CreateNote(string parameters) => _evernoteService.SaveNote(parameters);

        private void CreateReminder(string parameters)
        {
            // Command should be built on this pattern
            // -r dd-mm-yyyy "text of the title"
            //   -or-
            // reminder dd-mm-yyyy "text of the title"
            var regex = new Regex(@"^(?<command>.*)\s(?<date>\d{2}[-\/]\d{2}[-\/]\d{2,4}[ {0,1}]\d{0,2}[:]{0,1}\d{0,2}|\d)[\s]{0,1}(?<title>.*)");
            var groups = regex.Match(parameters).Groups;

            if (groups.Count == 0) { ThrowInvalid(parameters); return; }
            else
            {
                var title = groups["title"]?.Value;
                if (string.IsNullOrEmpty(title)) { ThrowInvalid(parameters); }
                else if (groups["date"]?.Value != null)
                {
                    var t = DateTime.Today;
                    DateTime date = new DateTime(t.Year, t.Month, t.Day, 09, 00, 00).AddDays(1);

                    if (DateTime.TryParse(groups["date"]?.Value, out var dt)) { date = dt; }
                    else if (int.TryParse(groups["date"]?.Value, out var offset)) { date = date.AddDays(offset); }

                    _evernoteService.SaveReminder(date, title);
                }
                else { _evernoteService.SaveNote(title); }
            }
        }

        private bool IsNote(string parameters) => !IsReminder(parameters);

        private bool IsReminder(string parameters)
        {
            var regex = new Regex(@"(-r|reminder).*");
            return regex.IsMatch(parameters);
        }

        #endregion Methods
    }
}