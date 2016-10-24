using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDraw.Windows.Base;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows;
using ConsoleDraw;

namespace Console_MusicPlayer.View.Windows
{
    class AddNewPlaylistWindow : PopupWindow
    {
        private Button applyBtn;
        private Button exitBtn;
        private TextBox nameTxtBox;

        public AddNewPlaylistWindow(Window parentWindow)
            : base("Utwórz nową playliste", (Console.WindowHeight/2)-10, (Console.WindowWidth/2)-5, 45, 6, parentWindow)
        {
            var nameLabel = new Label("Nazwa: ", PostionX + 2, PostionY + 2, "nameLabel", parentWindow) { BackgroundColour = ConsoleColor.Gray };
            nameTxtBox = new TextBox(PostionX + 2, PostionY + 10, "", "nameTxtBox", this, 30);

            applyBtn = new Button(PostionX + 4, PostionY + 2, "Zastosuj", "applyBtn", this);
            applyBtn.Action = delegate () { Apply(); };

            exitBtn = new Button(PostionX + 4, PostionY + 13, "Anuluj", "exitBtn", this);
            exitBtn.Action = delegate () { ExitWindow(); };

            Inputs.Add(nameLabel);
            Inputs.Add(nameTxtBox);
            
            Inputs.Add(applyBtn);
            Inputs.Add(exitBtn);

            CurrentlySelected = applyBtn;

            Draw();
            MainLoop();
        }

        public void Apply()
        {
            ExitWindow();
        }
    }
}
