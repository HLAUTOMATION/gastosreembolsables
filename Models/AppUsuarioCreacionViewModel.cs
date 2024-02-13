using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class AppUsuarioCreacionViewModel : AppUsuario
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public String Email { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> tipocontratos { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> proyectos { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> perfiles { get; set; }


        public bool Estado { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> colaboradores { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> roles { get; set; }

        [Display(Name = "Role")]
        public string IdRole { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(50, ErrorMessage = "El {0} debe estar entre al menos {2} caracteres de longitud", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public String Password { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "La contraseña y confirmación de contraseña no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

    }
}
