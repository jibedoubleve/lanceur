using SpotifyAPI.Web.Models;
using System.Collections.Generic;

namespace Probel.Lanceur.Plugin.Spotify.Spotify
{
    public class TrackInfo
    {
        #region Properties

        public List<Image> AlbumImages { get; internal set; }
        public string Artists { get; internal set; }
        public int Duration { get; internal set; }
        public bool IsPlaying { get; internal set; }
        public int Progress { get; internal set; }
        public string SongId { get; internal set; }
        public string Title { get; internal set; }

        #endregion Properties
    }
}