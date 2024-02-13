namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Sector { get; set; }
        public String Direccion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
    }
}

