using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Producto
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        public String Nombre { get; set; }

        [Display(Name = "Categoria")]
        [ValidateNever]
        public string NombreCategoria { get; set; }
        public int IdCategoria { get; set; }

        [Display(Name = "Regla aplicada")]
        [ValidateNever]
        public string NombreRegla { get; set; }
        public int IdRegla { get; set; }
        [ValidateNever]
        public DateTime FechaCreacion { get; set; }
        [ValidateNever]
        public DateTime FechaFin { get; set; }
        [ValidateNever]
        public int Estado { get; set; }
    }
}
