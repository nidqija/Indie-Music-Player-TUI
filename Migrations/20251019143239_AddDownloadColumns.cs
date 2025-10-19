using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlayer.Migrations
{
    /// <inheritdoc />
    public partial class AddDownloadColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "downloadfilePath",
                table: "Songs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "hasDownloaded",
                table: "Songs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "downloadfilePath",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "hasDownloaded",
                table: "Songs");
        }
    }
}
