using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Asignacion
    {
        public int Id { get; set; }

        [Display(Name ="Colaborador")]
        public int IdColaborador { get; set; }

        [Display(Name = "Empresa")]
        public int IdEmpresa { get; set; }

        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }
    }
}
