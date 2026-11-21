using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVC_Contactos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBLPRODUCTOS",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrNombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StrCodigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DecPrecioCompra = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DecPrecioVenta = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    StrDetalle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IntStock = table.Column<int>(type: "int", nullable: false),
                    StrFoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DtmFechaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StrUsuarioModifico = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLPRODUCTOS", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_TBLPRODUCTOS_TBLCATEGORIAS_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "TBLCATEGORIAS",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TBLCATEGORIAS",
                columns: new[] { "IdCategoria", "DtmFechaModifica", "StrDescripcion", "StrUsuarioModifico" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bebidas", "javier" },
                    { 2, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Snacks", "javier" },
                    { 3, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aseo", "javier" },
                    { 4, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Papelería", "javier" }
                });

            migrationBuilder.InsertData(
                table: "TBLPRODUCTOS",
                columns: new[] { "IdProducto", "DecPrecioCompra", "DecPrecioVenta", "DtmFechaModifica", "IdCategoria", "IntStock", "StrCodigo", "StrDetalle", "StrFoto", "StrNombre", "StrUsuarioModifico" },
                values: new object[,]
                {
                    { 1, 900m, 1500m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 120, "AGU-001", "Botella PET", null, "Agua mineral 600 ml", "javier" },
                    { 2, 3200m, 4800m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 60, "GAS-001", "Botella familiar", null, "Gaseosa cola 1.5 L", "javier" },
                    { 3, 2800m, 4200m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 45, "JUG-001", "Caja tetrapak", null, "Jugo de naranja 1 L", "javier" },
                    { 4, 2100m, 3500m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 80, "SNK-001", "Sabor natural", null, "Papas fritas 150 g", "javier" },
                    { 5, 1200m, 2000m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 100, "SNK-002", "Paquete x6", null, "Galletas de chocolate", "javier" },
                    { 6, 6500m, 9800m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 30, "ASE-001", "Ropa blanca y de color", null, "Detergente en polvo 1 kg", "javier" },
                    { 7, 3000m, 5200m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 40, "ASE-002", "Antibacterial", null, "Jabón de manos 500 ml", "javier" },
                    { 8, 2500m, 4500m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, "PAP-001", "Pasta blanda", null, "Cuaderno rayado 100 hojas", "javier" },
                    { 9, 600m, 1200m, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 200, "PAP-002", "Punta media", null, "Lapicero azul", "javier" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBLPRODUCTOS_IdCategoria",
                table: "TBLPRODUCTOS",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_TBLPRODUCTOS_StrCodigo",
                table: "TBLPRODUCTOS",
                column: "StrCodigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBLPRODUCTOS");

            migrationBuilder.DeleteData(
                table: "TBLCATEGORIAS",
                keyColumn: "IdCategoria",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TBLCATEGORIAS",
                keyColumn: "IdCategoria",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TBLCATEGORIAS",
                keyColumn: "IdCategoria",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TBLCATEGORIAS",
                keyColumn: "IdCategoria",
                keyValue: 4);
        }
    }
}
