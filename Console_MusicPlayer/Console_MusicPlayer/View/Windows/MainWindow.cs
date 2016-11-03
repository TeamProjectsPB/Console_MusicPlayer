using ConsoleDraw;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows;
using ConsoleDraw.Windows.Base;
using System;
using System.IO;
using System.Collections.Generic;
using WMPLib;
using System.Collections;
using System.Linq;
using System.Timers;
using Console_MusicPlayer.Controller;
using Console_MusicPlayer.Model;

namespace Console_MusicPlayer.View.Windows
{


    class MainWindow : FullWindow
    {
        #region Members

        public static MediaPlayerController controller = new MediaPlayerController();
        static Timer timer;
        public static int songId = 0;

        private Label libraryTextBox;
        private Label playlistTextBox;
        private Label musicTextBox;
        private Label controlsLabel;
        private Button stopBtn;
        private Button playBtn;
        private Button pouseBtn;
        private Button nextTrackBtn;
        private Button previousTrackBtn;
        private Button volumeUpBtn;
        private Button volumeDownBtn;
        private Button addNewLibraryBtn;
        private Button addNewPlaylistBtn;
        private Label startLabel;
        private Label endLabel;
        public Label currentSongLabel;

        //private Label artistLabel;
        //private Label nameLabel;
        //private Label albumLabel;
        //private Label rankLabel;

        private Button artistLabelBtn;
        private Button nameLabelBtn;
        private Button albumLabelBtn;
        //private Button rankBtn;

        private Button repeatAllBtn;
        private Button shufflePlayBtn;
        //private Button openWindow;

        private Label volumeLabel;
        private FileBrowser currentPlaylistBrowser;
        private FileBrowser playlistsBrowser;
        private FileBrowser libraryBrowser;

        

        private AddTrackToPlaylistWindow addTrackToPlaylistWindow;
        #endregion

        public MainWindow()
            : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(UpdateCurrentPosition);
            timer.Interval = 1000;

            #region Elementy Interfejsu Inicjalizacja

            currentPlaylistBrowser = new FileBrowser(5, 33, 90, 32, controller.CurrentSongs, "currentPlaylistBrowser", this, true);
            playlistsBrowser = new FileBrowser(22, 3, 26, 11, controller.PlaylistsToString, "playlistsBrowser", this, true);
            libraryBrowser = new FileBrowser(6, 3, 26, 12, controller.LibrariesToString, "libraryBrowser", this, true);

            libraryTextBox = new Label("Biblioteka", 3, 10, "libraryTextBox", this);
            playlistTextBox = new Label("Playlisty", 19, 10, "playlistTextBox", this);
            musicTextBox = new Label("Utwory", 3, 77, "musicTextBox", this);
            currentSongLabel = new Label("Aktualna Piosenka", 41, 55, "currentSong", this);
            currentSongLabel.BackgroundColour = ConsoleColor.DarkGray;

            //openWindow = new Button(1, 33, "Dodaj utwor", "addTrackBtn", this) { Action = delegate () { new AddTrackToPlaylistWindow(openWindow.ParentWindow); } };

            addNewLibraryBtn = new Button(5, 3, "Dodaj biblioteke", "addNewLibraryBtn", this) { Action = delegate () { new AddNewLibraryWindow(addNewLibraryBtn.ParentWindow); } };
            addNewPlaylistBtn = new Button(21, 3, "Dodaj playliste", "addNewPlaylistBtn", this) { Action = delegate () { new AddNewPlaylistWindow(addNewPlaylistBtn.ParentWindow); } };


            //artistLabel = new Label("Artysta", 4, 37, "artistLabel", this);
            //nameLabel = new Label("Nazwa utworu", 4, 55, "nameLabel", this);
            //albumLabel = new Label("Album", 4, 85, "albumLabel", this);
            //rankLabel = new Label("Ocena", 4, 105, "rankLabel", this);

            artistLabelBtn = new Button(4, 37, "Artysta", "artistBtn", this) { Action = delegate () { Sort("Author"); } };
            nameLabelBtn = new Button(4, 55, "Nazwa utworu", "titleBtn", this) { Action = delegate () { Sort("Title"); } };
            albumLabelBtn = new Button(4, 95, "Album", "albumBtn", this) { Action = delegate () { Sort("Album"); } };
            //rankBtn = new Button(4, 105, "Ocena", "rankBtn", this);

