using Caliburn.Micro;
using Probel.Lanceur.Plugin.Spotify.Spotify;
using Probel.Lanceur.Plugin;
using SpotifyAPI.Web.Models;
using System;
using System.Linq;

namespace Probel.Lanceur.Plugin.Spotify.ViewModels
{
    public class MainViewModel : Screen
    {
        #region Fields

        private static string _artists;
        private static int _duration;
        private static Image _image;
        private static string _title;

        private bool _isVisible;
        private int _progress;

        #endregion Fields

        #region Properties

        public string Artists
        {
            get => _artists;
            set => Set(ref _artists, value, nameof(Artists));
        }

        public int Duration
        {
            get => _duration;
            set => Set(ref _duration, value, nameof(Duration));
        }

        public Image Image
        {
            get => _image;
            set => Set(ref _image, value, nameof(Image));
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value, nameof(IsVisible));
        }

        public ILogService Log { get; internal set; }

        public int Progress
        {
            get => _progress;
            set => Set(ref _progress, value, nameof(Progress));
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value, nameof(Title));
        }

        internal Action Pause { get; set; }
        internal Action Resume { get; set; }

        #endregion Properties

        #region Methods

        public void Load(TrackInfo t)
        {
            Artists = t.Artists;
            Duration = t.Duration;
            Progress = t.Progress;
            Title = t.Title;
            Image = (from i in t.AlbumImages
                     where i.Width == 300
                     select i).FirstOrDefault();
            IsVisible = true;
        }

        #endregion Methods
    }
}