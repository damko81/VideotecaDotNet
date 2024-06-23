using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideotecaDotNet_VideotecaDotNetAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedMovieTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CreatedDate", "Description", "Director", "Disc", "Duration", "Genre", "ImageSrc", "Infobar", "Name", "NameFromDisc", "Rating", "ReleaseDate", "Stars", "Storyline", "UpdatedDate", "Url" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test", "Test", "D:", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
