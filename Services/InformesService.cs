using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MVC_Contactos.Data;
using MVC_Contactos.Models;

namespace MVC_Contactos.Services
{
    public class InformesService
    {
        public static readonly Dictionary<string, string> Tipos = new()
        {
            { "ventas", "Ventas" },
            { "inventario", "Inventario" },
            { "financiero", "Financiero" },
            { "empleados", "Empleados" },
            { "clientes", "Clientes" }
        };

        private const int MaxDias = 366;
        private static readonly CultureInfo Cul = new("es-CO");
        private static readonly TimeZoneInfo Zona = TimeZoneInfo.Local;

        private readonly ContactosDbContext _context;
        private readonly ClienteDatos _clientes;

        public InformesService(ContactosDbContext context, IConfiguration configuration)
        {
            _context = context;
            _clientes = new ClienteDatos(configuration);
        }

        public string? Validar(string? tipo, DateTime? desde, DateTime? hasta)
        {
            if (string.IsNullOrEmpty(tipo) || !Tipos.ContainsKey(tipo)) return "Tipo de informe no válido.";
            if (tipo == "inventario") return null;
            if (desde == null || hasta == null) return "Seleccione las fechas desde y hasta.";
            if (hasta.Value.Date < desde.Value.Date) return "La fecha hasta no puede ser menor que la fecha desde.";
            if ((hasta.Value.Date - desde.Value.Date).Days + 1 > MaxDias) return $"El rango máximo es de {MaxDias} días.";
            return null;
        }

        public InformeResultado Generar(string tipo, DateTime? desde, DateTime? hasta)
        {
            if (tipo == "inventario") return Inventario();

            var d = desde!.Value.Date;
            var h = hasta!.Value.Date;
            var ini = ToUtc(d);
            var fin = ToUtc(h.AddDays(1));

            var r = tipo switch
            {
                "ventas" => Ventas(ini, fin),
                "financiero" => Financiero(ini, fin),
                "empleados" => Empleados(ini, fin),
                _ => Clientes(ini, fin)
            };
            r.Periodo = $"{d:dd/MM/yyyy} a {h:dd/MM/yyyy}";
            return r;
        }

        private InformeResultado Ventas(DateTime ini, DateTime fin)
        {
            var todas = Base(ini, fin);
            var filas = todas.Where(f => f.IdEstado != 3)
                .Select(f => new { f.DtmFecha, f.DecSubtotal, f.DecDescuento, f.DecIva, f.DecTotal })
                .ToList();

            var n = filas.Count;
            var total = filas.Sum(f => f.DecTotal);

            var porDia = filas
                .GroupBy(f => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(f.DtmFecha, DateTimeKind.Utc), Zona).Date)
                .OrderBy(g => g.Key)
                .Select(g => new[] { g.Key.ToString("dd/MM/yyyy"), N(g.Count()), M(g.Sum(x => x.DecTotal)) })
                .ToList();

            var top = _context.TBLFACTURADETALLE
                .Where(d => d.Factura!.DtmFecha >= ini && d.Factura.DtmFecha < fin && d.Factura.IdEstado != 3)
                .GroupBy(d => new { d.IdProducto, d.Producto!.StrNombre })
                .Select(g => new { g.Key.StrNombre, Cant = g.Sum(x => x.IntCantidad), Val = g.Sum(x => x.DecSubtotal) })
                .OrderByDescending(x => x.Cant).ThenByDescending(x => x.Val)
                .Take(5)
                .ToList()
                .Select(x => new[] { x.StrNombre, N(x.Cant), M(x.Val) })
                .ToList();

            var porEstado = todas
                .GroupBy(f => f.IdEstado)
                .Select(g => new { Estado = g.Key, N = g.Count(), Total = g.Sum(f => f.DecTotal) })
                .OrderBy(x => x.Estado)
                .ToList()
                .Select(x => new[] { Estado(x.Estado), N(x.N), M(x.Total) })
                .ToList();

