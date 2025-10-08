using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicPlayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCollectionToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "Songs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CollectionName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionId);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Collections_CollectionId",
                table: "Songs");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_Songs_CollectionId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "Songs");
        }
    }
}
