using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleDraw;
using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using Console_MusicPlayer.Model;
using Console_MusicPlayer.View.Windows;
using WMPLib;

namespace Console_MusicPlayer.View
{
    internal class FileBrowser : Input
    {
        private string iD;
        private bool AtRoot = false;

        private readonly ConsoleColor BackgroundColour = ConsoleColor.DarkGray;

        public Action ChangeItem;

        private int cursorX;

        private List<string> FileNames = new List<string>();

        public bool IncludeFiles;

        private int Offset;

        private bool Selected;

        private readonly ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private readonly ConsoleColor SelectedTextColour = ConsoleColor.White;

        public Action SelectFile;

        private readonly bool ShowingDrive = false;

        private readonly ConsoleColor TextColour = ConsoleColor.Black;

        public FileBrowser(int x, int y, int width, int height, List<string> playlist, string iD, Window parentWindow,
            bool includeFiles = false) : base(x, y, height, width, parentWindow, iD)
        {
            this.iD = iD;
            CurrentList = playlist;
            CurrentlySelectedFile = "";
            IncludeFiles = includeFiles;
            GetFileNames();
            Selectable = true;
        }

        public string CurrentlySelectedFile { get; private set; }

        public List<string> CurrentList { get; set; }

        private int CursorX
        {
            get { return cursorX; }
            set
            {
                cursorX = value;
                GetCurrentlySelectedFileName();
                SetOffset();
            }
        }

        public override void CursorMoveDown()
        {
            if (CursorX == CurrentList.Count - 1 ? false : !ShowingDrive)
            {
                CursorX = CursorX + 1;
                Draw();
            }
            else if (CursorX == CurrentList.Count - 1 ? true : !ShowingDrive)
            {
                ParentWindow.MovetoNextItemDown(Xpostion, Ypostion, Width);
            }
            else
            {
                CursorX = CursorX + 1;
                Draw();
            }
        }

        public override void CursorMoveUp()
        {
            if (CursorX == 0)
            {
                ParentWindow.MovetoNextItemUp(Xpostion, Ypostion, Width);
            }
            else
            {
                CursorX = CursorX - 1;
                Draw();
            }
        }

        public override void Draw()
        {
            var j = 0;
            WindowManager.DrawColourBlock(BackgroundColour, Xpostion, Ypostion, Xpostion + Height, Ypostion + Width);

            for (j = Offset; j < Math.Min(FileNames.Count, Height + Offset - 1); j++)
            {
                var str1 = FileNames[j].PadRight(Width - 2, ' ').Substring(0, Width - 2);
                if (j != CursorX)
                {
                    WindowManager.WirteText(str1, Xpostion + j - Offset + 1, Ypostion + 1, TextColour, BackgroundColour);
                }
                else if (!Selected)
                {
                    WindowManager.WirteText(str1, Xpostion + j - Offset + 1, Ypostion + 1, SelectedTextColour,
                        BackgroundColour);
                }
                else
                {
                    WindowManager.WirteText(str1, Xpostion + j - Offset + 1, Ypostion + 1, SelectedTextColour,
                        SelectedBackgroundColour);
                }
            }
        }

        

        public override void Enter()
        {
            if (CursorX < 0 || CursorX >= CurrentList.Count ? false : !ShowingDrive)
            {
                MediaPlayer mediaPlayer = MainWindow.player;
                if (iD.Equals("currentPlaylistBrowser"))
                {
                    mediaPlayer.SetCurrentSong(CurrentList.ElementAt(cursorX));
                    WindowManager.WirteText(CurrentlySelectedFile, 0, 0, this.TextColour, this.BackgroundColour);
                    mediaPlayer.Play();
                    (ParentWindow as MainWindow).ReloadCurrentPlaylistBrowser();
                    (ParentWindow as MainWindow).StartTimer();

                }
                else if (iD.Equals("playlistsBrowser"))
                {
                    mediaPlayer.SetCurrentPlaylist(CurrentList.ElementAt(cursorX));
                    mediaPlayer.CurrentSong = mediaPlayer.CurrentPlaylist.Tracks.FirstOrDefault();
                    (ParentWindow as MainWindow).ReloadCurrentPlaylistBrowser();
                    (ParentWindow as MainWindow).ReloadPlaylistsBrowser();                   
                }
            }
            else if (SelectFile == null ? false : !ShowingDrive)
            {
                SelectFile();
            }
        }

        private void GetCurrentlySelectedFileName()
        {
            if (cursorX >= CurrentList.Count())
            {
                CurrentlySelectedFile = FileNames[cursorX - CurrentList.Count];
                if (ChangeItem != null)
                {
                    ChangeItem();
                }
            }
            else if (CurrentlySelectedFile != "")
            {
                CurrentlySelectedFile = "";
                if (ChangeItem != null)
                {
                    ChangeItem();
                }
            }
        }


        public void GetFileNames()
        {
            if (!ShowingDrive)
            {
                try
                {
                    if (IncludeFiles)
                    {
                        FileNames = CurrentList;
                    }
                }
                catch (UnauthorizedAccessException unauthorizedAccessException)
                {
                    throw unauthorizedAccessException;
                }
            }
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();
            }
        }

        private void SetOffset()
        {
            while (CursorX - Offset > Height - 2)
            {
                Offset = Offset + 1;
            }
            while (CursorX - Offset < 0)
            {
                Offset = Offset - 1;
            }
        }

        public override void Unselect()
        {
            if (Selected)
            {
                Selected = false;
                Draw();
            }
        }

        public void CursorXAdder(int adder)
        {
            CursorX = (CursorX + adder)%CurrentList.Count;
            Draw();
        }
    }
}