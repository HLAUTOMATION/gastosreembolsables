using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Colaborador
    {

        public int Id { get; set; }
        public String Nombre { get; set; }

        [Display(Name ="Perfile")]
        public int IdPerfile { get; set; }
        public String Correo { get; set; }

        [Display(Name = "Fecha Contrato")]
        public DateTime FechaContrato { get; set; }

        [Display(Name = "TipoContrato")]
        public int IdTipoContrato { get; set; }
        public int Antiguedad => (DateTime.Now.Year - FechaContrato.Year)*12+ DateTime.Now.Month-FechaContrato.Month;

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
        
    }
}
