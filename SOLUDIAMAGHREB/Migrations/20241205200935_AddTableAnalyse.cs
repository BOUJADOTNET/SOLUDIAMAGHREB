using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOLUDIAMAGHREB.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAnalyse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_bordereauItems_Nlot",
                table: "bordereauItems",
                column: "Nlot");

            migrationBuilder.CreateTable(
                name: "Analyse",
                columns: table => new
                {
                    idAvisAppelOff = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nlot = table.Column<int>(type: "int", nullable: false),
                    Montant_total_DHS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Montant_total_littre_DHS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomberBordereau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CAST(GETDATE() AS DATE)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk__IDAvisAppelOffer", x => x.idAvisAppelOff);
                    table.ForeignKey(
                        name: "FK_Analyse_bordereauItems_Nlot",
                        column: x => x.Nlot,
                        principalTable: "bordereauItems",
                        principalColumn: "Nlot",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyse_idAvisAppelOff",
                table: "Analyse",
                column: "idAvisAppelOff",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Analyse_Nlot",
                table: "Analyse",
                column: "Nlot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analyse");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_bordereauItems_Nlot",
                table: "bordereauItems");
        }
    }
}
