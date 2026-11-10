using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaVerse71.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "MangaList");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "MangaList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "MangaList",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "MangaList",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
