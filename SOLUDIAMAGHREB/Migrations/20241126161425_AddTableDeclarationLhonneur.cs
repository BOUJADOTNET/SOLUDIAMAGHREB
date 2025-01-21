using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOLUDIAMAGHREB.Migrations
{
    /// <inheritdoc />
    public partial class AddTableDeclarationLhonneur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Declarationlh",
                columns: table => new
                {
                    idDeclar = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NomberBordereau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppelOffer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectMarche = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CAST(GETDATE() AS DATE)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Déclarationlhonneur", x => x.idDeclar);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Declarationlh");
        }
    }
}
