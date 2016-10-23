using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleDraw;
using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows;
using ConsoleDraw.Windows.Base;
using System.IO;
using System.Collections;
using Console_MusicPlayer.Model;
using WMPLib;
using Console_MusicPlayer.View.Windows;

namespace Console_MusicPlayer.View
{
    class FileBrowser : Input
    {

            private List<string> FileNames = new List<string>();

            public bool IncludeFiles;

            private ConsoleColor BackgroundColour = ConsoleColor.DarkGray;

            private ConsoleColor TextColour = ConsoleColor.Black;

            private ConsoleColor SelectedTextColour = ConsoleColor.White;

            private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

            private int cursorX;

            private int Offset = 0;

            private bool Selected = false;

            private bool AtRoot = false;

            private bool ShowingDrive = false;

            public Action ChangeItem;

            public Action SelectFile;

            public string CurrentlySelectedFile
            {
                get;
                private set;
            }

            public List<string> CurrentList
            {
                get;
                private set;
            }

            private int CursorX
            {
                get
                {
                    return this.cursorX;
                }
                set
                {
                    this.cursorX = value;
                    this.GetCurrentlySelectedFileName();
                    this.SetOffset();
                }
            }

            public FileBrowser(int x, int y, int width, int height, List<string> playlist, string iD, Window parentWindow, bool includeFiles = false) : base(x, y, height, width, parentWindow, iD)
            {
                this.CurrentList = playlist;
                this.CurrentlySelectedFile = "";
                this.IncludeFiles = includeFiles;
                this.GetFileNames();
                base.Selectable = true;
            }

            public override void CursorMoveDown()
            {
            if ((this.CursorX == this.CurrentList.Count - 1 ? false : !this.ShowingDrive))
            {
                this.CursorX = this.CursorX + 1;
                this.Draw();
            }
            else if ((this.CursorX == this.CurrentList.Count - 1 ? true : !this.ShowingDrive))
                {
                    this.ParentWindow.MovetoNextItemDown(this.Xpostion, this.Ypostion, this.Width);
                }
                else
                {
                    this.CursorX = this.CursorX + 1;
                    this.Draw();
                }
            }

            public override void CursorMoveUp()
            {
                if (this.CursorX == 0)
                {
                    this.ParentWindow.MovetoNextItemUp(this.Xpostion, this.Ypostion, this.Width);
                }
                else
                {
                    this.CursorX = this.CursorX - 1;
                    this.Draw();
                }
            }

            public override void Draw()
            {
                int j=0;
                WindowManager.DrawColourBlock(this.BackgroundColour, this.Xpostion, this.Ypostion, this.Xpostion + this.Height, this.Ypostion + this.Width);

            for (j = this.Offset; j < Math.Min(this.FileNames.Count, this.Height + this.Offset - 1); j++)
            {
                string str1 = this.FileNames[j].PadRight(this.Width - 2, ' ').Substring(0, this.Width - 2);
                if (j != this.CursorX)
                {
                    WindowManager.WirteText(str1, this.Xpostion + j - this.Offset + 1, this.Ypostion + 1, this.TextColour, this.BackgroundColour);
                }
                else if (!this.Selected)
                {
                    WindowManager.WirteText(str1, this.Xpostion + j - this.Offset + 1, this.Ypostion + 1, this.SelectedTextColour, this.BackgroundColour);
                }
                else
                {
                    WindowManager.WirteText(str1, this.Xpostion + j - this.Offset + 1, this.Ypostion + 1, this.SelectedTextColour, this.SelectedBackgroundColour);
                }
            }


        }


        public override void Enter()
            {
                if ((this.CursorX < 1 || this.CursorX >= this.CurrentList.Count ? false : !this.ShowingDrive))
                {
                //this.GoIntoFolder();
                WindowsMediaPlayer pa = MainWindow.p.MPlayer;
                pa.controls.stop();
                WindowManager.WirteText(CurrentlySelectedFile, 0, 0, this.TextColour, this.BackgroundColour);
                
                pa.URL = FileNames[cursorX];
                pa.controls.play();
            }
                else if ((this.SelectFile == null ? false : !this.ShowingDrive))
                {
                    this.SelectFile();
                
                }
            }

            private void GetCurrentlySelectedFileName()
            {
                if (this.cursorX >= this.CurrentList.Count<string>())
                {
                    this.CurrentlySelectedFile = this.FileNames[this.cursorX - this.CurrentList.Count];
                    if (this.ChangeItem != null)
                    {
                        this.ChangeItem();
                    }
                }
                else if (this.CurrentlySelectedFile != "")
                {
                    this.CurrentlySelectedFile = "";
                    if (this.ChangeItem != null)
                    {
                        this.ChangeItem();
                    }
                }
            }

            public void GetFileNames()
            {
                if (!this.ShowingDrive)
                {
                    try
                    {
                        if (this.IncludeFiles)
                        {
                        this.FileNames = CurrentList;
                                
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
                if (!this.Selected)
                {
                    this.Selected = true;
                    this.Draw();
                }
            }

            private void SetOffset()
            {
                while (this.CursorX - this.Offset > this.Height - 2)
                {
                    this.Offset = this.Offset + 1;
                }
                while (this.CursorX - this.Offset < 0)
                {
                    this.Offset = this.Offset - 1;
                }
            }

            public override void Unselect()
            {
                if (this.Selected)
                {
                    this.Selected = false;
                    this.Draw();
                }
            }
        }
    }
