using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Proyecto
    {
        public int Id { get; set; }

        [Display(Name = "IdEmpresa")]
        public int IdEmpresa { get; set; }

       // [EnumDataType(typeof(EmpresaEnum))]
        public EmpresaEnum enumId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Descripcíon")]
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; } 
        public DateTime FechaFin { get; set; }

        public int Estado { get; set; }


        [Display(Name = "Empresa")]
        [ValidateNever]
        public string NombreEmpresa { get; set; }
    }
}
