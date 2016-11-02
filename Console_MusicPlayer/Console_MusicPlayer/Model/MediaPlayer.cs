using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Threading;
using ConsoleDraw.Windows.Base;
using WMPLib;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    internal class MediaPlayer
    {
        #region Members

        private static readonly string[] mediaExtensions =
        {
            ".MP3", ".WAV"
        };


        private static WindowsMediaPlayer mPlayer = new WindowsMediaPlayer();
        private List<string> currentPlaylistSongUrl;

        private bool libraryCurrentlyPlaying;
        private bool randomPlay;
        private bool repeatAll;

        #endregion

        #region Properties

        public WindowsMediaPlayer MPlayer
        {
            get { return mPlayer; }
        }
        public IWMPPlaylist CurrentPlaylist
        {
            get { return mPlayer.currentPlaylist; }
            set { mPlayer.currentPlaylist = value; }
        }
        public static Dictionary<string, Song> SongInfo { get; set; }
        public List<IWMPPlaylist> Playlists { get; set; }
        public List<Library> Libraries { get; set; }
        public bool LibraryCurrentlyPlaying
        {
            get { return libraryCurrentlyPlaying; }
            set
            {
                if (libraryCurrentlyPlaying)
                {
                    mPlayer.playlistCollection.remove(CurrentPlaylist);
                }
                libraryCurrentlyPlaying = value;
            }
        }
        #endregion
        #region  Constructors
        public MediaPlayer()
        {
            SongInfo = new Dictionary<string, Song>();
            LoadMediaInfo();
            currentPlaylistSongUrl = new List<string>();
            Playlists = new List<IWMPPlaylist>();
            Libraries = new List<Library>();
            //SetAllMediaPlaylist();
        }
        ~MediaPlayer()
        {
            var temporaryPlaylists = mPlayer.playlistCollection.getByName("temporaryPlaylist");
            for (int i = 0; i < temporaryPlaylists.count; i++)
            {
                mPlayer.playlistCollection.remove(temporaryPlaylists.Item(i));
            }
            
        }
        #endregion
        #region MediaPlayerControls

        public void Play()
        {
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
            mPlayer.controls.next();
        }

        public void PreviousTrack()
        {
            mPlayer.controls.previous();
        }

        public string VolumeUp()
        {
            if (MPlayer.settings.volume < 100)
            {
                MPlayer.settings.volume = MPlayer.settings.volume + 10;
            }
            return mPlayer.settings.volume + "%";
        }

        public string VolumeDown()
        {
            if (MPlayer.settings.volume > 0)
            {
                MPlayer.settings.volume = MPlayer.settings.volume - 10;
            }
            return mPlayer.settings.volume + "%";
        }

        public bool ChangeRandomPlayStatement()
        {
            randomPlay = !randomPlay;
            mPlayer.settings.setMode("shuffle", randomPlay);
            return randomPlay;
        }

        public bool ChangeRepeatAllStatement()
        {
            repeatAll = !repeatAll;
            mPlayer.settings.setMode("loop", repeatAll);
            return repeatAll;
        }

        #endregion
        #region CurrentSongMethods

        public string GetCurrentPosition()
        {
            if (mPlayer.controls.currentItem != null)
            {
                return MPlayer.controls.currentPositionString;
            }
            return "00:00";
        }

        public double GetCurrentPositionDouble()
        {
            try
            {
                if (mPlayer.controls.currentItem != null)
                {
                    return MPlayer.controls.currentPosition;
                }
                return 0;
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
            return "00:00";
        }

        public double GetDurationDouble()
        {
            if (mPlayer.controls.currentItem != null)
            {
                return mPlayer.controls.currentItem.duration;
            }
            return 0;
        }

        #endregion
        #region SortPlaylist

        public void Sort(string attribute, bool sortAsc)
        {
            if (CurrentPlaylist.getItemInfo("SortAttribute").Equals(attribute))
            {
                CurrentPlaylist.setItemInfo("SortAscending", sortAsc.ToString());
            }
            else
            {
                sortAsc = true;
                CurrentPlaylist.setItemInfo("SortAttribute", attribute);
                CurrentPlaylist.setItemInfo("SortAscending", "true");
            }
            SetCurrentPlaylistSongUrl(CurrentPlaylist);
        }
        
        #endregion
        #region Getters
        public string FormatedViewSong(Song s)
        {
            var spaces = 21;
            var formated = "";
            var artist = "";
            var title = "";
            var album = "";
            var value = "";
            var white_spaces = "";

            try
            {
                if (s.Tag.FirstPerformer.Length < spaces - 1)
                {
                    for (var i = 0; i < spaces - 1 - s.Tag.FirstPerformer.Length; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    artist = s.Tag.FirstPerformer + white_spaces + " ";
                    white_spaces = "";
                }
                else if (s.Tag.FirstPerformer.Length <= 2)
                {
                    for (var i = 0; i < spaces; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    artist = white_spaces;
                    white_spaces = "";
                }
                else
                {
                    artist = s.Tag.FirstPerformer.Substring(0, spaces - 1) + " ";
                }
            }
            catch
            {
                artist = String.Empty;
            }
            try
            {
                if (s.Tag.Title.Length < 39)
                {
                    for (var i = 0; i < 39 - s.Tag.Title.Length; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    title = s.Tag.Title + white_spaces + " ";
                    white_spaces = "";
                }
                else if (s.Tag.Title.Length <= 1)
                {
                    for (var i = 0; i < 40; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    title = white_spaces;
                    white_spaces = "";
                }
                else
                {
                    title = s.Tag.Title.Substring(0, 39) + " ";
                }
            }
            catch
            {
                title = string.Empty;
            }
            try
            {
                if (s.Tag.Album.Length < spaces + 4)
                {
                    for (var i = 0; i < spaces + 4 - s.Tag.Album.Length; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    album = s.Tag.Album + white_spaces + " ";
                    white_spaces = "";
                }
                else if (s.Tag.Album.Length <= 2)
                {
                    for (var i = 0; i < spaces; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    album = white_spaces;
                    white_spaces = "";
                }
                else
                {
                    album = s.Tag.Album.Substring(0, spaces - 1) + " ";
                }
            }
            catch
            {
                album = string.Empty;
            }
            if (String.IsNullOrWhiteSpace(title))
            {
                return Path.GetFileNameWithoutExtension(s.Name);
            }
            else
            {
                return artist + title + album;
            }
        }
        public List<string> GetCurrentSongs()
        {
            var currentSongs = new List<string>();
            currentPlaylistSongUrl.ForEach(x =>
            {
                var song = SongInfo[x];
                currentSongs.Add(FormatedViewSong(song));
            });
            return currentSongs;
        }
        public List<string> GetPlaylists()
        {
            var playlistName = new List<string>();
            Playlists.ForEach(x => playlistName.Add(x.name));
            return playlistName;
        }
        public List<string> GetLibraries()
        {
            var libraryName = new List<string>();
            Libraries.ForEach(x => libraryName.Add(x.Name));
            return libraryName;
        }
        public string GetCurrentVolume()
        {
            return mPlayer.settings.volume + "%";
        }
        public string GetCurrentSongLabel()
        {
            try
            {
                return Path.GetFileNameWithoutExtension(mPlayer.currentMedia.sourceURL);
            }
            catch
            {
                return "Aktualna piosenka";
            }
        }
        #endregion
        #region Setters
        public void SetCurrentPlaylist(string newCurrentPlaylist)
        {
            libraryCurrentlyPlaying = false;
            var temporaryPlaylist = Playlists.Find(x => x.name.Equals(newCurrentPlaylist));
            SetCurrentPlaylistSongUrl(temporaryPlaylist);
            CurrentPlaylist = temporaryPlaylist;
        }
        private void SetCurrentPlaylistSongUrl(IWMPPlaylist playlist)
        {
            currentPlaylistSongUrl.Clear();
            for (int i = 0; i < playlist.count; i++)
            {
                currentPlaylistSongUrl.Add(playlist.Item[i].sourceURL);
            }
        }
        public void SetAllMediaPlaylist()
        {
            var audio = mPlayer.mediaCollection.getByAttribute("MediaType", "audio");
            IWMPPlaylist playlist;
            if (mPlayer.mediaCollection.getByName("temporaryPlaylist").count > 0)
            {
                playlist = mPlayer.mediaCollection.getByName("temporaryPlaylist");
                playlist.clear();
            }
            else
            {
                //playlist = mPlayer.mediaCollection.getByName("temporaryPlaylist");
                playlist = mPlayer.playlistCollection.newPlaylist("temporaryPlaylist");
            }
            for (int i = 0; i < audio.count; i++)
            {
                var audioItem = audio.Item[i];
                Libraries.ForEach(x => { if (audioItem.sourceURL.Contains(x.Url)) { playlist.appendItem(audioItem); } });
            }
            SetCurrentPlaylistSongUrl(playlist);
            CurrentPlaylist = playlist;
        }
        public void SetCurrentLibrary(string newCurrentLibrary)
        {
            libraryCurrentlyPlaying = true;
            var newCurrentLib = Libraries.Find(x => x.Name.Equals(newCurrentLibrary));

            var temporaryPlaylist = mPlayer.playlistCollection.newPlaylist("temporaryPlaylist");
            for (var i = 0; i < mPlayer.mediaCollection.getAll().count; i++)
            {
                var media = mPlayer.mediaCollection.getAll().Item[i];
                var url = media.sourceURL;
                if (media.sourceURL.Contains(newCurrentLib.Url))
                {
                    var count = mPlayer.mediaCollection.getAll().count;
                    temporaryPlaylist.appendItem(media);
                }
            }
            SetCurrentPlaylistSongUrl(temporaryPlaylist);
            CurrentPlaylist = temporaryPlaylist;
        }
        public void SetCurrentSong(int index)
        {
            mPlayer.controls.playItem(mPlayer.currentPlaylist.Item[index]);
        }
        #endregion
        #region Remove
        public void RemoveTrack(int index)
        {
            var media = mPlayer.currentPlaylist.Item[index];
            var count = CurrentPlaylist.count;
            CurrentPlaylist.removeItem(media);
            var countafter = CurrentPlaylist.count;
            SetCurrentPlaylistSongUrl(CurrentPlaylist);
        }
        //return: true - current playlist was removed;
        public bool RemovePlaylist(string name)
        {
            bool removedCurrentPlaylist = CurrentPlaylist.name.Equals(name);
            if (removedCurrentPlaylist)
            {
                SetAllMediaPlaylist();
            }
            var playlists = new List<IWMPPlaylist>();
            for (int i = 0; i < mPlayer.playlistCollection.getByName(name).count; i++)
            {
                playlists.Add(mPlayer.playlistCollection.getByName(name).Item(i));
            }
            playlists.ForEach(x => mPlayer.playlistCollection.remove(x));
            Playlists.Remove(Playlists.Find(x => x.name.Equals(name)));
            return removedCurrentPlaylist;
        }
        #endregion
        #region Library
        public void AddLibrary(string name, string url)
        {
            if (Directory.Exists(url))
            {
                Libraries.Add(new Library(name, url));
                var songsUrl = new List<string>(Directory.EnumerateFiles(url, "*.*", SearchOption.AllDirectories).
                    Where(
                        s => mediaExtensions.Contains(Path.GetExtension(s), StringComparer.OrdinalIgnoreCase)));
                /*var songsUrl = new List<string>(Directory.EnumerateFiles(url, "*.*", SearchOption.AllDirectories).
                    Where(
                        s =>
                            s.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".wav", StringComparison.OrdinalIgnoreCase)));*/
                songsUrl.ForEach(x =>
                {
                    if (!SongInfo.ContainsKey(x))
                    {
                        mPlayer.mediaCollection.add(x);
                        var song = Song.Create(x);
                        SongInfo.Add(x, song);
                    }
                });
            }
        }

        public void AddTrackToPlaylist(int trackIndex, string playlistName)
        {
            var song = mPlayer.currentPlaylist.Item[trackIndex];
            var playlists = mPlayer.playlistCollection.getByName(playlistName);
            for (int i = 0; i < playlists.count; i++)
            {
                mPlayer.playlistCollection.getByName(playlistName).Item(i).appendItem(song);
            }
        }
        public void CreatePlaylist(string name)
        {
            IWMPPlaylist newPlaylist = mPlayer.playlistCollection.newPlaylist(name);
            Playlists.Add(newPlaylist);
        }
        public void AddPlaylist(string name)
        {
            if (mPlayer.playlistCollection.getByName(name).count > 0)
            {
                Playlists.Add(mPlayer.playlistCollection.getByName(name).Item(0));
            }
        }
        private void LoadMediaInfo()
        {
            var mediaCollection = mPlayer.mediaCollection.getByAttribute("MediaType", "audio");
            var count = mediaCollection.count;
            for (var i = 0; i < mediaCollection.count; i++)
            {
                var media = mediaCollection.Item[i];
                var url = media.sourceURL;
                //if (mediaExtensions.Contains(Path.GetExtension(media.sourceURL), StringComparer.OrdinalIgnoreCase))
                //{
                    var mediaInfo = Song.Create(media.sourceURL);
                    SongInfo.Add(media.sourceURL, mediaInfo);
                //}
            }
        }
        #endregion
    }
}