using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song = TagLib.File;

namespace Console_MusicPlayer.Model
{
    public static class PlayListEditor
    {
        public static List<string> ReadPlayListFile(string xmlUrl)
        {
            List<string> songUrls = new List<string>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlUrl);

            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("song");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Name.Equals("song"))
                {
                    songUrls.Add(xmlNode.InnerText);
                }
            }
            return songUrls;
        }

        public static void SavePlayListToNewFile(Playlist playlist)
        {
            XmlTextWriter xmlTextWriter = new XmlTextWriter(Directory.GetCurrentDirectory() + "\\playlists\\" +  playlist.Name + ".playlist", null);
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteWhitespace("\n");
            {
                xmlTextWriter.WriteStartElement("playlist");
                {
                    foreach (Song song in playlist.Tracks)
                    {
                        xmlTextWriter.WriteWhitespace("\n\t");
                        // TODO sony name chyba nie zawiera pelnej sciezki do pliku a o to nam chodzilo zeby bylo zapisywane
                        xmlTextWriter.WriteElementString("song", song.Name);
                    }
                }
                xmlTextWriter.WriteWhitespace("\n");
                xmlTextWriter.WriteEndElement();
            }
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Close();
        }
    }
}
