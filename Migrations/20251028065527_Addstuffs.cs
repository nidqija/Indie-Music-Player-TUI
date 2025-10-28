using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlayer.Migrations
{
    /// <inheritdoc />
    public partial class Addstuffs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Collections_CollectionId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_CollectionId",
                table: "Songs");

            migrationBuilder.CreateTable(
                name: "SongCollections",
                columns: table => new
                {
                    CollectionsCollectionId = table.Column<int>(type: "integer", nullable: false),
                    SongsSongId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongCollections", x => new { x.CollectionsCollectionId, x.SongsSongId });
                    table.ForeignKey(
                        name: "FK_SongCollections_Collections_CollectionsCollectionId",
                        column: x => x.CollectionsCollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongCollections_Songs_SongsSongId",
                        column: x => x.SongsSongId,
                        principalTable: "Songs",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongCollections_SongsSongId",
                table: "SongCollections",
                column: "SongsSongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongCollections");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_CollectionId",
                table: "Songs",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Collections_CollectionId",
                table: "Songs",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "CollectionId");
        }
    }
}
