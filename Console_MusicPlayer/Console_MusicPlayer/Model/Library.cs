using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    class Library
    {
        #region Members
        private string name;
        private string url;
        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

       

        #endregion
        #region Constructor

        public Library(string name, string url)
        {
            this.name = name;
            this.url = url;
        }

        #endregion


    }
}
