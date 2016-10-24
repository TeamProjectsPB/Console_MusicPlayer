using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Console_MusicPlayer.View.Windows;
using WMPLib;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    class MediaPlayer
    {
        #region Members
        private WindowsMediaPlayer mPlayer;
        private Song currentSong;
        private Playlist currentPlaylist;
        private List<Library> libraries;
        private List<Playlist> playlists;
        #endregion

        #region Properties

        public WindowsMediaPlayer MPlayer
        {
            get { if (mPlayer == null) { return mPlayer = new WindowsMediaPlayer(); } else return mPlayer; }
        }

        public Song CurrentSong
        {
            get { return currentSong; }
            set { currentSong = value; }
        }

        public Playlist CurrentPlaylist
        {
            get { return currentPlaylist; }
            set { currentPlaylist = value; }
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

        #region  Constructors
        public MediaPlayer()
        {
            mPlayer = new WindowsMediaPlayer();
            mPlayer.PlayStateChange += Player_PlayStateChange;
            
            libraries = new List<Library>();
            playlists = new List<Playlist>();
        }

        #endregion

        private void Player_PlayStateChange(int newState)
        {
            if ((WMPPlayState) newState == WMPPlayState.wmppsMediaEnded)
            {
                NextTrack();
            }
            /*if ((WMPLib.WMPPlayState)newState == WMPLib.WMPPlayState.wmppsStopped)
            {
                //Actions on stop
            }
            //else if((WMPPlayState)newState == WMPPlayState.wmppsPaused)*/
        }

        public void AddLibrary(string url)
        {
            if (Directory.Exists(url))
            {
                libraries.Add(new Library(url));
            }
        }

        public void LoadPlaylists()
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
            /*if (playlists.Count > 0)
            {
                currentPlaylist = playlists.FirstOrDefault();
                if (currentPlaylist != null)
                {
                    currentSong = currentPlaylist.Tracks.FirstOrDefault();
                }
            }*/
        }

        public List<string> PlayListsAsString()
        {
            List<string> playlists = new List<string>();
            Playlists.ForEach(x => playlists.Add(x.Name));
            return playlists;
        }

        public List<string> LibrariesAsString()
        {
            List<string> libraries = new List<string>();
            Libraries.ForEach(x => libraries.Add(x.Url));
            return libraries;
        }

        #region PlayerControls
        public void Play()
        {
            if (mPlayer.playState != WMPPlayState.wmppsPaused && currentSong != null)
            {
                mPlayer.URL = currentSong.Name;
            }
            MPlayer.controls.play();
        }

        public void Pause()
        {
            mPlayer.controls.pause();
        }
        public void Stop()
        {
            mPlayer.controls.stop();
        }

        public void NextTrack()
        {
            int currentIndex = currentPlaylist.Tracks.IndexOf(currentSong);
            currentSong = currentPlaylist.Tracks.ElementAt(++currentIndex%currentPlaylist.Tracks.Count);
            Play();
        }

        public void PreviousTrack()
        {
            int currentIndex = currentPlaylist.Tracks.IndexOf(currentSong);
            currentSong = currentPlaylist.Tracks.ElementAt(--currentIndex % currentPlaylist.Tracks.Count);
            Play();
        }

        public string VolumeUp()
        {
            if (MPlayer.settings.volume < 100)
            {
                MPlayer.settings.volume = MPlayer.settings.volume + 10;
            }
            return mPlayer.settings.volume.ToString() + "%";
        }

        public string VolumeDown()
        {
            if (MPlayer.settings.volume > 0)
            {
                MPlayer.settings.volume = MPlayer.settings.volume - 10;
            }
            return mPlayer.settings.volume.ToString() + "%";
        }

        #endregion

        #region CurrentSongMethods
        public string GetCurrentPosition()
        {
            return MPlayer.controls.currentPositionString;
        }
        public double GetCurrentPositionDouble()
        {
            return (MPlayer.controls.currentPosition);
        }

        public string GetDuration()
        {
            return mPlayer.controls.currentItem.durationString;
        }
        public double GetDurationDouble()
        {
            return (mPlayer.controls.currentItem.duration);
        }
        #endregion
        public void SetCurrentPlaylist(string newCurrentPlaylist)
        {
            currentPlaylist = Playlists.Find(x => x.Name.Equals(newCurrentPlaylist));
        }

        public void SetCurrentLibrary(int index)
        {
            currentPlaylist = new Playlist(libraries.ElementAt(index).SongsInLibrary);
            currentSong = currentPlaylist.Tracks.FirstOrDefault();
        }

        public void SetCurrentSong(string newCurrentSong)
        {
            currentSong = currentPlaylist.Tracks.Find(x => x.Name.Equals(newCurrentSong));}
    }
}
