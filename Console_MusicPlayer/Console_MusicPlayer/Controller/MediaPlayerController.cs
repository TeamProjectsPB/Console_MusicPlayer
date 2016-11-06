using System.Collections.Generic;
using System.IO;
using Console_MusicPlayer.Model;
using Song = TagLib.File;

namespace Console_MusicPlayer.Controller
{
    internal class MediaPlayerController
    {
        public MediaPlayerController()
        {
            if (File.Exists(fileUrl))
            {
                LoadCurrentVolume(fileUrl);
                LoadLibraries(fileUrl);
                LoadPlaylists(fileUrl);
                LoadLibraryMediaPlaylist();
            }
            else { ConfigFile.CreateNewFile(fileUrl);}
        }

        public void LoadLibraryMediaPlaylist()
        {
            player.LoadLibraryMediaPlaylist();
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

        public void LoadCurrentVolume(string url)
        {
            player.CurrentVolume = ConfigFile.GetVolume(url);
        }

        public void CreatePlaylist(string name)
        {
            ConfigFile.SaveNewPlaylist(fileUrl, name);
            player.CreatePlaylist(name);
        }

        public void CreateLibrary(string name, string url)
        {
            ConfigFile.SaveNewLibrary(fileUrl, name, url);
            AddLibrary(name, url);
        }
        public void AddLibrary(string name, string url)
        {
            player.AddLibrary(name, url);
        }

        public void AddPlaylist(string name)
        {
            player.AddPlaylist(name);
        }
        public void AddTrackToPlaylist(int trackIndex, string playlistName)
        {
            player.AddTrackToPlaylist(trackIndex, playlistName);
        }
        #region SortPlaylist
        public void Sort(string attribute)
        {
            player.Sort(attribute);
        }
        #endregion

        #region Members
        public static MediaPlayer player = new MediaPlayer();        
        private readonly string fileUrl = Directory.GetCurrentDirectory() + "\\config.dat";
        #endregion
        #region Properties
        public string CurrentVolumeToString
        {
            get { return player.CurrentVolume + "%"; }
        }
        public string CurrentPositionToString
        {
            get { return player.CurrentPositionToString; }
        }
        public double CurrentPosition
        {
            get { return player.CurrentPosition; }
        }
        public string DurationToString
        {
            get { return player.DurationToString; }
        }
        public double Duration
        {
            get { return player.Duration; }
        }
        public List<Song> CurrentSongs
        {
            get { return player.CurrentSongs; }
        }

        public List<string> CurrentSongsToString
        {
            get { return player.CurrentSongsToString; }
        }
        public List<string> PlaylistsToString
        {
            get { return player.PlaylistsToString; }
        }
        public List<string> LibrariesToString
        {
            get { return player.LibrariesToString; }
        }
        public string CurrentSong
        {
            get { return player.CurrentSong; }
        }
        public int SongId { get; set; }
        #endregion
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
            var volume = player.VolumeUp();
            ConfigFile.SaveVolume(fileUrl, volume);
            return volume + "%";
        }
        public string VolumeDown()
        {
            var volume = player.VolumeDown();
            ConfigFile.SaveVolume(fileUrl, volume);
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

        public void LoadCurrentPlaylist(string newCurrentPlaylist)
        {
            player.LoadCurrentPlaylist(newCurrentPlaylist);
        }

        public void LoadCurrentLibrary(string newCurrentLibrary)
        {
            player.LoadCurrentLibrary(newCurrentLibrary);
        }

        public void LoadCurrentSong(int index)
        {
            player.LoadCurrentSong(index);
        }

        #region Remove

        public void RemoveTrack(int index)
        {
            player.RemoveTrack(index);
        }

        public bool RemovePlaylist(string name)
        {
            bool returnedValue = player.RemovePlaylist(name);
            ConfigFile.RemovePlaylist(fileUrl, name);
            return returnedValue;
        }

        public bool RemoveLibrary(string name)
        {
            bool returnedValue = player.RemoveLibrary(name);
            ConfigFile.RemoveLibrary(fileUrl, name);
            return returnedValue;
        }

      

        #endregion
    }
}