            return new InformeResultado
            {
                Titulo = "Informe de ventas",
                Secciones =
                {
                    new InformeSeccion
                    {
                        Titulo = "Totales (sin anuladas)",
                        Metricas =
                        {
                            new Metrica("Facturas", N(n)),
                            new Metrica("Subtotal", M(filas.Sum(f => f.DecSubtotal))),
                            new Metrica("Descuentos", M(filas.Sum(f => f.DecDescuento))),
                            new Metrica("IVA", M(filas.Sum(f => f.DecIva))),
                            new Metrica("Total", M(total)),
                            new Metrica("Promedio por factura", M(n == 0 ? 0m : Math.Round(total / n, 2)))
                        }
                    },
                    new InformeSeccion { Titulo = "Ventas por día", Columnas = { "Fecha", "Facturas", "Total" }, Filas = porDia },
                    new InformeSeccion { Titulo = "Top 5 productos", Columnas = { "Producto", "Unidades", "Total" }, Filas = top },
                    new InformeSeccion { Titulo = "Facturas por estado", Columnas = { "Estado", "Facturas", "Total" }, Filas = porEstado }
                }
            };
        }

        private InformeResultado Inventario()
        {
            var prods = _context.TBLPRODUCTOS.Include(p => p.Categoria).OrderBy(p => p.StrNombre).ToList();

            var filas = prods.Select(p => new[]
            {
                p.StrCodigo, p.StrNombre, p.Categoria?.StrDescripcion ?? "", N(p.IntStock),
                M(p.DecPrecioCompra), M(p.DecPrecioVenta), M(p.IntStock * p.DecPrecioCompra)
            }).ToList();

            return new InformeResultado
            {
                Titulo = "Informe de inventario",
                Periodo = $"Estado al {DateTime.Now:dd/MM/yyyy HH:mm}",
                Secciones =
                {
                    new InformeSeccion
                    {
                        Titulo = "Resumen",
                        Metricas =
                        {
                            new Metrica("Productos", N(prods.Count)),
                            new Metrica("Unidades en stock", N(prods.Sum(p => p.IntStock))),
                            new Metrica("Productos sin stock", N(prods.Count(p => p.IntStock == 0))),
                            new Metrica("Valor a costo", M(prods.Sum(p => p.IntStock * p.DecPrecioCompra))),
                            new Metrica("Valor a venta", M(prods.Sum(p => p.IntStock * p.DecPrecioVenta)))
                        }
                    },
                    new InformeSeccion
                    {
                        Titulo = "Productos",
                        Columnas = { "Código", "Producto", "Categoría", "Stock", "P. compra", "P. venta", "Valor a costo" },
                        Filas = filas
                    }
                }
            };
        }

        private InformeResultado Financiero(DateTime ini, DateTime fin)
        {
            var pagadas = Base(ini, fin).Where(f => f.IdEstado == 2).ToList();
            var porCobrar = Base(ini, fin).Where(f => f.IdEstado == 1 || f.IdEstado == 4)
                .Sum(f => (decimal?)f.DecTotal) ?? 0m;
            var costo = _context.TBLFACTURADETALLE
                .Where(d => d.Factura!.DtmFecha >= ini && d.Factura.DtmFecha < fin && d.Factura.IdEstado == 2)
                .Sum(d => (decimal?)(d.IntCantidad * d.Producto!.DecPrecioCompra)) ?? 0m;

            var sub = pagadas.Sum(f => f.DecSubtotal);
            var desc = pagadas.Sum(f => f.DecDescuento);
            var neto = sub - desc;

            return new InformeResultado
            {
                Titulo = "Informe financiero",
                Secciones =
                {
                    new InformeSeccion
                    {
                        Titulo = "Resumen",
                        Metricas =
                        {
                            new Metrica("Ingresos cobrados (pagadas)", M(pagadas.Sum(f => f.DecTotal))),
                            new Metrica("Por cobrar (pendientes + vencidas)", M(porCobrar)),
                            new Metrica("Descuentos otorgados", M(desc)),
                            new Metrica("IVA cobrado", M(pagadas.Sum(f => f.DecIva))),
                            new Metrica("Venta neta sin IVA", M(neto)),
                            new Metrica("Costo de lo vendido", M(costo)),
                            new Metrica("Utilidad bruta", M(neto - costo))
                        }
                    }
                }
            };
        }