            //artistLabel.BackgroundColour = ConsoleColor.DarkGray;
            //albumLabel.BackgroundColour = ConsoleColor.DarkGray;
            //nameLabel.BackgroundColour = ConsoleColor.DarkGray;
            //rankLabel.BackgroundColour = ConsoleColor.DarkGray;

            artistLabelBtn.BackgroundColour = ConsoleColor.DarkGray;
            albumLabelBtn.BackgroundColour = ConsoleColor.DarkGray;
            nameLabelBtn.BackgroundColour = ConsoleColor.DarkGray;
            //rankBtn.BackgroundColour = ConsoleColor.DarkGray;

            controlsLabel = new Label("Sterowanie", 40, 65, "controlLabel", this);

            repeatAllBtn = new Button(44, 13, "Repeat All", "stopBtn", this) { Action = delegate () { ChangeRepeatAllStatement(); } };
            shufflePlayBtn = new Button(44, 3, "Random", "stopBtn", this) { Action = delegate () { ChangeRandomPlayStatement(); } };

            stopBtn = new Button(44, 55, "  ■  ", "stopBtn", this) { Action = delegate () { Stop(); } };
            playBtn = new Button(44, 75, "  >  ", "playBtn", this) { Action = delegate () { Play(); } };
            pouseBtn = new Button(44, 65, "  ||  ", "pouseBtn", this) { Action = delegate () { Pause(); } };

            nextTrackBtn = new Button(44, 85, "  >|  ", "nextTrackBtn", this) { Action = delegate () { NextTrack(); } };

            volumeDownBtn = new Button(44, 110, " - ", "volumeDown", this) { Action = delegate () { VolumeDown(); } };
            volumeUpBtn = new Button(44, 123, " + ", "volumeDown", this) { Action = delegate () { VolumeUp(); } };
            volumeLabel = new Label(controller.CurrentVolumeToString, 44, 117, "volumeLabel", this);
            //volumeLabel.SetText(controller.CurrentVolume());

            previousTrackBtn = new Button(44, 45, "  |<  ", "previousTrackBtn", this) { Action = delegate () { PreviousTrack(); } };

            startLabel = new Label("0:00", 42, 4, "startLabel", this);
            endLabel = new Label("0:00", 42, 123, "endLabel", this);

            #endregion

            DrawUIContainers();


            AddAllInputs();

            CurrentlySelected = playBtn;
            Draw();
            MainLoop();

        }


        #region Adders

        public void CreatePlaylist(string name)
        {
            controller.CreatePlaylist(name);
            ReloadPlaylistsBrowser();
        }
        #endregion
        #region Sort

        public void Sort(string attribute)
        {
            controller.Sort(attribute);
            ReloadCurrentPlaylistBrowser();
        }
        #endregion
        #region MediaPlayerControls

        private void Play()
        {
            controller.Play();
            StartTimer();
        }

        private void Pause()
        {
            controller.Pause();
            StopTimer();
        }

        private void Stop()
        {
            controller.Stop();
            StopTimer();
        }

        private void VolumeUp()
        {
            volumeLabel.SetText("    ");
            volumeLabel.SetText(controller.VolumeUp());
            
        }

        private void VolumeDown()
        {
            volumeLabel.SetText("    ");
            volumeLabel.SetText(controller.VolumeDown());
        }

        private void NextTrack()
        {
            controller.NextTrack();
        }

        private void PreviousTrack()
        {
            controller.PreviousTrack();
        }
        private void ChangeRandomPlayStatement()
        {
            if (controller.ChangeRandomPlayStatement())
            {
                shufflePlayBtn.BackgroundColour = ConsoleColor.DarkYellow;
            }
            else
            {
                shufflePlayBtn.BackgroundColour = ConsoleColor.Gray;
            }
        }
        private void ChangeRepeatAllStatement()
        {
            if (controller.ChangeRepeatAllStatement())
            {
                repeatAllBtn.BackgroundColour = ConsoleColor.DarkYellow;
            }
            else
            {
                repeatAllBtn.BackgroundColour = ConsoleColor.Gray;
            }
        }
        #endregion
        #region Timer
        public void UpdateCurrentPosition(object sender, ElapsedEventArgs elapsedEventArgs)
        {

            currentSongLabel.SetText("                                                                              ");
            currentSongLabel.SetText(controller.CurrentSong);
            startLabel.SetText(controller.CurrentPositionToString);
            endLabel.SetText(controller.DurationToString);
            double start = controller.CurrentPosition;
            double end = controller.Duration;
            UpdateSeekBar(start, end);

        }
        public static void StartTimer()
        {
            if (!timer.Enabled)
            {

                timer.Start();
            }
        }

