using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSongIdfromPlayHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayHistories_Songs_SongId",
                table: "PlayHistories");

            migrationBuilder.DropIndex(
                name: "IX_PlayHistories_SongId",
                table: "PlayHistories");

            migrationBuilder.DropColumn(
                name: "SongId",
                table: "PlayHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SongId",
                table: "PlayHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayHistories_SongId",
                table: "PlayHistories",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayHistories_Songs_SongId",
                table: "PlayHistories",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId");
        }
    }
}
