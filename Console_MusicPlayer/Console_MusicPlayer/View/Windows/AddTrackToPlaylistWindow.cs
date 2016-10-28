using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDraw.Windows.Base;
using ConsoleDraw.Inputs;
using TagLib.Id3v2;

namespace Console_MusicPlayer.View.Windows
{
    class AddTrackToPlaylistWindow : PopupWindow
    {
        FileBrowser playlistBrowser;
        private Button cancelButton;
        public bool DialogResult { get; private set; }

        public String SelectedPlaylist { get; set; }
        public AddTrackToPlaylistWindow(Window parentWindow)
            : base(
                "Dodaj piosenkę do playlisty", (Console.WindowHeight/2) - 10, (Console.WindowWidth/2) - 5, 45, 45,
                parentWindow)
        {
            DialogResult = false;
            playlistBrowser = new FileBrowser(5, 5, Height - 10, Width - 10,
                MainWindow.controller.GetPlaylists(), "addTrackToPlaylist", parentWindow, true);
            cancelButton = new Button(Height - 5, Width - 5, "Anuluj", "cancelButton", parentWindow)
            {
                Action = delegate() { ExitWindow(); }
            };

            Inputs.Add(playlistBrowser);
            Inputs.Add(cancelButton);

            CurrentlySelected = cancelButton;

            Draw();
            MainLoop();
        }

        public void Apply()
        {
            SelectedPlaylist = playlistBrowser.CurrentlySelectedFile;
            DialogResult = true;
            ExitWindow();
            
        }
    }
}
