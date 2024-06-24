using IMDbApiLib;
using System.Text.RegularExpressions;
using VideotecaDotNet_VideotecaDotNetAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public class BusinessService
    {
        private static ApiLib _apiLib = new ApiLib("k_igrt8re6");

        public static List<Movie> LoadMovies(string disc)
        {
            List<Movie> movies = new List<Movie>();
            List<string> directories = ListOfDirectories(disc);
            
            foreach (string directory in directories)
            {
                Movie movie = GetMoviesByNameFromDisc(directory);
                movie.Disc = disc;
                movie.NameFromDisc = directory;
            
                movies.Add(movie);
            }

            return movies;
        }

        private static Movie GetMoviesByNameFromDisc(string nameFromDisc)
        {
            Movie movie = new Movie();
            var result = _apiLib.SearchMovieAsync(nameFromDisc).Result;

            if (string.IsNullOrEmpty(result.ErrorMessage) && result.Results.Count > 0)
            {
                string imdbId = result.Results.First().Id;
                var titleData = _apiLib.TitleAsync(imdbId).Result;

                string Name = titleData.FullTitle;
                string Genre = titleData.Genres;
                string Rating = titleData.IMDbRating;

                movie.Name = Name;
                movie.Genre = Genre;
                movie.Rating = Rating;
            }

            return movie;
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
