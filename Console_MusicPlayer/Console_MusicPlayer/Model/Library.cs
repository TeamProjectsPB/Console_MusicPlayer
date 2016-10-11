using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = TagLib.File;

namespace Console_MusicPlayer.Model
{
    class Library
    {
        #region Members
        private string url;
        private List<TagLib.File> songsInLibrary;
        private List<Playlist> playlistsInLibrary;
        #endregion
        #region Properties
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public List<File> SongsInLibrary
        {
            get { return songsInLibrary; }
            set { songsInLibrary = value; }
        }

        public List<Playlist> PlaylistsInLibrary
        {
            get { return playlistsInLibrary; }
            set { playlistsInLibrary = value; }
        }
        #endregion
        #region Constructors
        public Library(string url)
        {
            this.url = url;
            songsInLibrary = new List<File>();
            playlistsInLibrary = new List<Playlist>();
        }
        #endregion

        #region PrivateMethods

        /*private void ReadPlaylist()
        {
            
        }*/
        #endregion
        #region Methods

        public void GetSongs()
        {
            List<string> songsUrl = new List<string>(Directory.EnumerateFiles(url, "*.*").Where(s => s.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) || s.ToLower().EndsWith(".wav", StringComparison.OrdinalIgnoreCase)));
            for (int i = 0; i < songsUrl.Count; i++)
            {
                TagLib.File newSong = TagLib.File.Create(songsUrl.ElementAt(i));
                songsInLibrary.Add(newSong);
            }
        }

        /*public void GetPlaylists()
        {
            List<string> playlistsUrls = new List<string>(Directory.EnumerateFiles(url, "*.*").Where(s => s.EndsWith(".playlist", StringComparison.OrdinalIgnoreCase)));


        }*/


        #endregion
    }
}
