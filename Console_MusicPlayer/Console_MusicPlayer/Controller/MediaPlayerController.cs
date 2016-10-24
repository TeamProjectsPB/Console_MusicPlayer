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

        public MediaPlayerController()
        {
            player.AddLibrary("D:\\Muzyka");
            player.LoadPlaylists();
            player.SetCurrentLibrary(0);
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
        #endregion
        #region BrowserSetters
        public void SetCurrentPlaylist(string newCurrentPlaylist)
        {
            player.SetCurrentPlaylist(newCurrentPlaylist);
        }

        public void SetCurrentLibrary(int index)
        {
            player.SetCurrentLibrary(index);
        }

        public void SetCurrentSong(string newCurrentSong)
        {
            player.SetCurrentSong(newCurrentSong);
        }

        public void SetFirstOrDefaultSong()
        {
            player.SetFirstOrDefaultSong();
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


    }
}
