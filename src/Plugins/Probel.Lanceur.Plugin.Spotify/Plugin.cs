using Probel.Lanceur.Plugin.Spotify.Helpers;
using Probel.Lanceur.Plugin.Spotify.Spotify;
using Probel.Lanceur.Plugin.Spotify.ViewModels;
using Probel.Lanceur.Plugin.Spotify.Views;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Probel.Lanceur.Plugin.Spotify
{
    public class Plugin : PluginBase
    {
        #region Fields

        private static Player _player;
        private readonly Timer _timer;

        #endregion Fields

        #region Constructors

        public Plugin()
        {
            _timer = new Timer() { Interval = TimeSpan.FromSeconds(0.5).TotalMilliseconds };
            _timer.Elapsed += OnTimerElapsed;
        }

        #endregion Constructors

        #region Properties

        public MainView View { get; private set; }

        public MainViewModel ViewModel { get; private set; }

        #endregion Properties

        #region Methods

        private async Task ConfigureSpotifyAsync()
        {
            if (_player == null)
            {
                var c = new Connector();
                var spotify = await c.GetClientAsync();
                if (spotify != null) { _player = new Player(spotify); }
            }
            if (ViewModel.Player == null) { ViewModel.Player = _player; }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_player != null)
            {
                var song = _player.GetCurrentSong();
                ViewModel.Load(song);
            }
            else { _timer.Stop(); }
        }

        protected override void Initialise()
        {
            View = new MainView();
            ViewModel = View.DataContext as MainViewModel;

            ViewModel.Pause = () => _timer?.Stop();
            ViewModel.Resume = () => _timer?.Start();

            MainView.SetPluginArea(View);
        }

        public override async void Execute(PluginCmdline parameters)
        {
            ViewModel.Log = Logger;
            MainView.ShowPlugin();

            await ConfigureSpotifyAsync();

            _timer.Start();

            if (parameters.IsNextSong()) { _player.GotoNextSong(); }
            else if (parameters.IsPreviousSong()) { _player.GotoPreviousSong(); }
            else if (parameters.IsRestartSong()) { _player.GotoSameSong(); }
        }

        #endregion Methods
    }
}