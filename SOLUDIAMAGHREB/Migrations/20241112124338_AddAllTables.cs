using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOLUDIAMAGHREB.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BORDEREAUMANAGER",
                columns: table => new
                {
                    NomberBordereau = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CAST(GETDATE() AS DATE)"),
                    Intitu_AppelOffres = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BORDEREAUMANAGER", x => x.NomberBordereau);
                });

            migrationBuilder.CreateTable(
                name: "MyBorderauItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nlot = table.Column<int>(type: "int", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomdeMarque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conditionnement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unite_Compte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    Prix_Unit_TVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Prix_Total_TVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NomberBordereau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppelOffres = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyBorderauItems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bordereauItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nlot = table.Column<int>(type: "int", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unite_Compte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    Prix_Unit_TVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Prix_Total_TVA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NomberBordereau = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BordereauItems", x => new { x.Nlot, x.Id });
                    table.ForeignKey(
                        name: "FK_bordereauItems_BORDEREAUMANAGER_NomberBordereau",
                        column: x => x.NomberBordereau,
                        principalTable: "BORDEREAUMANAGER",
                        principalColumn: "NomberBordereau",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bordereauItems_Id",
                table: "bordereauItems",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bordereauItems_NomberBordereau",
                table: "bordereauItems",
                column: "NomberBordereau");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bordereauItems");

            migrationBuilder.DropTable(
                name: "MyBorderauItems");

            migrationBuilder.DropTable(
                name: "BORDEREAUMANAGER");
        }
    }
}
