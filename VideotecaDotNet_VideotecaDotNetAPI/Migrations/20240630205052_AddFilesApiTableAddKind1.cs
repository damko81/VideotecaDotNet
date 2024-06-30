using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideotecaDotNet_VideotecaDotNetAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFilesApiTableAddKind1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "FilesApi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "FilesApi",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 30, 22, 50, 51, 504, DateTimeKind.Local).AddTicks(52));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 6, 30, 22, 50, 51, 503, DateTimeKind.Local).AddTicks(9829), new DateTime(2024, 6, 30, 22, 50, 51, 503, DateTimeKind.Local).AddTicks(9832) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "FilesApi");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 6, 30, 22, 26, 13, 293, DateTimeKind.Local).AddTicks(4595), new DateTime(2024, 6, 30, 22, 26, 13, 293, DateTimeKind.Local).AddTicks(4598) });
        }
    }
}
