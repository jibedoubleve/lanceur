using Probel.Lanceur.Plugin.Evernote.Models;
using System;
using System.Text.RegularExpressions;

namespace Probel.Lanceur.Plugin.Evernote.Services
{
    internal class CmdExecutor
    {
        #region Fields

        private readonly EvernoteService _evernoteService = new EvernoteService();
        private readonly Settings _settings;

        #endregion Fields

        #region Constructors

        public CmdExecutor(Settings settings)
        {
            _settings = settings;
        }

        #endregion Constructors

        #region Methods

        public void Execute(Cmdline cmd)
        {
            if (_settings.IsEmpty() == false && IsReminder(cmd.Parameters)) { CreateReminder(cmd.Parameters); }
            else if (_settings.IsEmpty() == false && IsNote(cmd.Parameters)) { CreateNote(cmd.Parameters); }
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