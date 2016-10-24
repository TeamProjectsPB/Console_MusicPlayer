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

        public List<string> GetSongs()
        {
            List<string> playlist = new List<string>();
            tracks.ForEach(x => playlist.Add(FormatedViewSong(x)));
            return playlist;
        }

        public string FormatedViewSong(Song s)
        {
            int spaces = 21;
            string formated = "";
            string artist = "";
            string title = "";
            string album = "";
            string value = "";
            string white_spaces = "";

            try
            {
                if (s.Tag.FirstPerformer.Length < spaces-1)
                {
                    for (int i = 0; i < spaces - s.Tag.FirstPerformer.Length; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    artist = s.Tag.FirstPerformer + white_spaces;
                    white_spaces = "";
                }
                else if(s.Tag.FirstPerformer.Length<=2)
                {
                    for (int i = 0; i < spaces; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    artist = white_spaces;
                    white_spaces = "";
                }
                else
                {
                    artist = s.Tag.FirstPerformer.Substring(0, spaces);
                }
            }
            catch
            {
                artist = "";
            }
            try
            {
                if (s.Tag.Title.Length < 40)
                {
                    for (int i = 0; i < 40 - s.Tag.Title.Length; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    title = s.Tag.Title + white_spaces;
                    white_spaces = "";
                }
                else if (s.Tag.Title.Length <= 1)
                {
                    for (int i = 0; i <40; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    title = white_spaces;
                    white_spaces = "";
                }
                else
                {
                    title = s.Tag.Title.Substring(0, 40);
                }
            }
            catch
            {
                title = "";
            }
            try
            {
                if (s.Tag.Album.Length < spaces+5)
                {
                    for (int i = 0; i < spaces+5 - s.Tag.Album.Length; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    album = s.Tag.Album + white_spaces;
                    white_spaces = "";
                }
                else if (s.Tag.Album.Length <= 2)
                {
                    for (int i = 0; i < spaces; i++)
                    {
                        white_spaces = white_spaces + " ";
                    }
                    album = white_spaces;
                    white_spaces = "";
                }
                else
                {
                    album = s.Tag.Album.Substring(0, spaces);
                }
            }
            catch
            {
                album = "";
            }
                formated = artist + title + album;
            

            return formated;
        }
        #endregion
    }
}
