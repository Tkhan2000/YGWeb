using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCardsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    atk = table.Column<int>(type: "int", nullable: true),
                    def = table.Column<int>(type: "int", nullable: true),
                    level = table.Column<int>(type: "int", nullable: true),
                    race = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    archetype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
