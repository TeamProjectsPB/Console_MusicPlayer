using ConsoleDraw;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows;
using ConsoleDraw.Windows.Base;
using System;
using System.IO;
using System.Collections.Generic;
using WMPLib;
using System.Collections;
using Console_MusicPlayer.Model;

namespace Console_MusicPlayer.View.Windows
{
    

    class MainWindow : FullWindow
    {
        static public MediaPlayer p = new MediaPlayer(); 
        List<Button> songs = new List<Button>();


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
        private FileBrowser currentPlaylist;
        private FileBrowser playlistsList;
        private FileBrowser libraryList;




        public MainWindow()
            : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {


            //fileMenu = BulidFileMenu();
            //settingMenu = BuildSettingMenu();
            // helpMenu = BulidHelpMenu();

            #region Przykladowe playlisty 

            List<string> lista = new List<string>();

            lista.Add(@"D:\Muzyka\Hip-Hop PL\KeKe - Trzecie Rzeczy (2016)\KęKę - Trzecie Rzeczy\KęKę - Nie Chcę Umierać.mp3");
            lista.Add(@"D:\Muzyka\Hip-Hop PL\KeKe - Trzecie Rzeczy (2016)\KęKę - Trzecie Rzeczy\KęKę - Smutek.mp3");
            lista.Add(@"D:\Muzyka\Hip-Hop PL\KeKe - Trzecie Rzeczy (2016)\KęKę - Trzecie Rzeczy\KęKę - Arrivederci.mp3");
            lista.Add(@"D:\Muzyka\Hip-Hop PL\KeKe - Trzecie Rzeczy (2016)\KęKę - Trzecie Rzeczy\KęKę - Nie Chcę Umierać.mp3");
            lista.Add(@"D:\Muzyka\Hip-Hop PL\KeKe - Trzecie Rzeczy (2016)\KęKę - Trzecie Rzeczy\KęKę - Nie Chcę Umierać.mp3");


            List <string> listaPlaylist = new List<string>();
            for (int a = 0; a < 10; a++)
            {
                listaPlaylist.Add("playlista " + a);
            }

            List<string> listaBibliotek = new List<string>();
            for (int a = 0; a < 7; a++)
            {
                listaBibliotek.Add("biblioteka " + a);
            }

            #endregion

            #region Elementy Interfejsu Inicjalizacja

            currentPlaylist = new FileBrowser(6, 33, 90, 32, lista, "folderSelect", this, true);
           playlistsList = new FileBrowser(20,3,14,11,listaPlaylist,"playlistsList",this,true);
           libraryList = new FileBrowser(5,3,14,12,listaBibliotek,"libraryList",this,true);

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
            pouseBtn = new Button(44, 65, "  ||  ", "pouseBtn", this) { Action = delegate () { Pouse(); } };

            nextTrackBtn = new Button(44, 85, "  >|  ", "nextTrackBtn", this);

            volumeDownBtn = new Button(44, 110, " - ", "volumeDown", this) { Action = delegate () { VolumeDown(); } };
            volumeUpBtn = new Button(44, 123, " + ", "volumeDown", this) { Action = delegate () { VolumeUp(); } };
            volumeLabel = new Label(p.MPlayer.settings.volume.ToString() + "%", 44, 117, "volumeLabel", this);
            volumeLabel.SetText(p.MPlayer.settings.volume.ToString());

            previousTrackBtn = new Button(44, 45, "  |<  ", "previousTrackBtn", this);

            startLabel = new Label("0:00", 42, 4, "startLabel", this);
            endLabel = new Label("0:00", 42, 123, "endLabel", this);

            #endregion

            DrawUIContainers();
            

            AddAllInputs();

            CurrentlySelected = playBtn;
            Draw();
            MainLoop();
        }



        #region MediaPlayerControls

        public void Play()
        {
            if (p.MPlayer.URL == "")
            {
                p.MPlayer.URL = @"D:\Muzyka\Hip-Hop PL\KeKe - Trzecie Rzeczy (2016)\KęKę - Trzecie Rzeczy\KęKę - Nic Już Nie Muszę.mp3";
            }
            p.MPlayer.controls.play();
            p.MPlayer.settings.volume = 25;


        }

        public void Pouse()
        {
            startLabel.SetText(p.MPlayer.controls.currentPositionString);
            endLabel.SetText(p.MPlayer.controls.currentItem.durationString);
            p.MPlayer.controls.pause();
        }
        public void Stop()
        {
            p.MPlayer.controls.stop();

        }
        public string getCurrentPosition()
        {
            string pos;

            pos = p.MPlayer.controls.currentPositionString.ToString();

            return pos;
        }

        private void VolumeUp()
        {
            if (p.MPlayer.settings.volume < 100)
            {
                p.MPlayer.settings.volume = p.MPlayer.settings.volume + 10;
                volumeLabel.SetText(p.MPlayer.settings.volume.ToString() + "%");
            }
        }

        private void VolumeDown()
        {
            if (p.MPlayer.settings.volume > 0)
            {
               p.MPlayer.settings.volume = p.MPlayer.settings.volume - 10;
                volumeLabel.SetText(p.MPlayer.settings.volume.ToString() + "%");
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
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 4, 2, 18, 30);
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 20, 2, 40, 30);
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 4, 32, 40, 128);
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 41, 2, 45, 128);

            WindowManager.DrawColourBlock(ConsoleColor.Gray, 42, 10, 43, 120);
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

            Inputs.Add(currentPlaylist);
            Inputs.Add(playlistsList);
            Inputs.Add(libraryList);
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
