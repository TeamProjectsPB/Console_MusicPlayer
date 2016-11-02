using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDraw.Windows.Base;
using ConsoleDraw.Inputs;
using TagLib.Id3v2;
using Console_MusicPlayer.Controller;

namespace Console_MusicPlayer.View.Windows
{
    class AddTrackToPlaylistWindow : PopupWindow
    {
        Window parentWindow;
        FileBrowser playlistBrowser;
        MediaPlayerController controller;
        private Button cancelButton;
        public bool DialogResult { get; private set; }
        public String SelectedPlaylist { get; set; }

        public AddTrackToPlaylistWindow(Window parentWindow)
            : base("Dodaj piosenkę do playlisty", (Console.WindowHeight/2) - 10, (Console.WindowWidth/2) - 5, 33, 20,parentWindow)
        {
            this.parentWindow = parentWindow;
            controller = MainWindow.controller;

            DialogResult = false;
            playlistBrowser = new FileBrowser(PostionX + 2, PostionY + 2, 29, 16,MainWindow.controller.GetPlaylists(), "addTrackToPlaylist", this, true);
            cancelButton = new Button(PostionX + 18, PostionY + 12, "Anuluj", "cancelButton", this)
            {
                Action = delegate() { ExitWindow(); }
            };

            Inputs.Add(cancelButton);
            Inputs.Add(playlistBrowser);

            CurrentlySelected = cancelButton;

            Draw();
            MainLoop();
        }

        public void Apply()
        {
            //SelectedPlaylist = playlistBrowser.CurrentlySelectedFile;
            DialogResult = true;
            ExitWindow();
        }
    }
}
