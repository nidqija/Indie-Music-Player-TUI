using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSongInfotoPlayHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayHistories_Songs_SongId",
                table: "PlayHistories");

            migrationBuilder.AlterColumn<int>(
                name: "SongId",
                table: "PlayHistories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "songArtist",
                table: "PlayHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "songTitle",
                table: "PlayHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "songUrl",
                table: "PlayHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayHistories_Songs_SongId",
                table: "PlayHistories",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayHistories_Songs_SongId",
                table: "PlayHistories");

            migrationBuilder.DropColumn(
                name: "songArtist",
                table: "PlayHistories");

            migrationBuilder.DropColumn(
                name: "songTitle",
                table: "PlayHistories");

            migrationBuilder.DropColumn(
                name: "songUrl",
                table: "PlayHistories");

            migrationBuilder.AlterColumn<int>(
                name: "SongId",
                table: "PlayHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayHistories_Songs_SongId",
                table: "PlayHistories",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "SongId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
