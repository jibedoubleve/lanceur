﻿using Caliburn.Micro;
using Probel.Lanceur.Core.Entities;
using Probel.Lanceur.Core.Entities.Settings;
using Probel.Lanceur.Core.Helpers;
using Probel.Lanceur.Core.Services;
using Probel.Lanceur.Images;
using Probel.Lanceur.Infrastructure;
using Probel.Lanceur.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Probel.Lanceur.ViewModels
{
    public class MainViewModel : Screen, IHandle<string>, IMainViewModel
    {
        #region Fields

        private readonly IAliasService _aliasService;
        private readonly IParameterResolver _resolver;
        private readonly IScreenRuler _screenRuler;
        private readonly ISettingsService _settingsService;
        private AliasText _aliasName;
        private ObservableCollection<object> _aliasNameList;
        private AppSettings _appSettings;
        private string _colour;
        private string _errorMessage;
        private bool _isOnError;
        private double _left;
        private double _opacity;
        private object _selectedResult;
        private string _session;
        private double _top;

        #endregion Fields

        #region Constructors

        public MainViewModel(
            IAliasService aliasService,
            ISettingsService settings,
            IEventAggregator ea,
            IScreenRuler screenRuler,
            ILogService logService,
            IParameterResolver resolver,
            IUserNotifyer notifyer
            )
        {
            Notifyer = notifyer;
            ResultItemHelper.Logger = logService;

            LogService = logService;
            ea.Subscribe(this);

            _resolver = resolver;
            _screenRuler = screenRuler;
            _settingsService = settings;
            _aliasService = aliasService;
        }

        #endregion Constructors

        #region Properties

        public AliasText AliasName
        {
            get => _aliasName;
            set => Set(ref _aliasName, value, nameof(AliasName));
        }

        public AppSettings AppSettings
        {
            get
            {
                _appSettings = _appSettings ?? _settingsService.Get();
                return _appSettings;
            }
            set => _appSettings = value;
        }

        public string Colour
        {
            get => _colour;
            set => Set(ref _colour, value, nameof(Colour));
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
        }

        public bool IsOnError
        {
            get => _isOnError;
            set => Set(ref _isOnError, value);
        }

        public double Left
        {
            get => _left;
            set => Set(ref _left, value);
        }

        public ILogService LogService { get; }

        public IUserNotifyer Notifyer { get; }

        public double Opacity
        {
            get => _opacity;
            set => Set(ref _opacity, value);
        }

        public ObservableCollection<object> Results
        {
            get => _aliasNameList;
            set => Set(ref _aliasNameList, value, nameof(Results));
        }

        IEnumerable<object> IMainViewModel.Results => Results;

        public object SelectedResult
        {
            get => _selectedResult;
            set => Set(ref _selectedResult, value, nameof(SelectedResult));
        }

        public string Session
        {
            get => _session;
            set => Set(ref _session, value, nameof(Session));
        }

        public double Top
        {
            get => _top;
            set => Set(ref _top, value);
        }

        #endregion Properties

        #region Methods

        public ExecutionResult ExecuteText(AliasText alias, string cmdline)
        {
            var sid = _settingsService.Get().SessionId;
            var cmd = _resolver.Split(cmdline, sid);

            if (alias.IsExecutable) { _aliasService.Execute(alias); }
            else { return ExecuteText(alias.AsCommandLine(cmd.Parameters)); }

            return ExecutionResult.SuccesShow;
        }

        public void Handle(string message)
        {
            if (message == Services.UiEvent.CornerCommand)
            {
                var r = _screenRuler.StickTo(Left, Top);
                Left = r.X;
                Top = r.Y;
                SaveSettings();
            }
            if (message == Services.UiEvent.CenterCommand)
            {
                var r = _screenRuler.Center(150);
                Left = r.X;
                Top = r.Y;
                SaveSettings();
            }
        }

        public void LoadAliases() => RefreshAliases();

        public void LoadSettings()
        {
            var s = _settingsService.Get().WindowSection;
            var pos = s.Position;
            Top = pos.Top;
            Left = pos.Left;
            Colour = s.Colour;
            Opacity = s.Opacity;
        }

        public void OnShow()
        {
            // In any case, I don't want to see an hourglass cursor
            // indicating a work is going on when I'm displaying
            // this view as I just want to launch a shortcut!
            Notifyer.NotifyEndWait();

            AppSettings = _settingsService.Get();
            RefreshAliases();
        }

        public void RefreshAliases(string criterion)
        {
            Session = _aliasService.GetSession(AppSettings.SessionId);
            var l = _aliasService.GetAliasNames(AppSettings.SessionId, criterion);
            Results = new ObservableCollection<object>(l.GetRefreshed());
        }

        public void SaveSettings()
        {
            AppSettings.WindowSection.Position.Left = Left;
            AppSettings.WindowSection.Position.Top = Top;

            _settingsService.SavePosition(AppSettings);
            AppSettings = _settingsService.Get();
        }

        public void SetResult(IEnumerable<object> source, bool keepalive = false)
        {
            Results = new ObservableCollection<object>(source);
        }

        /// <summary>
        /// Executes the alias and returns <c>True</c> if execution was a success.
        /// Otherwise returns <c>False</c>
        /// </summary>
        /// <param name="cmdLine">The command line (the alias & the arguments) to be executed.</param>
        /// <returns><c>True</c> on success; otherwise <c>False</c></returns>
        private ExecutionResult ExecuteText(string cmdLine)
        {
            try
            {
                var sid = _settingsService.Get().SessionId;
                var r = _aliasService.Execute(cmdLine, sid);

                if (r.IsError) { ErrorMessage = r.Error; }

                return r;
            }
            catch (Exception ex)
            {
                /* I swallow the error as this crash shouldn't crash the application
                 * I log and continue without any other warning.
                 */
                var msg = $"An error occured while trying to execute the alias '{cmdLine}'";
                LogService.Error(msg, ex);
                return ExecutionResult.Failure(msg);
            }
        }

        private void RefreshAliases()
        {
            Session = _aliasService.GetSession(AppSettings.SessionId);
            var aliases = _aliasService.GetAliasNames(AppSettings.SessionId, null);
            var r = aliases.GetRefreshed();
            Results = new ObservableCollection<object>(r);
        }

        #endregion Methods
    }
}