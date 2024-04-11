using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms.Backend.Migrations
{
    /// <inheritdoc />
    public partial class eliminacion_indice_unico_lastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Lastname",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Lastname",
                table: "Usuarios",
                column: "Lastname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Lastname",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Lastname",
                table: "Usuarios",
                column: "Lastname",
                unique: true);
        }
    }
}
