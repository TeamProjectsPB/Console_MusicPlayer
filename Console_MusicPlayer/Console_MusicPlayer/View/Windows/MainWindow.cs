using ConsoleDraw;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace Console_MusicPlayer.View.Windows
{
    class MainWindow : FullWindow
    {
        WindowsMediaPlayer mPlayer = new WindowsMediaPlayer();
        private Menu fileMenu;
        private Menu settingMenu;
        private Menu helpMenu;
        private Label libraryTextBox;
        private Label playlistTextBox;
        private Label musicTextBox;
        private Label controlsLabel;
        private Button stopBtn;
        private Button playBtn;
        private Button pouseBtn;
        private Button nextTrackBtn;
        private Button previousTrackBtn;
        private Label startLabel;
        private Label endLabel;




        public MainWindow()
            : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {
            

            fileMenu = BulidFileMenu();
            settingMenu = BuildSettingMenu();
            helpMenu = BulidHelpMenu();

            DrawUIContainers();

            libraryTextBox = new Label("Biblioteka", 3, 10, "libraryTextBox", this);
            playlistTextBox = new Label("Playlisty", 19, 10, "playlistTextBox", this);
            musicTextBox = new Label("Utwory", 3, 77, "musicTextBox", this);
            controlsLabel = new Label("Sterowanie", 40, 65, "controlLabel", this);

            stopBtn = new Button(44, 55, "  ■  ", "stopBtn", this){ Action = delegate () { Stop(); } };
            playBtn = new Button(44, 75, "  >  ", "playBtn", this){ Action = delegate() { Play(); } };
            pouseBtn = new Button(44, 65, "  ||  ", "pouseBtn", this) { Action = delegate () { Pouse(); } };

            nextTrackBtn = new Button(44, 85, "  >|  ", "nextTrackBtn", this);
            previousTrackBtn = new Button(44, 45, "  |<  ", "previousTrackBtn", this);

            startLabel = new Label("0:00", 42, 4, "startLabel", this);
            endLabel = new Label("0:00", 42, 123, "endLabel", this);



            AddAllInputs();

            CurrentlySelected = playBtn;
            Draw();
            MainLoop();
        }

        #region MediaPlayerControls

        public void Play()
        {
            if (mPlayer.URL == "")
            {
                mPlayer.URL = @"D:\Muzyka\Hip-Hop PL\KeKe - Trzecie Rzeczy (2016)\KęKę - Trzecie Rzeczy\KęKę - Nic Już Nie Muszę.mp3";
            }
            mPlayer.controls.play();
            

        }

        public void Pouse()
        {
            startLabel.SetText(mPlayer.controls.currentPositionString);
            endLabel.SetText(mPlayer.controls.currentItem.durationString);
            mPlayer.controls.pause();
        }
        public void Stop()
        {
            mPlayer.controls.stop();
        }
        public string getCurrentPosition()
        {
            string pos;

            pos = mPlayer.controls.currentPositionString.ToString();

            return pos;
        }
        #endregion

        #region UI_Draw
        public override void ReDraw()
        {
            WindowManager.DrawColourBlock(ConsoleColor.DarkGray, 4, 2, 18, 30);
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
            Inputs.Add(fileMenu);
            Inputs.Add(settingMenu);
            Inputs.Add(helpMenu);
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
        }

        private Menu BulidFileMenu()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            Menu fileMenu = new Menu("Plik", 1, 2, "fileMenu", this);

            var loadMenuItem = new MenuItem("Otwórz", "fileMenuMenuItemLoad", fileMenu.MenuDropdown);
            loadMenuItem.Action = delegate () { LoadData(loadMenuItem.ParentWindow); };
            menuItems.Add(loadMenuItem);

            var saveMenuItem = new MenuItem("Zapisz", "fileMenuMenuItemSave", fileMenu.MenuDropdown);
            saveMenuItem.Action = delegate () { SaveData(saveMenuItem.ParentWindow); };
            menuItems.Add(saveMenuItem);

            var exitMenuItem = new MenuItem("Zakończ", "fileMenuMenuItemExit", fileMenu.MenuDropdown);
            exitMenuItem.Action = delegate () { ExitApp(saveMenuItem.ParentWindow); };
            menuItems.Add(exitMenuItem);

            fileMenu.MenuItems.AddRange(menuItems);

            return fileMenu;
        }

        private Menu BuildSettingMenu()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            Menu settingMenu = new Menu("Ustawienia", 1, 8, "settingMenu", this);

            var resolutionMenuItem = new MenuItem("Rozdzielczosc", "settingMenuMenuItemResolution", fileMenu.MenuDropdown);
            menuItems.Add(resolutionMenuItem);

            settingMenu.MenuItems.AddRange(menuItems);

            return settingMenu;
        }

        private Menu BulidHelpMenu()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            Menu helpMenu = new Menu("Pomoc", 1, 20, "helpMenu", this);

            var viewHelpMenuItem = new MenuItem("Pomoc", "fileMenuMenuItemViewHelp", fileMenu.MenuDropdown);
            viewHelpMenuItem.Action = delegate () { new Alert("Coming Soon!", viewHelpMenuItem.ParentWindow); };
            menuItems.Add(viewHelpMenuItem);

            var aboutMenuItem = new MenuItem("Kontakt", "fileMenuMenuItemAbout", fileMenu.MenuDropdown);
            aboutMenuItem.Action = delegate () { new Alert("Marcin Kozikowski\tmarcinkozikowski@wp.pl      Marcin Januszewski\tmarcinjanuszewski@wp.pl", viewHelpMenuItem.ParentWindow); };
            menuItems.Add(aboutMenuItem);

            helpMenu.MenuItems.AddRange(menuItems);

            return helpMenu;
        }

        private void ExitApp(Window parent)
        {
            
            var exitCheck = new Confirm(parent, "Are you sure you wish to Exit?", "Exit");

            //if (!exitCheck.Result)
                return;

            ProgramInfo.ExitProgram = true;
        }

        public void LoadData(Window parent)
        {
            var fileTypes = new Dictionary<String, String>() { { "txt", "Text Document" }, { "*", "All Files" } };

            var loadMenu = new OpenMenu(fileTypes, parent);

            if (loadMenu.DataLoaded) //Data Load was successful
            {
                parent.ExitWindow();


                Draw();

                FileInfo.Filename = loadMenu.FileNameLoaded;
                FileInfo.Path = loadMenu.PathOfLoaded;
                FileInfo.HasChanged = false;

                SelectItemByID("textArea");
            }
        }

        public Boolean SaveData(Window parent)
        {
            var saveMenu = new SaveMenu("Dane do zapisania", parent);

            if (saveMenu.FileWasSaved) //Data Save was successful
            {
                parent.ExitWindow();

                Draw();

                FileInfo.Filename = saveMenu.FileSavedAs;
                FileInfo.Path = saveMenu.PathToFile;
                FileInfo.HasChanged = false;

                SelectItemByID("textArea");
            }

            return saveMenu.FileWasSaved;
        }

        #endregion

    }


}
