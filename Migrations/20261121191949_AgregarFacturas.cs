using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVC_Contactos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBLFACTURAS",
                columns: table => new
                {
                    IdFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdEmpleado = table.Column<int>(type: "int", nullable: false),
                    DtmFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false),
                    DecSubtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DecDescuento = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DecIva = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DecTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DtmFechaModifica = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StrUsuarioModifico = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLFACTURAS", x => x.IdFactura);
                    table.ForeignKey(
                        name: "FK_TBLFACTURAS_TBLEMPLEADO_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "TBLEMPLEADO",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBLFACTURADETALLE",
                columns: table => new
                {
                    IdDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFactura = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IntCantidad = table.Column<int>(type: "int", nullable: false),
                    DecPrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DecSubtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLFACTURADETALLE", x => x.IdDetalle);
                    table.ForeignKey(
                        name: "FK_TBLFACTURADETALLE_TBLFACTURAS_IdFactura",
                        column: x => x.IdFactura,
                        principalTable: "TBLFACTURAS",
                        principalColumn: "IdFactura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBLFACTURADETALLE_TBLPRODUCTOS_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "TBLPRODUCTOS",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TBLFACTURAS",
                columns: new[] { "IdFactura", "DecDescuento", "DecIva", "DecSubtotal", "DecTotal", "DtmFecha", "DtmFechaModifica", "IdCliente", "IdEmpleado", "IdEstado", "StrUsuarioModifico" },
                values: new object[,]
                {
                    { 1, 0m, 6175m, 32500m, 38675m, new DateTime(2026, 11, 18, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 2, "javier" },
                    { 2, 3520m, 6019.20m, 35200m, 37699.20m, new DateTime(2026, 11, 19, 16, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 1, "javier" },
                    { 3, 0m, 3648m, 19200m, 22848m, new DateTime(2026, 11, 20, 14, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 3, "javier" },
                    { 4, 825m, 2978.25m, 16500m, 18653.25m, new DateTime(2026, 10, 25, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 4, "javier" }
                });

            migrationBuilder.InsertData(
                table: "TBLFACTURADETALLE",
                columns: new[] { "IdDetalle", "DecPrecioUnitario", "DecSubtotal", "IdFactura", "IdProducto", "IntCantidad" },
                values: new object[,]
                {
                    { 1, 1500m, 15000m, 1, 1, 10 },
                    { 2, 3500m, 17500m, 1, 4, 5 },
                    { 3, 9800m, 19600m, 2, 6, 2 },
                    { 4, 5200m, 15600m, 2, 7, 3 },
                    { 5, 4800m, 19200m, 3, 2, 4 },
                    { 6, 4500m, 4500m, 4, 8, 1 },
                    { 7, 1200m, 12000m, 4, 9, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBLFACTURADETALLE_IdFactura",
                table: "TBLFACTURADETALLE",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_TBLFACTURADETALLE_IdProducto",
                table: "TBLFACTURADETALLE",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_TBLFACTURAS_IdCliente",
                table: "TBLFACTURAS",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_TBLFACTURAS_IdEmpleado",
                table: "TBLFACTURAS",
                column: "IdEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBLFACTURADETALLE");

            migrationBuilder.DropTable(
                name: "TBLFACTURAS");
        }
    }
}
