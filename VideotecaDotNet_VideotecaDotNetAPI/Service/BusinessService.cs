using System.Text.RegularExpressions;
using VideotecaDotNet_VideotecaDotNetAPI.Models;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public class BusinessService
    {
        public static List<Movie> LoadMovies(string disc)
        {
            List<Movie> movies = new List<Movie>();
            Dictionary<string, string> directories = ListOfDirectories(disc);

            foreach (var directory in directories)
            {
                Movie movie = new Movie();
                movie.Disc = disc;
                movie.NameFromDisc = directory.Key;
                movie.Name = directory.Value;
              
                movies.Add(movie);
            }

            return movies;
        }

        private static Dictionary<string, string> ListOfDirectories(string disc)
        {
            string movieKey = "";
            Dictionary<string,string> directories = new Dictionary<string,string>();

            DirectoryInfo folder = new DirectoryInfo(disc);
            foreach (DirectoryInfo directory in folder.GetDirectories())
            {
                movieKey = RemoveSpecialChar(directory.Name);
                directories.Add(movieKey, directory.Name);
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
