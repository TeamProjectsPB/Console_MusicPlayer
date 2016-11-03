using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDraw.Windows.Base;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows;
using ConsoleDraw;
using Console_MusicPlayer.Controller;

namespace Console_MusicPlayer.View.Windows
{
    class AddNewLibraryWindow : PopupWindow
    {
        private Button applyBtn;
        private Button exitBtn;
        private TextBox nameTxtBox;
        private TextBox urlTxtBox;
        MediaPlayerController controller;

        public AddNewLibraryWindow(Window parentWindow)
            : base("Dodaj bibliotekę", (Console.WindowHeight/2)-10, (Console.WindowWidth/2)-5, 45, 9, parentWindow)
        {
            var nameLabel = new Label("Nazwa: ", PostionX + 2, PostionY + 2, "nameLabel", parentWindow) { BackgroundColour = ConsoleColor.Gray };
            nameTxtBox = new TextBox(PostionX + 2, PostionY + 10, "", "nameTxtBox", this, 30);

            var urlLabel = new Label("Ścieżka", PostionX + 4, PostionY + 2, "widthLabel", parentWindow) { BackgroundColour = ConsoleColor.Gray };
            urlTxtBox = new TextBox(PostionX + 4, PostionY + 10, "", "heightTxtBox", this, 30);

            applyBtn = new Button(PostionX + 6, PostionY + 2, "Zastosuj", "applyBtn", this);
            applyBtn.Action = delegate () { Apply(); };

            exitBtn = new Button(PostionX + 6, PostionY + 13, "Anuluj", "exitBtn", this);
            exitBtn.Action = delegate () { ExitWindow(); };

            Inputs.Add(nameLabel);
            Inputs.Add(nameTxtBox);
            Inputs.Add(urlLabel);
            Inputs.Add(urlTxtBox);
            Inputs.Add(applyBtn);
            Inputs.Add(exitBtn);

            CurrentlySelected = applyBtn;

            Draw();
            MainLoop();
        }

        public void Apply()
        {
            string name;
            string url;
            name = nameTxtBox.GetText();
            url = urlTxtBox.GetText();
            controller = MainWindow.controller;
            if (!urlTxtBox.GetText().Equals("") && !nameTxtBox.GetText().Equals(""))
            {
                controller.CreateLibrary(name, url);
            }
            ExitWindow();
            (ParentWindow as MainWindow).ReloadLibraryBrowser();
        }
    }
}
