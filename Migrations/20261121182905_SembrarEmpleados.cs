using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVC_Contactos.Migrations
{
    /// <inheritdoc />
    public partial class SembrarEmpleados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TBLEMPLEADO",
                columns: new[] { "IdEmpleado", "DtmFechaModifica", "DtmIngreso", "DtmRetiro", "IdRolEmpleado", "NumDocumento", "StrDireccion", "StrEmail", "StrTelefono", "StrUsuarioModifico", "TBLROLESIdRolEmpleado", "strDatosAdicionales", "strNombre" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1012345678L, "Cra 10 # 20-30", "laura.gomez@empresa.com", "3001112233", "javier", null, "Administradora general", "Laura Gómez" },
                    { 2, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1023456789L, "Calle 45 # 12-08", "carlos.ruiz@empresa.com", "3012223344", "javier", null, "Ventas zona norte", "Carlos Ruiz" },
                    { 3, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 1034567890L, "Av. 68 # 5-50", "marta.perez@empresa.com", "3023334455", "javier", null, "Caja principal", "Marta Pérez" },
                    { 4, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1045678901L, "Cra 7 # 72-14", "andres.torres@empresa.com", "3034445566", "javier", null, "Ventas zona sur", "Andrés Torres" },
                    { 5, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1056789012L, "Calle 100 # 15-20", "sofia.herrera@empresa.com", "3045556677", "javier", null, "Retirada por fin de contrato", "Sofía Herrera" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TBLEMPLEADO",
                keyColumn: "IdEmpleado",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TBLEMPLEADO",
                keyColumn: "IdEmpleado",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TBLEMPLEADO",
                keyColumn: "IdEmpleado",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TBLEMPLEADO",
                keyColumn: "IdEmpleado",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TBLEMPLEADO",
                keyColumn: "IdEmpleado",
                keyValue: 5);
        }
    }
}
