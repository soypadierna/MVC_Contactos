using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVC_Contactos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEmpleados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBLROLES",
                columns: table => new
                {
                    IdRolEmpleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrDescripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLROLES", x => x.IdRolEmpleado);
                });

            migrationBuilder.CreateTable(
                name: "TBLEMPLEADO",
                columns: table => new
                {
                    IdEmpleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumDocumento = table.Column<long>(type: "bigint", nullable: false),
                    StrDireccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdRolEmpleado = table.Column<int>(type: "int", nullable: true),
                    DtmIngreso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DtmRetiro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    strDatosAdicionales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DtmFechaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StrUsuarioModifico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TBLROLESIdRolEmpleado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLEMPLEADO", x => x.IdEmpleado);
                    table.ForeignKey(
                        name: "FK_TBLEMPLEADO_TBLROLES_TBLROLESIdRolEmpleado",
                        column: x => x.TBLROLESIdRolEmpleado,
                        principalTable: "TBLROLES",
                        principalColumn: "IdRolEmpleado");
                });

            migrationBuilder.InsertData(
                table: "TBLROLES",
                columns: new[] { "IdRolEmpleado", "StrDescripcion" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Vendedor" },
                    { 3, "Cajero" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBLEMPLEADO_TBLROLESIdRolEmpleado",
                table: "TBLEMPLEADO",
                column: "TBLROLESIdRolEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBLEMPLEADO");

            migrationBuilder.DropTable(
                name: "TBLROLES");
        }
    }
}
