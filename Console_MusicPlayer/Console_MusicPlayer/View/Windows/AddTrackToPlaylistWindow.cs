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
                "Dodaj piosenkę do playlisty", (Console.WindowHeight/2) - 10, (Console.WindowWidth/2) - 5, 33, 20,
                parentWindow)
        {
            DialogResult = false;
            playlistBrowser = new FileBrowser(PostionX + 2, PostionY + 2, 29, 16,
                MainWindow.controller.GetPlaylists(), "addTrackToPlaylist", parentWindow, true);
            cancelButton = new Button(PostionX + 19, PostionY + 12, "Anuluj", "cancelButton", parentWindow)
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
            SelectedPlaylist = playlistBrowser.CurrentlySelectedFile;
            DialogResult = true;
            ExitWindow();
            
        }
    }
}
