using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    class Library
    {
        #region Members
        private string url;
        private List<Song> songsInLibrary;
        #endregion
        #region Properties
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public List<Song> SongsInLibrary
        {
            get { return songsInLibrary; }
            set { songsInLibrary = value; }
        }
        #endregion
        #region Constructors
        public Library(string url)
        {
            this.url = url;
            songsInLibrary = new List<Song>();
            LoadSongs();
        }
        #endregion


        #region PrivateMethods

        #endregion
        #region Methods

        private void LoadSongs()
        {
            if (Directory.Exists(url))
            {
                List<string> songsUrl = new List<string>(Directory.EnumerateFiles(url, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".wav", StringComparison.OrdinalIgnoreCase)));
                for (int i = 0; i < songsUrl.Count; i++)
                {
                    try
                    {
                        TagLib.File newSong = TagLib.File.Create(songsUrl.ElementAt(i));
                        songsInLibrary.Add(newSong);
                    }
                    catch (TagLib.CorruptFileException) { }
                }
            }
        }
        #endregion
    }
}
