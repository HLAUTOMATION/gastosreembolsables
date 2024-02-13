using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class ClienteCreacionViewModel : Cliente
    {
        [ValidateNever]
        public IEnumerable<SelectListItem> empresas { get; set; }
       
    }
}
