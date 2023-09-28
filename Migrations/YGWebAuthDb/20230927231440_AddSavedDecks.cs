using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGWeb.Migrations.YGWebAuthDb
{
    /// <inheritdoc />
    public partial class AddSavedDecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deck",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YGWebUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deck_AspNetUsers_YGWebUserId",
                        column: x => x.YGWebUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deck_YGWebUserId",
                table: "Deck",
                column: "YGWebUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deck");
        }
    }
}
