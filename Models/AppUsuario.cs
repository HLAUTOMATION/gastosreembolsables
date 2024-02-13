using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class AppUsuario : IdentityUser
    {
        

       
        public int Id { get; set; }

        [ValidateNever]
        public string IdRol { get; set; }

        [ValidateNever]
        public string Password { get; set; }


        [ValidateNever]
        public string ConfirmPassword { get; set; }

        [ValidateNever]
        [Display(Name="Colaborador")]
        public int IdColaborador { get; set; }

        [Display(Name = "Fecha Creacion")]
        [ValidateNever]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha Desactivacion")]
        [ValidateNever]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Estado")]
        [ValidateNever]
        public int Estado { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        [ValidateNever]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Fecha Contrato")]
        [ValidateNever]
        public DateTime FechaContrato { get; set; }

        public int Antiguedad => (DateTime.Now.Year - FechaContrato.Year) * 12 + DateTime.Now.Month - FechaContrato.Month;

        [ValidateNever]
        [Display(Name = "Tipo Contrato")]
        public int TipoContrato { get; set; }

        [Display(Name = "Apellido Nombre")]
        [ValidateNever]
        public string ApellidoNombre { get; set; }

        [Display(Name = "Proyecto")]
        [ValidateNever]
        public string NombreProyecto { get; set; }


        [Display(Name = "Rut")]
        [ValidateNever]
        public string Rut { get; set; }

        [ValidateNever]
        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }


        [Display(Name = "Perfile")]
        [ValidateNever]
        public string NombrePerfile { get; set; }

        [ValidateNever]
        public int IdPerfile { get; set; }

        [Display(Name = "Cuentas del Usuario")]
        [ValidateNever]
        public IEnumerable<Cuenta> cuentas { get; set; }

        [ValidateNever]

        [Display(Name = "Correo")]
        public string EmailUsuario { get; set; }


        

    }
}
