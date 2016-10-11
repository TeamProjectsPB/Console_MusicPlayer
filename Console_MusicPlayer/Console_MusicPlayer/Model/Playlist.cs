using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace Console_MusicPlayer.Model
{
    class Playlist
    {
        #region Members
        private string name;
        private List<TagLib.File> tracks;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<File> Tracks
        {
            get { return tracks; }
            set { tracks = value; }
        }

        #endregion

        #region Constructors
        public Playlist(string name)
        {
            this.name = name;
        }

        public Playlist(List<File> tracks)
        {
            name = string.Empty;
            this.tracks = tracks;
        }

        public Playlist(string name, List<File> tracks)
        {
            this.name = name;
            this.tracks = tracks;
        }

        #endregion

        #region Methods
        public void AddSong(TagLib.File song)
        {
            tracks.Add(song);
        }

        public void DeleteSong(TagLib.File song)
        {
            tracks.Remove(song);
        }

        public void MoveSong(TagLib.File song, int move)
        {
            int songIndex = tracks.IndexOf(song);
            tracks.Remove(song);
            tracks.Insert(songIndex + move, song);
        }
        #endregion
    }
}
