using System.Text.RegularExpressions;
using VideotecaDotNet_VideotecaDotNetAPI.Models;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public class BusinessService
    {
        public static List<Movie> LoadMovies(string disc)
        {
            List<Movie> movies = new List<Movie>();
            List<string> directories = ListOfDirectories(disc);

            foreach (string directory in directories)
            {
                Movie movie = new Movie();
                movie.Disc = disc;
                movie.NameFromDisc = directory;
                movie.Name = directory;
              
                movies.Add(movie);
            }

            return movies;
        }

        private static List<string> ListOfDirectories(string disc)
        {
            string movieKey = "";
            List<string> directories = new List<string>();

            DirectoryInfo folder = new DirectoryInfo(disc);
            foreach (DirectoryInfo directory in folder.GetDirectories().Where(dir => !dir.Attributes.HasFlag(FileAttributes.Hidden)))
            {
                movieKey = RemoveSpecialChar(directory.Name);
                directories.Add(movieKey);
            }

            return directories;
        }

        private static string RemoveSpecialChar(string name)
        {
            string nameTmp = name.Replace(".", " ");
            nameTmp = nameTmp.Replace("(", " ");
            nameTmp = nameTmp.Replace(")", " ");
            nameTmp = nameTmp.Replace("[", " ");
            nameTmp = nameTmp.Replace("]", " ");
            nameTmp = nameTmp.Replace("<", " ");
            nameTmp = nameTmp.Replace(">", " ");
            nameTmp = nameTmp.Replace("{", " ");
            nameTmp = nameTmp.Replace("}", " ");

            return nameTmp;
        }
    }
}
