using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Correo { get; set; }

        [Display(Name="IdEmpresa")]
        public int IdEmpresa{ get; set; }

        [Display(Name = "Empresa")]

        [ValidateNever]
        public string NombreEmpresa { get; set; }
        public String Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
    }
}
