using VideotecaDotNet_VideotecaDotNetAPI.Dto;

namespace VideotecaDotNet_VideotecaDotNetAPI.Data
{
    public static class MovieStore
    {
       public static List<MovieDTO> movieList = new List<MovieDTO>
                                                {
                                                    new MovieDTO{Id=1, Name="Alien2"},
                                                    new MovieDTO{Id=2, Name="Commando"}
                                                };

    }

}
