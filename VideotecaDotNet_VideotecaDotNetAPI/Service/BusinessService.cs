using IMDbApiLib;
using VideotecaDotNet_VideotecaDotNetAPI.Models;

namespace VideotecaDotNet_VideotecaDotNetAPI.Service
{
    public class BusinessService
    {
        private static ApiLib _apiLib = new ApiLib("k_igrt8re6");

        public static List<Movie> LoadMovies(string disc, List<string> namesFromDiscDbList)
        {
            List<Movie> movies = new List<Movie>();

            List<string> namesFromDisc = ListOfDirectories(disc);
            
            foreach (string nameFromDisc in namesFromDisc)
            {
                if (!namesFromDiscDbList.Exists(x => x == nameFromDisc))
                {
                    Movie movie = GetMoviesByNameFromDisc(nameFromDisc);
                    movie.Disc = disc;
                    movie.NameFromDisc = nameFromDisc;

                    movies.Add(movie);
                }
            }

            return movies;
        }

        private static Movie GetMoviesByNameFromDisc(string nameFromDisc)
        {
            Movie movie = new Movie();
            var result = _apiLib.SearchMovieAsync(nameFromDisc).Result;

            if (string.IsNullOrEmpty(result.ErrorMessage) && result.Results.Count > 0)
            {
                string imdbId = "";

                if (result.Results.Exists(x => x.Title == nameFromDisc))
                {
                    imdbId = result.Results.First(u => u.Title == nameFromDisc).Id;
                }
                else
                {
                    imdbId = result.Results.First().Id;
                }   

                var titleData = _apiLib.TitleAsync(imdbId).Result;

                movie.Name = titleData.Title;
                movie.Genre = titleData.Genres;
                movie.Rating = titleData.IMDbRating;
                movie.Description = titleData.Keywords;
                movie.Stars = titleData.Stars;
                movie.Director = titleData.Directors;
                movie.Duration = titleData.RuntimeStr;
                movie.ReleaseDate = titleData.ReleaseDate;
                movie.ImageSrc = titleData.Image;
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
