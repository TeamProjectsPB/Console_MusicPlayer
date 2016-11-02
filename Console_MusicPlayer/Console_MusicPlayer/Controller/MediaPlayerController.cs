using System;
using System.Collections.Generic;
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

        public MediaPlayerController()
        {
            sortAsc = false;
            player.MPlayer.settings.volume = 5;
            player.AddLibrary("Mix", "D:\\Muzyka\\Mix");
            player.AddPlaylist("dziendobry");
            player.AddPlaylist("elo5");
            player.AddPlaylist("superplaylista");
            player.SetAllMediaPlaylist();
            //player.SetCurrentLibrary("Mix");
            
        }

        public void CreatePlaylist(string name)
        {
            player.CreatePlaylist(name);
            sortAsc = false;
        }
        public void AddLibrary(string url, string name)
        {
            player.AddLibrary(url, name);
            sortAsc = false;
        }

        #region Getters
        public string GetCurrentVolume()
        {
            return player.GetCurrentVolume();
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
            return player.VolumeUp();
        }

        public string VolumeDown()
        {
            return player.VolumeDown();
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