        public static void StopTimer()
        {
            if (timer.Enabled)
            {
                timer.Stop();

            }
        }
        #endregion
        #region UI_Draw

        public Tuple<bool, string> RunAddToTrackWindow()
        {
            addTrackToPlaylistWindow = new Windows.AddTrackToPlaylistWindow(this);
            return Tuple.Create(addTrackToPlaylistWindow.DialogResult, addTrackToPlaylistWindow.SelectedPlaylist);
        }

        public override void ReDraw()
        {
            //DrawUIContainers();
        }

        public void DrawUIContainers()
        {
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 4, 2, 18, 30); //Biblioteki
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 20, 2, 39, 30); //Playlisty
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 4, 32, 39, 128);//Utwory srodek
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 41, 2, 46, 128);//Sterowanie
            WindowManager.DrawColourBlock(ConsoleColor.Gray, 42, 10, 43, 120);//Seekbar domyslny szary
        }

        public void ResetSeekBar()
        {
            WindowManager.DrawColourBlock(ConsoleColor.Gray, 42, 10, 43, 120);//Seekbar domyslny szary
        }
        public void UpdateSeekBar(double start, double end)
        {
            int durationView = 0;
            try
            {
                durationView = (Int32)((start / end) * 110);
                ResetSeekBar();
                WindowManager.DrawColourBlock(ConsoleColor.Black, 42, 10, 43, 11 + durationView);//Seekbar
            }
            catch (Exception e)
            {
                e.ToString();
            }

        }

        public void AddAllInputs()
        {

            Inputs.Add(libraryTextBox);
            Inputs.Add(musicTextBox);
            Inputs.Add(playlistTextBox);
            Inputs.Add(controlsLabel);

            Inputs.Add(shufflePlayBtn);
            Inputs.Add(repeatAllBtn);
            Inputs.Add(stopBtn);
            Inputs.Add(playBtn);
            Inputs.Add(nextTrackBtn);
            Inputs.Add(previousTrackBtn);
            Inputs.Add(startLabel);
            Inputs.Add(endLabel);
            Inputs.Add(pouseBtn);
            //Inputs.Add(artistLabel);
            //Inputs.Add(albumLabel);
            //Inputs.Add(nameLabel);
            //Inputs.Add(rankLabel);
            Inputs.Add(volumeDownBtn);
            Inputs.Add(volumeLabel);
            Inputs.Add(volumeUpBtn);
            Inputs.Add(addNewLibraryBtn);
            Inputs.Add(addNewPlaylistBtn);

            Inputs.Add(artistLabelBtn);
            Inputs.Add(nameLabelBtn);
            Inputs.Add(albumLabelBtn);
            Inputs.Add(currentSongLabel);
            //Inputs.Add(openWindow);

            Inputs.Add(currentPlaylistBrowser);
            Inputs.Add(playlistsBrowser);
            Inputs.Add(libraryBrowser);


        }

        private void ExitApp(Window parent)
        {

            var exitCheck = new Confirm(parent, "Are you sure you wish to Exit?", "Exit");

            //if (!exitCheck.Result)
            return;

            ProgramInfo.ExitProgram = true;
            Environment.Exit(Environment.ExitCode);
        }



        #endregion
        #region FileBrowserReloaders
        public void ReloadCurrentPlaylistBrowser()
        {
            currentPlaylistBrowser.ResetCursorX();
            currentPlaylistBrowser.CurrentList = controller.CurrentSongs;
            currentPlaylistBrowser.GetFileNames();
            currentPlaylistBrowser.Draw();
        }

        public void ReloadPlaylistsBrowser()
        {
            playlistsBrowser.CurrentList = controller.PlaylistsToString;
            playlistsBrowser.GetFileNames();
            playlistsBrowser.Draw();
        }
        public void ReloadLibraryBrowser()
        {
            libraryBrowser.CurrentList = controller.LibrariesToString;
            libraryBrowser.GetFileNames();
            libraryBrowser.Draw();
        }
        #endregion

    }
}
