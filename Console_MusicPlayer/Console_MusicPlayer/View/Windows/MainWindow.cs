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

        public static MediaPlayerController controller;
        static public bool canScroolList = true;
        Timer timer;

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

        //private Label artistLabel;
        //private Label nameLabel;
        //private Label albumLabel;
        //private Label rankLabel;

        private Button artistLabelBtn;
        private Button nameLabelBtn;
        private Button albumLabelBtn;
        private Button rankBtn;

        private Button repeatAllBtn;
        private Button shufflePlayBtn;

        private Label volumeLabel;
        private FileBrowser currentPlaylistBrowser;
        private FileBrowser playlistsBrowser;
        private FileBrowser libraryBrowser;
        #endregion

        public MainWindow()
            : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {
            controller = new MediaPlayerController();
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(UpdateCurrentPosition);
            timer.Interval = 1000;

            #region Elementy Interfejsu Inicjalizacja

            currentPlaylistBrowser = new FileBrowser(5, 33, 90, 32, controller.GetCurrentSongs(), "currentPlaylistBrowser", this, true);
            playlistsBrowser = new FileBrowser(22, 3, 26, 11, controller.GetPlaylists(), "playlistsBrowser", this, true);
            libraryBrowser = new FileBrowser(6, 3, 26, 12, controller.GetLibraries(), "libraryBrowser", this, true);

            libraryTextBox = new Label("Biblioteka", 3, 10, "libraryTextBox", this);
            playlistTextBox = new Label("Playlisty", 19, 10, "playlistTextBox", this);
            musicTextBox = new Label("Utwory", 3, 77, "musicTextBox", this);

            addNewLibraryBtn = new Button(5, 3, "Dodaj biblioteke", "addNewLibraryBtn", this) { Action = delegate () { new AddNewLibraryWindow(addNewLibraryBtn.ParentWindow); } };
            addNewPlaylistBtn = new Button(21, 3, "Dodaj playliste", "addNewPlaylistBtn", this) { Action = delegate () { new AddNewPlaylistWindow(addNewPlaylistBtn.ParentWindow); } };


            //artistLabel = new Label("Artysta", 4, 37, "artistLabel", this);
            //nameLabel = new Label("Nazwa utworu", 4, 55, "nameLabel", this);
            //albumLabel = new Label("Album", 4, 85, "albumLabel", this);
            //rankLabel = new Label("Ocena", 4, 105, "rankLabel", this);

            artistLabelBtn = new Button(4, 37, "Artysta", "artistBtn", this) {Action = delegate() { SortByArtist(); }};
            nameLabelBtn = new Button(4, 55, "Nazwa utowru", "titleBtn", this) { Action = delegate () { SortByTitle(); } };
            albumLabelBtn = new Button(4, 85, "Album", "albumBtn", this) { Action = delegate () { SortByAlbum(); } };
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

            repeatAllBtn = new Button(44, 13, "Repeat All", "stopBtn", this);
            shufflePlayBtn = new Button(44, 3, "Random", "stopBtn", this);

            stopBtn = new Button(44, 55, "  ■  ", "stopBtn", this) { Action = delegate () { Stop(); } };
            playBtn = new Button(44, 75, "  >  ", "playBtn", this) { Action = delegate () { Play(); } };
            pouseBtn = new Button(44, 65, "  ||  ", "pouseBtn", this) { Action = delegate () { Pause(); } };

            nextTrackBtn = new Button(44, 85, "  >|  ", "nextTrackBtn", this) { Action = delegate () { NextTrack(); } };

            volumeDownBtn = new Button(44, 110, " - ", "volumeDown", this) { Action = delegate () { VolumeDown(); } };
            volumeUpBtn = new Button(44, 123, " + ", "volumeDown", this) { Action = delegate () { VolumeUp(); } };
            volumeLabel = new Label(controller.GetCurrentVolume(), 44, 117, "volumeLabel", this);
            volumeLabel.SetText(controller.GetCurrentVolume());

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
        private void SortByArtist()
        {
            controller.SortByArtist();
            ReloadCurrentPlaylistBrowser();

        }
        
        private void SortByTitle()
        {
            controller.SortByTitle();
            ReloadCurrentPlaylistBrowser();

        }

        private void SortByAlbum()
        {
            controller.SortByAlbum();
            ReloadCurrentPlaylistBrowser();
        }



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
            volumeLabel.SetText(controller.VolumeUp());
        }

        private void VolumeDown()
        {
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
        #endregion
        #region Timer
        private void UpdateCurrentPosition(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            canScroolList = false;
            startLabel.SetText(controller.GetCurrentPosition());
            endLabel.SetText(controller.GetDuration());
            double start = controller.GetCurrentPositionDouble();
            double end = controller.GetDurationDouble();
            UpdateSeekBar(start, end);
            canScroolList = true;
        }
        public void StartTimer()
        {
            if (!timer.Enabled)
            {
                canScroolList = false;
                timer.Start();
            }
        }

        public void StopTimer()
        {
            if (timer.Enabled)
            {
                timer.Stop();
                canScroolList = true;
            }
        }
        #endregion
        #region UI_Draw

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

        public void UpdateSeekBar(double start, double end)
        {
            int durationView = 0;
            try
            {
                durationView = (Int32)((start / end) * 110);
                WindowManager.DrawColourBlock(ConsoleColor.Black, 42, 10, 43, 11 + durationView);//Seekbar                                                                              //Draw();
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
            Inputs.Add(volumeUpBtn);
            Inputs.Add(addNewLibraryBtn);
            Inputs.Add(addNewPlaylistBtn);

            Inputs.Add(artistLabelBtn);
            Inputs.Add(nameLabelBtn);
            Inputs.Add(albumLabelBtn);
            //Inputs.Add(rankBtn);

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
        }



        #endregion

        #region FileBrowserReloaders
        public void ReloadCurrentPlaylistBrowser()
        {
            currentPlaylistBrowser.CurrentList = controller.GetCurrentSongs();
            currentPlaylistBrowser.GetFileNames();
            currentPlaylistBrowser.Draw();
        }

        public void ReloadPlaylistsBrowser()
        {
            playlistsBrowser.CurrentList = controller.GetPlaylists();
            playlistsBrowser.GetFileNames();
            playlistsBrowser.Draw();
        }
        #endregion


    }


}
