using Console_MusicPlayer.View.Windows;
using ConsoleDraw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_MusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup
            WindowManager.UpdateWindow(130, 45);
            //WindowManager.UpdateWindow(100, 40);

            WindowManager.SetWindowTitle("Console Music Player");

            //Start Program
            new MainWindow();

            //Anything to run before exit
        }
    }
}
