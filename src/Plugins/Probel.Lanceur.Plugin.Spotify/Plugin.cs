using Probel.Lanceur.Plugin.Spotify.Spotify;
using Probel.Lanceur.Plugin.Spotify.ViewModels;
using Probel.Lanceur.Plugin.Spotify.Views;
using Probel.Lanceur.Plugin;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Probel.Lanceur.Plugin.Spotify
{
    public class Plugin : PluginBase
    {
        #region Fields

        private static Player _player;
        private Timer _timer;

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

        public override async void Execute(Cmdline parameters)
        {
            ViewModel.Log = Logger;
            MainView.HideResults();

            await ConfigureSpotifyAsync();

            _timer.Start();
        }

        protected override void Initialise()
        {
            View = new MainView();
            ViewModel = View.DataContext as MainViewModel;

            ViewModel.Pause = () => _timer?.Stop();
            ViewModel.Resume = () => _timer?.Start();

            MainView.SetPluginArea(View);
        }

        private async Task ConfigureSpotifyAsync()
        {
            if (_player == null)
            {
                var c = new Connector(Logger);
                var spotify = await c.GetClientAsync();
                if (spotify != null) { _player = new Player(spotify); }
            }
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

        #endregion Methods
    }
}