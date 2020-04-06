using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probel.Lanceur.Plugin.Spotify.Spotify
{
    public class Player
    {
        #region Fields

        private readonly SpotifyWebAPI _spotify;

        #endregion Fields

        #region Constructors

        public Player(SpotifyWebAPI spotify)
        {
            _spotify = spotify;
        }

        #endregion Constructors

        #region Methods

        public TrackInfo GetCurrentSong()
        {
            var context = _spotify.GetPlayingTrack();
            var artists = string.Empty;

            if (context.Item != null)
            {

                for (var i = 0; i < context.Item.Artists.Count; i++)
                {
                    if (i != context.Item.Artists.Count - 1) { artists += context.Item.Artists[i].Name + ", "; }
                    else { artists += context.Item.Artists[i].Name; }
                }

                return new TrackInfo
                {
                    Artists = artists,
                    Title = context.Item.Name,
                    AlbumImages = context.Item.Album.Images,
                    SongId = context.Item.Id,
                    Duration = context.Item.DurationMs,
                    Progress = context.ProgressMs,
                    IsPlaying = context.IsPlaying,
                };
            }
            else { return new TrackInfo(); }
        }

        public IEnumerable<string> GetDevices()
        {
            var result = new List<string>();
            var devices = _spotify.GetDevices();

            if (devices.HasError()) { Throws(devices); }

            devices?.Devices?.ForEach(d => result.Add(d?.Name ?? "none"));

            return result;
        }

        private void Throws(BasicModel model)
        {
            var msg = $"Spotify error: [{model.Error.Status}] {model.Error.Message}";
            throw new NotSupportedException(msg);
        }

        #endregion Methods
    }
}
