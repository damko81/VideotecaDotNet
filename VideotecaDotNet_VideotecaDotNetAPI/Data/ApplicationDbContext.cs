using Microsoft.EntityFrameworkCore;
using VideotecaDotNet_VideotecaDotNetAPI.Models;

namespace VideotecaDotNet_VideotecaDotNetAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<FilesApi> FilesApi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                   Id = 1,
                   Disc = "D:",
                   Name = "Test",
                   NameFromDisc = "Test",
                   Genre = "Test",
                   Rating = "Test",
                   Description = "Test",
                   Stars = "Test",
                   Infobar = "Test",
                   Director = "Test",
                   Duration = "Test",
                   Storyline = "Test",
                   ReleaseDate = "Test",
                   Url = "Test",
                   ImageSrc = "Test",
                   CreatedDate = DateTime.Now,
                   UpdatedDate = DateTime.Now
                }
             );

            modelBuilder.Entity<Users>().HasData(
               new Users
               {
                   Id = 1,
                   Name = "Test",
                   UserName = "Test",
                   Password = "Test"
               }
            );

            modelBuilder.Entity<FilesApi>().HasData(
             new FilesApi
             {
                 Id = 1,
                 Name = "Test",
                 Path = "Test",
                 Kind = "Test",
                 Description = "Test",
                 Size = 123,
                 Data = new byte[] { 0x20 }
             }
          );
        }
        
    }
}
