using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class AppUsuarioLoginViewModel:AppUsuario
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public String Password { get; set; }

        [Display(Name = "Recuerda me")]
        public bool Recuerdame { get; set; }

    }
}
