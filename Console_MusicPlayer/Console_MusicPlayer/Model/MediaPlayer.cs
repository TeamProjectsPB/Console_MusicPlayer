using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Console_MusicPlayer.Controller;
using Console_MusicPlayer.View.Windows;
using WMPLib;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    class MediaPlayer
    {
        #region Members
        private static WindowsMediaPlayer mPlayer;
        private Song currentSong;
        private Playlist currentPlaylist;
        private List<Library> libraries;
        private List<Playlist> playlists;
        #endregion

        #region Properties

        public WindowsMediaPlayer MPlayer
        {
            //get { if (mPlayer == null) { return mPlayer = new WindowsMediaPlayer(); } else return mPlayer; }
            get { return mPlayer; }
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
                if (MediaPlayerController.RandomPlay)
                {
                    NextRandomTrack();
                }
                else
                {
                    NextTrack();
                }
            }
            else if ((WMPPlayState) newState == WMPPlayState.wmppsReady)
            {
                mPlayer.controls.play();
            }
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
        #region Getters
        public List<string> GetCurrentSongs()
        {
            if (currentPlaylist != null)
            {
                return currentPlaylist.GetSongs();
            }
            else
            {
                return new List<string>();
            }
        } 
        public List<string> GetPlaylists()
        {
            List<string> playlists = new List<string>();
            Playlists.ForEach(x => playlists.Add(x.Name));
            return playlists;
        }

        public List<string> GetLibraries()
        {
            List<string> libraries = new List<string>();
            Libraries.ForEach(x => libraries.Add(x.Url));
            return libraries;
        }

        public string GetCurrentVolume()
        {
            return mPlayer.settings.volume.ToString() + "%";
        }
        #endregion
        #region PlayerControls
        public void Play()
        {
            if (mPlayer.playState != WMPPlayState.wmppsPaused && currentSong != null)
            {
                mPlayer.URL = currentSong.Name;
            }
            mPlayer.controls.play();
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
            int nextIndex = currentPlaylist.Tracks.IndexOf(currentSong) + 1;
            if (nextIndex < currentPlaylist.Tracks.Count)
            {
                currentSong = currentPlaylist.Tracks.ElementAt(nextIndex);
                Play();
            }
            else if (MediaPlayerController.RepeatAll)
            {
                currentSong = currentPlaylist.Tracks.ElementAt(nextIndex % currentPlaylist.Tracks.Count);
                Play();
            }
        }

        public void NextRandomTrack()
        {
            Random random = new Random();
            currentSong = currentPlaylist.Tracks.ElementAt(random.Next(0, currentPlaylist.Tracks.Count - 1));
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
            if (mPlayer.controls.currentItem != null)
            {
                return MPlayer.controls.currentPositionString;
            }
            else return "00:00";
        }
        public double GetCurrentPositionDouble()
        {
            try
            {
                if (mPlayer.controls.currentItem != null)
                {
                    return (MPlayer.controls.currentPosition);
                }
                else return 0;
            }
            catch
            {
                return 0;
            }
        }

        public string GetDuration()
        {
            if (mPlayer.controls.currentItem != null)
            {
                return mPlayer.controls.currentItem.durationString;
            }
            else return "00:00";

        }
        public double GetDurationDouble()
        {
            if (mPlayer.controls.currentItem != null)
            {
                return (mPlayer.controls.currentItem.duration);
            }
            else return 0;
        }
        #endregion
        #region Setters
        public void SetCurrentPlaylist(string newCurrentPlaylist)
        {
            currentPlaylist = Playlists.Find(x => x.Name.Equals(newCurrentPlaylist));
        }

        public void SetCurrentLibrary(int index)
        {
            if (libraries.Count > index)
            {
                currentPlaylist = new Playlist(libraries.ElementAt(index).SongsInLibrary);
                currentSong = currentPlaylist.Tracks.FirstOrDefault();
            }
        }

        public void SetCurrentSong(string newCurrentSong)
        {
            currentSong = currentPlaylist.Tracks.Find(x => x.Name.Equals(newCurrentSong));
        }

        public void SetCurrentSong(int index)
        {
            currentSong = currentPlaylist.Tracks.ElementAt(index);
        }

        public string GetCurrentSongLabel()
        {
            return System.IO.Path.GetFileNameWithoutExtension(currentSong.Name);
        }
        public void SetFirstOrDefaultSong()
        {
            currentSong = currentPlaylist.Tracks.FirstOrDefault();
        }
        #endregion
        #region SortPlaylist
        public void SortByArtist(bool sortDesc)
        {
            currentPlaylist.Tracks.Sort((x, y) => string.Compare(x.Tag.FirstPerformer, y.Tag.FirstPerformer));
            if (sortDesc)
            {
                currentPlaylist.Tracks.Reverse();
            }
        }

        public void SortByTitle(bool sortDesc)
        {
            currentPlaylist.Tracks.Sort((x, y) => string.Compare(x.Tag.Title, y.Tag.Title));
            if (sortDesc)
            {
                currentPlaylist.Tracks.Reverse();
            }
        }

        public void SortByAlbum(bool sortDesc)
        {
            currentPlaylist.Tracks.Sort((x, y) => string.Compare(x.Tag.Album, y.Tag.Album));
            if (sortDesc)
            {
                currentPlaylist.Tracks.Reverse();
            }
        }

        #endregion
    }
}
