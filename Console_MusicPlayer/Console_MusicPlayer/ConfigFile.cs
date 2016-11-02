﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Console_MusicPlayer.Controller;
using Console_MusicPlayer.Model;
using WMPLib;

namespace Console_MusicPlayer
{
    public static class ConfigFile
    {
        #region Getters
        public static int GetVolume(string url)
        {
            int volume = 50;
            if (File.Exists(url))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(url);
                    XmlNodeList xmlNodeList = document.GetElementsByTagName("head");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        var xmlNodeList2 = xmlNode.ChildNodes;
                        foreach (XmlNode xmlNode2 in xmlNodeList2)
                        {
                            if (xmlNode2.Name.Equals("volume"))
                            {
                                int.TryParse(xmlNode2.InnerText, out volume);
                            }
                        }
                    }
                }
                catch{ }
            }
            return volume;
        }

        public static List<Tuple<string, string>> GetLibraries(string url)
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
            //List<string> libraries = new List<string>();
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(url);

                XmlNodeList xmlNodeList = document.GetElementsByTagName("libraries");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    var xmlNodeList2 = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in xmlNodeList2)
                    {
                        if (xmlNode2.Name.Equals("library"))
                        {
                            if (xmlNode2.Attributes["name"] != null && xmlNode2.Attributes["url"] != null)
                            {
                                var name = xmlNode2.Attributes["name"].Value;
                                var libUrl = xmlNode2.Attributes["url"].Value;
                                tuples.Add(Tuple.Create(name, libUrl));
                            }
                        }
                    }
                }
            }
            catch { }
            return tuples;
        }
        public static List<string> GetPlaylists(string url)
        {
            List<string> libraries = new List<string>();
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(url);

                XmlNodeList xmlNodeList = document.GetElementsByTagName("playlists");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    var xmlNodeList2 = xmlNode.ChildNodes;
                    foreach (XmlNode xmlNode2 in xmlNodeList2)
                    {
                        if (xmlNode2.Name.Equals("playlist"))
                        {
                            libraries.Add(xmlNode2.InnerText);
                        }
                    }
                }
            }
            catch { }
            return libraries;
        }
        #endregion

        #region Save

        public static void SaveVolume(string url, int volume)
        {
            //XDocument xDocument = XDocument.Load(url);
            //XElement root = xDocument.Element("player");
            //IEnumerable<XElement> rows = root.Descendants("head");
            if (File.Exists(url))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(url);
                    XmlNodeList xmlNodeList = document.GetElementsByTagName("head");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        var xmlNodeList2 = xmlNode.ChildNodes;
                        bool exists = false;
                        foreach (XmlNode xmlNode2 in xmlNodeList2)
                        {
                            if (xmlNode2.Name.Equals("volume"))
                            {
                                xmlNode2.InnerText = volume.ToString();
                                exists = true;
                            }
                        }
                        if (!exists)
                        {
                            XmlNode volumeNode = document.CreateNode(XmlNodeType.Element, "volume", "");
                            volumeNode.InnerText = volume.ToString();
                            xmlNode.AppendChild(volumeNode);
                        }
                    }
                    document.Save(url);
                }
                catch { }
            }
        }
        #endregion

        private static void SaveNewFile(string url, int volume, List<string> playlists, List<Library> libraries)
        {
            XmlTextWriter writer = new XmlTextWriter(url, null);
            writer.WriteStartDocument();
            writer.WriteWhitespace("\n");
            {
                writer.WriteStartElement("player");
                {
                    writer.WriteWhitespace("\n\t");
                    writer.WriteStartElement("head");
                    {
                        writer.WriteWhitespace("\n\t\t");
                        writer.WriteAttributeString("volume", volume.ToString());
                    }
                    writer.WriteEndElement();
                    writer.WriteWhitespace("\n\t");
                    writer.WriteStartElement("playlists");
                    {
                        playlists.ForEach(x =>
                        {
                            writer.WriteWhitespace("\n\t\t");
                            writer.WriteElementString("playlist", x);
                        });
                    }
                    writer.WriteEndElement();
                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteStartElement("libraries");
                    {
                        libraries.ForEach(x =>
                        {
                            writer.WriteWhitespace("\n\t\t");
                            writer.WriteAttributeString("name", x.Name);
                            writer.WriteAttributeString("url", x.Url);
                        });
                    }
                    writer.WriteEndElement();
                }
            } 
        }

        
    }
}
