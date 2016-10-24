﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TagLib;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    public class Playlist
    {
        #region Members
        private string name;
        private List<Song> tracks;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public List<Song> Tracks
        {
            get { return tracks; }
            set { tracks = value; }
        }

        #endregion

        #region Constructors

        public Playlist(string name)
        {
            this.name = name;
            tracks = new List<File>();
        }

        public Playlist(List<Song> tracks)
        {
            name = string.Empty;
            this.tracks = tracks;
        }

        public Playlist(string name, List<Song> tracks)
        {
            this.name = name;
            this.tracks = tracks;
        }

        #endregion

        #region Methods
        public void AddSong(Song song)
        {
            tracks.Add(song);
        }

        public void DeleteSong(Song song)
        {
            tracks.Remove(song);
        }

        public void MoveSong(Song song, int move)
        {
            int songIndex = tracks.IndexOf(song);
            if ((songIndex + move >= 0) && (songIndex + move < tracks.Count))
            {
                tracks.Remove(song);
                tracks.Insert(songIndex + move, song);
            }
        }

        public List<string> GetPlayListAsString()
        {
            List<string> playlist = new List<string>();
            tracks.ForEach(x => playlist.Add(x.Name));
            return playlist;
        } 
        #endregion
    }
}
