using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_MusicPlayer.Model;

namespace Console_MusicPlayer.Controller
{
    class MediaPlayerController
    {
        static public MediaPlayer player = new MediaPlayer();
        private bool sortAsc;
        string url = Directory.GetCurrentDirectory() + "\\config.dat";
        public MediaPlayerController()
        {
            sortAsc = false;
            if (File.Exists(url))
            {
                SetCurrentVolume(url);
                LoadLibraries(url);
                LoadPlaylists(url);
                SetLibraryMediaPlaylist();
            }
        }

        private static void SetLibraryMediaPlaylist()
        {
            player.SetLibraryMediaPlaylist();
        }

        private void LoadPlaylists(string url)
        {
            var playlists = ConfigFile.GetPlaylists(url);
            playlists.ForEach(x => AddPlaylist(x));
        }

        private void LoadLibraries(string url)
        {
            var libraries = ConfigFile.GetLibraries(url);
            libraries.ForEach(x => AddLibrary(x.Item1, x.Item2));
        }

        public void SetCurrentVolume(string url)
        {
            player.SetCurrentVolume(ConfigFile.GetVolume(url));

        }
        public void CreatePlaylist(string name)
        {
            player.CreatePlaylist(name);
            sortAsc = false;
        }
        public void AddLibrary(string name, string url)
        {
            player.AddLibrary(name, url);
            sortAsc = false;
        }

        public void AddPlaylist(string name)
        {
            player.AddPlaylist(name);
            sortAsc = false;
        }

        #region Getters
        public string GetCurrentVolume()
        {
            return player.CurrentVolume + "%";
        }

        public string GetCurrentPosition()
        {
            return player.GetCurrentPosition();
        }

        public double GetCurrentPositionDouble()
        {
            return player.GetCurrentPositionDouble();
        }

        public string GetDuration()
        {
            return player.GetDuration();
        }

        public double GetDurationDouble()
        {
            return player.GetDurationDouble();
        }

        #endregion;
        #region WMPControl
        public void Play()
        {
            player.Play();
        }

        public void Pause()
        {
            player.Pause();
        }

        public void Stop()
        {
            player.Stop();
        }

        public string VolumeUp()
        {
            int volume = player.VolumeUp();
            ConfigFile.SaveVolume(url, volume);
            return volume + "%";
        }

        public string VolumeDown()
        {
            int volume = player.VolumeDown();
            ConfigFile.SaveVolume(url, volume);
            return volume + "%";
        }

        public void NextTrack()
        {
            player.NextTrack();
        }

        public void PreviousTrack()
        {
            player.PreviousTrack();
        }

        public bool ChangeRandomPlayStatement()
        {
            return player.ChangeRandomPlayStatement();
        }

        public bool ChangeRepeatAllStatement()
        {
            return player.ChangeRepeatAllStatement();
        }
        #endregion
        #region BrowserSetters
        public void SetCurrentPlaylist(string newCurrentPlaylist)
        {
            player.SetCurrentPlaylist(newCurrentPlaylist);
        }

        public void SetCurrentLibrary(string newCurrentLibrary)
        {
            player.SetCurrentLibrary(newCurrentLibrary);
        }

        public void SetCurrentSong(int index)
        {
            player.SetCurrentSong(index);
        }

        public string GetCurrentSongLabel()
        {
            return player.GetCurrentSongLabel();
        }

        #endregion

        #region ReturnLists

        public List<string> GetCurrentSongs()
        {
            return player.GetCurrentSongs();
        } 

        public List<string> GetPlaylists()
        {
            return player.GetPlaylists();
        }

        public List<string> GetLibraries()
        {
            return player.GetLibraries();
        }

        #endregion
        #region SortPlaylist

        public void Sort(string attribute)
        {
            sortAsc = !sortAsc;
            player.Sort(attribute, sortAsc);
        }
        #endregion
        #region Remove

        public void AddTrackToPlaylist(int trackIndex, string playlistName)
        {
            player.AddTrackToPlaylist(trackIndex, playlistName);
        }
        public void RemoveTrack(int index)
        {
            player.RemoveTrack(index);
        }

        public bool RemovePlaylist(string name)
        {
            return player.RemovePlaylist(name);
        }
        #endregion


    }
}
