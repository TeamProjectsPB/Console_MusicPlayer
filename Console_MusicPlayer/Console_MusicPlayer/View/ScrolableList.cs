using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleDraw;
using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows;
using ConsoleDraw.Windows.Base;
using Console_MusicPlayer.Controller;
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
            (ParentWindow as MainWindow).StopTimer();
            //System.Threading.Thread.Sleep(200);
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
                    //Draw();
                }
            (ParentWindow as MainWindow).StartTimer();
        }

        public override void CursorMoveUp()
        {
            (ParentWindow as MainWindow).StopTimer();
            if (CursorX == 0)
                {
                    ParentWindow.MovetoNextItemUp(Xpostion, Ypostion, Width);
                }
                else
                {
                    CursorX = CursorX - 1;
                    Draw();
                }
            (ParentWindow as MainWindow).StartTimer();
        }

        public override void Draw()
        {
            (ParentWindow as MainWindow).StopTimer();
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
            (ParentWindow as MainWindow).StartTimer();
        }

        
        public override void Enter()
        {
            
            if (CursorX < 0 || CursorX >= CurrentList.Count ? false : !ShowingDrive)
            {
                MediaPlayerController controller = MainWindow.controller;

                if (iD.Equals("currentPlaylistBrowser"))
                {
                    WindowManager.DrawColourBlock(ConsoleColor.Gray, 42, 10, 43, 120);//Seekbar domyslny szary
                    //controller.SetCurrentSong(CurrentList.ElementAt(cursorX));
                    controller.Stop();
                    controller.SetCurrentSong(cursorX);
                    WindowManager.WirteText(CurrentlySelectedFile, 0, 0, this.TextColour, this.BackgroundColour);
                    //(ParentWindow as MainWindow).currentSongLabel.SetText("                                                                              ");
                    //(ParentWindow as MainWindow).currentSongLabel.SetText(controller.GetCurrentSongLabel(cursorX));
                    //(ParentWindow as MainWindow).ReloadCurrentPlaylistBrowser();

                }
                else if (iD.Equals("playlistsBrowser"))
                {
                    controller.Stop();
                    controller.SetCurrentPlaylist(CurrentList.ElementAt(cursorX));
                    //controller.CurrentSong = mediaPlayer.CurrentPlaylist.Tracks.FirstOrDefault();
                    //controller.SetFirstOrDefaultSong();
                    (ParentWindow as MainWindow).ReloadCurrentPlaylistBrowser();
                    (ParentWindow as MainWindow).ReloadPlaylistsBrowser();                   
                }
                else if (iD.Equals("libraryBrowser"))
                {
                    controller.Stop();
                    controller.SetCurrentLibrary(CurrentList.ElementAt(CursorX));
                    (ParentWindow as MainWindow).ReloadCurrentPlaylistBrowser();
                    (ParentWindow as MainWindow).ReloadLibraryBrowser();
                    (ParentWindow as MainWindow).ReloadPlaylistsBrowser();
                }
            }
            else if (SelectFile == null ? false : !ShowingDrive)
            {
                SelectFile();
            }
        }

        public override void BackSpace()
        {
            if (CursorX < 0 || CursorX >= CurrentList.Count ? false : !ShowingDrive)
            {
                MediaPlayerController controller = MainWindow.controller;

                if (iD.Equals("currentPlaylistBrowser"))
                {
                    Confirm confirm = new Confirm("Czy na pewno chcesz usunąć piosenkę?", ParentWindow, ConsoleColor.Gray);
                    if (confirm.ShowDialog() == DialogResult.OK)
                    {
                        controller.RemoveTrack(cursorX);
                        (ParentWindow as MainWindow).ReloadCurrentPlaylistBrowser();
                        ResetCursorX();
                    }
                    
                }
                else if (iD.Equals("playlistsBrowser"))
                {
                    Confirm confirm = new Confirm("Czy na pewno chcesz usunąć playlistę?", ParentWindow, ConsoleColor.Gray);
                    if (confirm.ShowDialog() == DialogResult.OK)
                    {
                        //controller.RemoveTrack(cursorX);
                        if (controller.RemovePlaylist(CurrentList.ElementAt(CursorX)))
                        {
                            (ParentWindow as MainWindow).ReloadCurrentPlaylistBrowser();
                        }
                        (ParentWindow as MainWindow).ReloadPlaylistsBrowser();
                        ResetCursorX();

                    }
                }
                else if (iD.Equals("libraryBrowser"))
                {
                    
                }
            }
            else if (SelectFile == null ? false : !ShowingDrive)
            {
                SelectFile();
            }
        }

        private void GetCurrentlySelectedFileName()
        {
            if (CurrentList.Count > 0 && cursorX >= CurrentList.Count())
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
                //Draw();
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
                //Draw();
            }
        }

        public void ResetCursorX()
        {
            CursorX = 0;
        }
    }
}