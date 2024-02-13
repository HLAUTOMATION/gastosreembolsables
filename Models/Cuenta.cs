namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        public string NombreBanco { get; set; }

        public string NumeroCuenta { get; set; }

        public string TipoCuenta { get; set; }

        public int IdColaborador { get; set; }

        public string CorreoUsuario { get; set; }
        public DateTime  FechaCreacion{ get; set; }
        public DateTime FechaFin { get; set; }

        public int Estado { get; set; }

    }
}
