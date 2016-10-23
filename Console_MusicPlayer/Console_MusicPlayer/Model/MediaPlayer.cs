using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    class MediaPlayer
    {
        #region Members
        private WindowsMediaPlayer mPlayer;
        private Song currentSong;
        private Playlist currentplaylist;
        private List<Library> libraries;
        private List<Playlist> playlists;
        #endregion

        #region Properties

        public WindowsMediaPlayer MPlayer
            {
                get { if (mPlayer == null) { return mPlayer = new WindowsMediaPlayer(); } else return mPlayer; }
            }

        public List<Library> Libraries
        {
            get { return libraries; }
            set { libraries = value; }
        }

        public List<Playlist> Playlists
        {
            get { return playlists; }
            set { playlists = value; }
        }

        #endregion

        public MediaPlayer()
        {
                libraries = new List<Library>();
                playlists = new List<Playlist>();    
        }

        private void GetPlaylists()
        {
            List<string> playlistsUrl =
                new List<string>(Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "\\playlists", "*.playlist"));
            foreach (string url in playlistsUrl)
            {
                List<string> songs = PlayListEditor.ReadPlayListFile(url);
                Playlist playlist = new Playlist(Path.GetFileNameWithoutExtension(url));
                foreach (string songUrl in songs)
                {
                    foreach (Library lib in libraries)
                    {
                        if (songUrl.Contains(lib.Url))
                        {
                            Song song = lib.SongsInLibrary.Find(x => x.Name.Equals(songUrl));
                            playlist.AddSong(song);
                        }
                    }
                }
                playlists.Add(playlist);
            }
        }
    }
}
