using Probel.Lanceur.Infrastructure;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Probel.Lanceur.Plugin.Spotify.Spotify
{
    public class Connector
    {
        #region Fields

        public static Timer _timer = new Timer { Interval = (int)TimeSpan.FromMinutes(30).TotalMilliseconds };

        private static readonly TokenSwapAuth _auth = new TokenSwapAuth(
            "https://songify.rocks/auth/_index.php",
            "http://localhost:4002/auth",
            Scope.UserModifyPlaybackState | Scope.UserReadPlaybackState | Scope.UserReadCurrentlyPlaying);

        private static bool _isAuthenticated = false;
        private static Token _lastToken;
        private static SpotifyWebAPI _spotify;
        private readonly int _timeoutSeconds;

        #endregion Fields

        #region Constructors

        public Connector(ILogService service, int timeoutSeconds = 5)
        {
            _timeoutSeconds = timeoutSeconds;
        }

        #endregion Constructors

        #region Methods

        public async Task<SpotifyWebAPI> GetClientAsync()
        {
            _timer.Elapsed += OnAutoRefresh;
            var cfg = JsonConfig.Load();

            // If Refresh and Accesstoken are present, just refresh the auth
            if (!string.IsNullOrEmpty(cfg.RefreshToken) && !string.IsNullOrEmpty(cfg.AccessToken))
            {
                _isAuthenticated = true;

                var rt = await _auth.RefreshAuthAsync(cfg.RefreshToken);

                if (rt == null) { throw new NullReferenceException(nameof(rt)); }

                _spotify = new SpotifyWebAPI()
                {
                    TokenType = rt.TokenType,
                    AccessToken = rt.AccessToken
                };

                cfg.AccessToken = rt.AccessToken;
                JsonConfig.Save(cfg);
            }
            else { _isAuthenticated = false; }

            _auth.AuthReceived += async (sender, payload) =>
            {
                if (_isAuthenticated) { return; }

                _lastToken = await _auth.ExchangeCodeAsync(payload.Code);

                cfg.RefreshToken = _lastToken.RefreshToken;
                cfg.AccessToken = _lastToken.AccessToken;
                JsonConfig.Save(cfg);

                _spotify = new SpotifyWebAPI()
                {
                    TokenType = _lastToken.TokenType,
                    AccessToken = _lastToken.AccessToken,
                };
                _auth.Stop();
            };

            _auth.OnAccessTokenExpired += async (sender, payload) =>
            {
                _spotify.AccessToken = (await _auth.RefreshAuthAsync(cfg.RefreshToken)).AccessToken;
                cfg.RefreshToken = _lastToken.RefreshToken;
                cfg.AccessToken = _spotify.AccessToken;
                JsonConfig.Save(cfg);
            };

            _auth.Start();

            if (_isAuthenticated) { _timer.Start(); }
            else { _auth.OpenBrowser(); }

            for (int i = 0; i < _timeoutSeconds * 4; i++)
            {
                if (_spotify != null) { return _spotify; }
                else { await Task.Delay(250); }
            }

            // We've reached the time out for the creatoion of the client
            throw new TimeoutException("The creation of an spotify client has timed out.");
        }

        private async void OnAutoRefresh(object sender, ElapsedEventArgs e)
        {
            var cfg = JsonConfig.Load();

            // When the timer elapses the tokens will get refreshed
            _spotify.AccessToken = (await _auth.RefreshAuthAsync(cfg.RefreshToken)).AccessToken;
            cfg.AccessToken = _spotify.AccessToken;

            JsonConfig.Save(cfg);
        }

        #endregion Methods
    }
}