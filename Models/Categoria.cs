namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFin { get; set; }

        public int Estado { get; set; } 
    }
}
