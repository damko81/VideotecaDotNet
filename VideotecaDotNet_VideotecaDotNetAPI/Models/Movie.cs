using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideotecaDotNet_VideotecaDotNetAPI.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Disc { get; set; }
        public string Name { get; set; }
        public string NameFromDisc { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }
        public string Stars { get; set; }
        public string Infobar { get; set; }
        public string Director { get; set; }
        public string Duration { get; set; }
        public string Storyline { get; set; }
        public string ReleaseDate { get; set; }
        public string Url { get; set; }
        public string ImageSrc { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
