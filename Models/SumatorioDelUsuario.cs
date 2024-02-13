namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class SumatorioDelUsuario
    {
        public IEnumerable<Sumatorio> SumatorioOperacionesPendientes{ get; set; }

        public IEnumerable<Sumatorio> SumatorioOperacionesProcesadas { get; set; }
    }
}