        private InformeResultado Empleados(DateTime ini, DateTime fin)
        {
            var datos = Base(ini, fin).Where(f => f.IdEstado != 3)
                .GroupBy(f => f.IdEmpleado)
                .Select(g => new { Id = g.Key, N = g.Count(), Total = g.Sum(f => f.DecTotal) })
                .ToList();
            var nombres = _context.TBLEMPLEADO.ToDictionary(e => e.IdEmpleado, e => e.strNombre ?? "");

            var filas = datos.OrderByDescending(x => x.Total)
                .Select(x => new[]
                {
                    nombres.TryGetValue(x.Id, out var nom) ? nom : $"Empleado {x.Id}",
                    N(x.N), M(x.Total), M(x.N == 0 ? 0m : Math.Round(x.Total / x.N, 2))
                }).ToList();

            return new InformeResultado
            {
                Titulo = "Informe de empleados",
                Secciones =
                {
                    new InformeSeccion
                    {
                        Titulo = "Resumen (sin anuladas)",
                        Metricas =
                        {
                            new Metrica("Empleados con ventas", N(datos.Count)),
                            new Metrica("Total vendido", M(datos.Sum(x => x.Total)))
                        }
                    },
                    new InformeSeccion { Titulo = "Ventas por empleado", Columnas = { "Empleado", "Facturas", "Total", "Promedio" }, Filas = filas }
                }
            };
        }

        private InformeResultado Clientes(DateTime ini, DateTime fin)
        {
            var datos = Base(ini, fin).Where(f => f.IdEstado != 3)
                .GroupBy(f => f.IdCliente)
                .Select(g => new { Id = g.Key, N = g.Count(), Total = g.Sum(f => f.DecTotal) })
                .ToList();
            var nombres = _clientes.ListarClientes().ToDictionary(c => c.IdCliente, c => c.StrNombre);

            var filas = datos.OrderByDescending(x => x.Total)
                .Select(x => new[]
                {
                    nombres.TryGetValue(x.Id, out var nom) ? nom : $"Cliente {x.Id}",
                    N(x.N), M(x.Total), M(x.N == 0 ? 0m : Math.Round(x.Total / x.N, 2))
                }).ToList();

            return new InformeResultado
            {
                Titulo = "Informe de clientes",
                Secciones =
                {
                    new InformeSeccion
                    {
                        Titulo = "Resumen (sin anuladas)",
                        Metricas =
                        {
                            new Metrica("Clientes con compras", N(datos.Count)),
                            new Metrica("Total comprado", M(datos.Sum(x => x.Total)))
                        }
                    },
                    new InformeSeccion { Titulo = "Compras por cliente", Columnas = { "Cliente", "Facturas", "Total", "Promedio" }, Filas = filas }
                }
            };
        }

        public static byte[] ToCsv(InformeResultado r)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Esc(r.Titulo));
            if (r.Periodo != "") sb.AppendLine(Esc("Periodo") + ";" + Esc(r.Periodo));

            foreach (var s in r.Secciones)
            {
                sb.AppendLine();
                sb.AppendLine(Esc(s.Titulo));
                foreach (var m in s.Metricas) sb.AppendLine(Esc(m.Etiqueta) + ";" + Esc(m.Valor));
                if (s.Columnas.Count > 0)
                {
                    sb.AppendLine(string.Join(";", s.Columnas.Select(Esc)));
                    foreach (var f in s.Filas) sb.AppendLine(string.Join(";", f.Select(Esc)));
                }
            }

            var enc = new UTF8Encoding(true);
            return enc.GetPreamble().Concat(enc.GetBytes(sb.ToString())).ToArray();
        }

        private IQueryable<TBLFACTURAS> Base(DateTime ini, DateTime fin) =>
            _context.TBLFACTURAS.Where(f => f.DtmFecha >= ini && f.DtmFecha < fin);

        private static DateTime ToUtc(DateTime local) =>
            TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(local, DateTimeKind.Unspecified), Zona);

        private static string Estado(int id) => id switch
        {
            1 => "Pendiente",
            2 => "Pagada",
            3 => "Anulada",
            4 => "Vencida",
            _ => "Desconocido"
        };

        private static string M(decimal v) => v.ToString("N2", Cul);
        private static string N(int v) => v.ToString("N0", Cul);

        private static string Esc(string v) =>
            v.IndexOfAny(new[] { ';', '"', '\n', '\r' }) >= 0 ? "\"" + v.Replace("\"", "\"\"") + "\"" : v;
    }
}