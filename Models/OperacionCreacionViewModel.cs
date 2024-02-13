using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class OperacionCreacionViewModel : Operacion
    {
        

        [ValidateNever]
        public IEnumerable<SelectListItem> productos { get; set; }

       

        [ValidateNever]
        public IFormFile Documento { get; set; }

        [ValidateNever]
        public IEnumerable<Regla> reglas { get; set; }

    }
}
