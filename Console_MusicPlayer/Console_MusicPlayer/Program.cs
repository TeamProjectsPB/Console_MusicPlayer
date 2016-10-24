using Console_MusicPlayer.View.Windows;
using ConsoleDraw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_MusicPlayer.Model;

namespace Console_MusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Library lib = new Library("d:\\music");
            //lib.GetSongs();
            //lib.SongsInLibrary.ForEach(x => Console.WriteLine(x.Name+ " " + x.Tag.Title + "\n"));
            //Console.ReadKey();
            
            //Setup
            WindowManager.UpdateWindow(130, 48);
            //WindowManager.UpdateWindow(100, 40);

            WindowManager.SetWindowTitle("Console Music Player");

            //Start Program
            new MainWindow();

            //Anything to run before exit
            
        }
    }
}
