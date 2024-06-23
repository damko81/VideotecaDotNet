using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideotecaDotNet_VideotecaDotNetAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedMovieTableWithCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 6, 23, 20, 44, 51, 541, DateTimeKind.Local).AddTicks(9574), new DateTime(2024, 6, 23, 20, 44, 51, 541, DateTimeKind.Local).AddTicks(9629) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
