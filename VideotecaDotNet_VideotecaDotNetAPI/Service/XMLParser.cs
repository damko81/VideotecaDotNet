using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Xml;
using VideotecaDotNet_VideotecaDotNetAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public static class XMLParser
    {

        public static byte[] CreateXML(List<Movie> movies)
        {
            var sb = new StringBuilder();
            using var xmlWriter = XmlWriter.Create(sb,new XmlWriterSettings { Indent = true });

            string disc = "";
            string nameFromDisc = "";

            xmlWriter.WriteStartElement("filmi");
            int i = 0;
            while (i < movies.Count) {

                Movie f = movies[i];

                if (!f.Disc.ToLower().Equals(disc.ToLower()))
                {
                    disc = f.Disc;
                    xmlWriter.WriteStartElement("disc");
                    xmlWriter.WriteAttributeString("disc", disc);
                }
                if (!f.NameFromDisc.Equals(nameFromDisc))
                {
                    nameFromDisc = f.NameFromDisc;
                    xmlWriter.WriteStartElement("nameFromDisc");
                    xmlWriter.WriteAttributeString("nameFromDisc", nameFromDisc);
                }

                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteAttributeString("name", f.Name);
                    xmlWriter.WriteStartElement("genre");
                    xmlWriter.WriteAttributeString("genre", f.Genre);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("rating");
                    xmlWriter.WriteAttributeString("rating", f.Rating);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("description");
                    xmlWriter.WriteAttributeString("description", f.Description);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("stars");
                    xmlWriter.WriteAttributeString("stars", f.Stars);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("director");
                    xmlWriter.WriteAttributeString("director", f.Director);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("duration");
                    xmlWriter.WriteAttributeString("duration", f.Duration);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("releaseDate");
                    xmlWriter.WriteAttributeString("releaseDate", f.ReleaseDate);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("url");
                    xmlWriter.WriteAttributeString("url", f.Url);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("imageSrcDec");
                    xmlWriter.WriteAttributeString("imageSrcDec", f.ImageSrc);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("imageSrc");
                    xmlWriter.WriteAttributeString("imageSrc", f.ImageSrc); //TODO: To byte
                    xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                ++i;
                //Preveriti za nasledje elemente, ali je potrebno zapreti: 1.node za nameFromDisc ali pa 2. node za disk
                if (i >= movies.Count || !movies[i].NameFromDisc.Equals(nameFromDisc)) { 
                    xmlWriter.WriteEndElement(); // end node element nameFromDisc
                }
                if (i >= movies.Count || !movies[i].Disc.Equals(disc))
                {
                    xmlWriter.WriteEndElement(); // end node element disc
                }
            }
            xmlWriter.WriteEndElement(); // End node element "filmi"
            xmlWriter.Flush();

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public static List<Movie> ReadXML(byte[] buffer)
        {
            List<Movie> movies = new List<Movie>();
            Movie x;

            XmlDocument document = new XmlDocument();
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                document.Load(ms);
            }

            XmlNodeList listOfDiscs = document.GetElementsByTagName("disc");

            int i = 0;
            while (i < listOfDiscs.Count) {

                XmlNode discNode = listOfDiscs.Item(i);
                string disc = discNode.Attributes.GetNamedItem("disc").Value;

                XmlNodeList nameFromDiscNL = discNode.ChildNodes;

                int j = 0;
                while (j < nameFromDiscNL.Count)
                {
                    XmlNode nameFromDiscNode = nameFromDiscNL.Item(j);
                    string nameFromDisc = nameFromDiscNode.Attributes.GetNamedItem("nameFromDisc").Value;

                    XmlNodeList nameNL = nameFromDiscNode.ChildNodes;

                    int z = 0;
                    while (z < nameNL.Count)
                    {
                        XmlNode nameNode = nameNL.Item(z);
                        XmlNodeList nameChildNL = nameNode.ChildNodes;

                        string name = nameNode.Attributes.GetNamedItem("name").Value;
                        string genre = "";
                        string rating = "";
                        string description = "";
                        string stars = "";
                        string director = "";
                        string duration = "";
                        string releaseDate = "";
                        string url = "";
                        string imageSrc = "";

                        int u = 0;
                        while(u < nameChildNL.Count)
                        {
                            XmlNode nameChild = nameChildNL.Item(u);
                            string childAttrib = nameChild.Name;

                            if (childAttrib.Equals("genre"))
                            {
                                genre = nameChild.Attributes.GetNamedItem("genre").Value;
                            }
                            if (childAttrib.Equals("rating"))
                            {
                                rating = nameChild.Attributes.GetNamedItem("rating").Value;
                            }
                            if (childAttrib.Equals("description"))
                            {
                                description = nameChild.Attributes.GetNamedItem("description").Value;
                            }
                            if (childAttrib.Equals("stars"))
                            {
                                stars = nameChild.Attributes.GetNamedItem("stars").Value;
                            }
                            if (childAttrib.Equals("director"))
                            {
                                director = nameChild.Attributes.GetNamedItem("director").Value;
                            }
                            if (childAttrib.Equals("duration"))
                            {
                                duration = nameChild.Attributes.GetNamedItem("duration").Value;
                            }
                            if (childAttrib.Equals("releaseDate"))
                            {
                                releaseDate = nameChild.Attributes.GetNamedItem("releaseDate").Value;
                            }
                            if (childAttrib.Equals("url"))
                            {
                                url = nameChild.Attributes.GetNamedItem("url").Value;
                            }
                            if (childAttrib.Equals("imageSrcDec"))
                            {
                                imageSrc = nameChild.Attributes.GetNamedItem("imageSrcDec").Value;
                            }

                            ++u;
                        }

                        x = new Movie { Disc = disc,
                                        Name = name, 
                                        Genre = genre, 
                                        Rating = rating, 
                                        Description = description, 
                                        Stars = stars, 
                                        Director = director, 
                                        Duration = duration, 
                                        ReleaseDate = releaseDate,
                                        NameFromDisc = nameFromDisc,
                                        Url = url,
                                        ImageSrc = imageSrc
                                      };
                        movies.Add(x);

                        ++z;
                    }

                    ++j;
                }

                ++i;
            }

            return movies;
        }

    }

}
