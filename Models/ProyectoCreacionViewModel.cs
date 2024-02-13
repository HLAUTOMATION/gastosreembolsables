using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class ProyectoCreacionViewModel : Proyecto
    {

        [ValidateNever]
        public IEnumerable<SelectListItem> empresas { get; set; }
    }
}
