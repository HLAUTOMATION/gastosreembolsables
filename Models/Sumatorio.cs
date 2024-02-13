namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Sumatorio
    {

        public int Id { get; set; }
        public int idProducto { get; set; }
        public string NombreProducto { get; set; }

        public string CorreoUsuario { get; set; }

        public string YearMonth { get; set; }

        public double Total { get; set; }
        public double Subtotal { get; set; }

        public double MontoReembolsado { get; set; }

        public double SubtotalReembolsado { get; set; }

        public double Estado { get; set; }
    }
}
