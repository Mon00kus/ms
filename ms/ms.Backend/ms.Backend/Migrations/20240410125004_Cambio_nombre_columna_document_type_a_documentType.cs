using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Cambio_nombre_columna_document_type_a_documentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Document_type",
                table: "Usuarios",
                newName: "DocumentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "Usuarios",
                newName: "Document_type");
        }
    }
}
