using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class ProductoCreacionViewModel:Producto

    {
        [ValidateNever]
        public IEnumerable<SelectListItem> categorias { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> reglas { get; set; }
    }
}
