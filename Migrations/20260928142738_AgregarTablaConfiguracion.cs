using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManager.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaConfiguracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuraciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreGimnasio = table.Column<string>(type: "TEXT", nullable: false),
                    TelefonoContacto = table.Column<string>(type: "TEXT", nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: false),
                    PrecioCuotaMensual = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiasToleranciaMora = table.Column<int>(type: "INTEGER", nullable: false),
                    MensajePieRecibo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuraciones", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Configuraciones",
                columns: new[] { "Id", "DiasToleranciaMora", "Direccion", "MensajePieRecibo", "NombreGimnasio", "PrecioCuotaMensual", "TelefonoContacto" },
                values: new object[] { 1, 0, "", "¡Gracias por entrenar con nosotros!", "Mi Gimnasio", 25000m, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuraciones");
        }
    }
}
