using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class ColaboradorCreacionViewModel : Colaborador
    {
        [ValidateNever]
        public IEnumerable<SelectListItem> perfiles { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> tipocontratos { get; set; }

        
    }
}
