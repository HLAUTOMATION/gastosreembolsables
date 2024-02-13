using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public String Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public String Password { get; set; }

        public bool Recuerdame { get; set; }

    }
}
