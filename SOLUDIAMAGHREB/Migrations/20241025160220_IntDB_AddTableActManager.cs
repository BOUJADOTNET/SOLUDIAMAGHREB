using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOLUDIAMAGHREB.Migrations
{
    /// <inheritdoc />
    public partial class IntDB_AddTableActManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACTMANAGER",
                columns: table => new
                {
                    IdactEng = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Appel_Offres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Objet_du_Marche = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Marche_passe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Capital = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NLot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Taux_TVA = table.Column<int>(type: "int", nullable: false),
                    Montant_HT_TVA = table.Column<int>(type: "int", nullable: false),
                    Montant_TVA = table.Column<int>(type: "int", nullable: false),
                    Montant_DH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Montant_TVA_Comprise = table.Column<int>(type: "int", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CAST(GETDATE() AS DATE)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ActManager", x => x.IdactEng);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true),
                    Prenom = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true),
                    UserName = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true),
                    Email = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true),
                    Clave = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Utilisat__3717C98297588A3A", x => x.idUser);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACTMANAGER");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
