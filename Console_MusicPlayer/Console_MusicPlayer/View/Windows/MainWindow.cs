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
using Console_MusicPlayer.Model;

namespace Console_MusicPlayer.View.Windows
{
    

    class MainWindow : FullWindow
    {
        #region Members

        static public bool canScroolList = true;

        static public MediaPlayer player = new MediaPlayer(); 
        List<Button> songs = new List<Button>();
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
        private Label startLabel;
        private Label endLabel;
        private Label artistLabel;
        private Label nameLabel;
        private Label albumLabel;
        private Label rankLabel;
        private Label volumeLabel;
        private FileBrowser currentPlaylistBrowser;
        private FileBrowser playlistsBrowser;
        private FileBrowser libraryBrowser;

        #endregion

        public MainWindow()
            : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(UpdateCurrentPosition);
            timer.Interval = 600;

            player.Libraries.Add(new Library("D:\\Muzyka"));
            //player.Libraries.Add(new Library("H:\\Marcin\\Music"));
            //player.LoadPlaylists();
            player.SetCurrentLibrary(0);
            //player.CurrentPlaylist = player.Playlists.FirstOrDefault();

            //fileMenu = BulidFileMenu();
            //settingMenu = BuildSettingMenu();
            // helpMenu = BulidHelpMenu();

            #region Elementy Interfejsu Inicjalizacja

            currentPlaylistBrowser = new FileBrowser(5, 33, 90, 32, player.CurrentPlaylist.GetPlayListAsString(), "currentPlaylistBrowser", this, true);
            playlistsBrowser = new FileBrowser(20,3,26,11,player.PlayListsAsString(),"playlistsBrowser",this,true);
            libraryBrowser = new FileBrowser(5,3,26,12,player.LibrariesAsString(),"libraryBrowser",this,true);

            libraryTextBox = new Label("Biblioteka", 3, 10, "libraryTextBox", this);
            playlistTextBox = new Label("Playlisty", 19, 10, "playlistTextBox", this);
            musicTextBox = new Label("Utwory", 3, 77, "musicTextBox", this);
            artistLabel = new Label("Artysta", 4, 37, "artistLabel", this);
            nameLabel = new Label("Nazwa utworu", 4, 55, "nameLabel", this);
            albumLabel = new Label("Album", 4, 85, "albumLabel", this);
            rankLabel = new Label("Ocena", 4, 105, "rankLabel", this);

            artistLabel.BackgroundColour = ConsoleColor.DarkGray;
            albumLabel.BackgroundColour = ConsoleColor.DarkGray;
            nameLabel.BackgroundColour = ConsoleColor.DarkGray;
            rankLabel.BackgroundColour = ConsoleColor.DarkGray;

            controlsLabel = new Label("Sterowanie", 40, 65, "controlLabel", this);

            stopBtn = new Button(44, 55, "  ■  ", "stopBtn", this) { Action = delegate () { Stop(); } };
            playBtn = new Button(44, 75, "  >  ", "playBtn", this) { Action = delegate () { Play(); } };
            pouseBtn = new Button(44, 65, "  ||  ", "pouseBtn", this) { Action = delegate () { Pause(); } };

            nextTrackBtn = new Button(44, 85, "  >|  ", "nextTrackBtn", this) {Action = delegate() { NextTrack(); }};

            volumeDownBtn = new Button(44, 110, " - ", "volumeDown", this) { Action = delegate () { VolumeDown(); } };
            volumeUpBtn = new Button(44, 123, " + ", "volumeDown", this) { Action = delegate () { VolumeUp(); } };
            volumeLabel = new Label(player.MPlayer.settings.volume.ToString() + "%", 44, 117, "volumeLabel", this);
            volumeLabel.SetText(player.MPlayer.settings.volume.ToString());

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


        #region Timer
        private void UpdateCurrentPosition(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            canScroolList = false;
            startLabel.SetText(player.GetCurrentPosition());
            endLabel.SetText(player.GetDuration());
            double start = player.GetCurrentPositionDouble();
            double end = player.GetDurationDouble();
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
        #region FileBrowserReloaders
        public void ReloadCurrentPlaylistBrowser()
        {
            currentPlaylistBrowser.CurrentList = player.CurrentPlaylist.GetPlayListAsString();
            currentPlaylistBrowser.GetFileNames();
            currentPlaylistBrowser.Draw();
        }

        public void ReloadPlaylistsBrowser()
        {
            playlistsBrowser.CurrentList = player.PlayListsAsString();
            playlistsBrowser.GetFileNames();
            playlistsBrowser.Draw();
        }
        #endregion
        #region MediaPlayerControls

        public void Play()
        {
            player.Play();
            StartTimer();
        }

        public void Pause()
        {
            startLabel.SetText(player.GetCurrentPosition());
            endLabel.SetText(player.GetDuration());
            player.Pause();
            StopTimer();
        }
        public void Stop()
        {
            player.Stop();
            StopTimer();
        }

        private void VolumeUp()
        {
            volumeLabel.SetText(player.VolumeUp());

        }

        private void VolumeDown()
        {
            volumeLabel.SetText(player.VolumeDown());
        }

        private void NextTrack()
        {
            player.NextTrack();
            //currentPlaylistBrowser. CursorXAdder(1);
        }

        private void PreviousTrack()
        {
            player.PreviousTrack();
            //currentPlaylistBrowser.CursorXAdder(-1);
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
            int durationView=0;
            try
            {
                durationView = (Int32)((start / end) * 110);
            }
            catch(Exception e)
            {
                e.ToString();
            }
            WindowManager.DrawColourBlock(ConsoleColor.Black, 42, 10, 43, 11+durationView);//Seekbar
            //Draw();
        }

        public void AddAllInputs()
        {

            Inputs.Add(libraryTextBox);
            Inputs.Add(musicTextBox);
            Inputs.Add(playlistTextBox);
            Inputs.Add(controlsLabel);
            Inputs.Add(stopBtn);
            Inputs.Add(playBtn);
            Inputs.Add(nextTrackBtn);
            Inputs.Add(previousTrackBtn);
            Inputs.Add(startLabel);
            Inputs.Add(endLabel);
            Inputs.Add(pouseBtn);
            Inputs.Add(artistLabel);
            Inputs.Add(albumLabel);
            Inputs.Add(nameLabel);
            Inputs.Add(rankLabel);
            Inputs.Add(volumeDownBtn);
            Inputs.Add(volumeUpBtn);

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

    }


}
