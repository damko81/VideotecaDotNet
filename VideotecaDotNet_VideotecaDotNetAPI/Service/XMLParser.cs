using System;
using System.Collections.Generic;
using System.Xml;
using VideotecaDotNet_VideotecaDotNetAPI.Models;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public static class XMLParser
    {
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
