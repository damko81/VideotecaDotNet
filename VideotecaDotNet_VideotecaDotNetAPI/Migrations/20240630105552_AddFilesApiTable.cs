using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideotecaDotNet_VideotecaDotNetAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFilesApiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilesApi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesApi", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FilesApi",
                columns: new[] { "Id", "Description", "Name", "Path", "Size" },
                values: new object[] { 1L, "Test", "Test", "Test", 123L });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 6, 30, 12, 55, 51, 649, DateTimeKind.Local).AddTicks(6142), new DateTime(2024, 6, 30, 12, 55, 51, 649, DateTimeKind.Local).AddTicks(6154) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesApi");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 6, 27, 12, 33, 33, 421, DateTimeKind.Local).AddTicks(6612), new DateTime(2024, 6, 27, 12, 33, 33, 421, DateTimeKind.Local).AddTicks(6615) });
        }
    }
}
