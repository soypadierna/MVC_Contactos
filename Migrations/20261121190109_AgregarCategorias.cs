using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Contactos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBLCATEGORIAS",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrDescripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DtmFechaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StrUsuarioModifico = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLCATEGORIAS", x => x.IdCategoria);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBLCATEGORIAS_StrDescripcion",
                table: "TBLCATEGORIAS",
                column: "StrDescripcion",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBLCATEGORIAS");
        }
    }
}
