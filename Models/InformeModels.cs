namespace MVC_Contactos.Models
{
    public class InformeVM
    {
        public string Tipo { get; set; } = "ventas";
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public InformeResultado? Resultado { get; set; }
    }

    public class InformeResultado
    {
        public string Titulo { get; set; } = "";
        public string Periodo { get; set; } = "";
        public List<InformeSeccion> Secciones { get; set; } = new();
    }

    public class InformeSeccion
    {
        public string Titulo { get; set; } = "";
        public List<Metrica> Metricas { get; set; } = new();
        public List<string> Columnas { get; set; } = new();
        public List<string[]> Filas { get; set; } = new();
    }

    public record Metrica(string Etiqueta, string Valor);
}