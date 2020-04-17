﻿using EvernoteSDK;
using EvernoteSDK.Advanced;
using Probel.Lanceur.Plugin.Evernote.Models;
using System;

namespace Probel.Lanceur.Plugin.Evernote.Services
{
    internal class EvernoteService
    {
        #region Fields

        private Settings _settings = null;

        #endregion Fields

        #region Properties

        private Settings Settings
        {
            get
            {
                if (_settings == null) { _settings = SettingsService.Get(); }
                return _settings;
            }
        }

        #endregion Properties

        #region Methods

        public void SaveNote(string title) => Create(title);

        public void SaveReminder(DateTime dt, string title) => Create(title, dt);

        internal object GetInboxNotes()
        {
            Connect();

            // Search for some text across all notes (i.e. personal, shared, and business).
            // Change the Search Scope parameter to limit the search to only personal, shared, business - or combine flags for some combination.
            //var results = ENSession.SharedSession.FindNotes(ENNoteSearch.NoteSearch("*"), null, ENSession.SearchScope.All, ENSession.SortOrder.RecentlyUpdated, 500);
            var results = ENSession.SharedSession.FindNotes(ENNoteSearch.NoteSearch(""), null, ENSession.SearchScope.Personal, ENSession.SortOrder.Normal, 21);

            return results;
        }

        private void Create(string title, DateTime? dt = null)
        {
            Connect();

            // Create a note (in the user's default notebook) with an attribute set (in this case, the ReminderOrder attribute to create a Reminder).
            ENNoteAdvanced note = new ENNoteAdvanced { Title = title };
            note.Content = ENNoteContent.NoteContentWithString("");
            if (dt.HasValue) { note.EdamAttributes["ReminderOrder"] = dt.Value.ToEdamTimestamp(); }

            ENSession.SharedSession.UploadNote(note, null);
        }

        private void Connect()
        {
            // Supply your key using ENSessionAdvanced instead of ENEsssion, to indicate your use of the Advanced interface.
            // Be sure to put your own consumer key and consumer secret here.
            ENSessionAdvanced.SetSharedSessionConsumerKey(Settings.Host, Settings.Key, Settings.Server);

            if (ENSession.SharedSession.IsAuthenticated == false)
            {
                ENSession.SharedSession.AuthenticateToEvernote();
            }
        }

        #endregion Methods
    }
}