using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class AsignacionCreacionViewModel :Asignacion

    {

        [ValidateNever]
        public IEnumerable<SelectListItem> colaboradores { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> empresas { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> proyectos { get; set; }

        public EmpresaEnum enumId { get; set; } = EmpresaEnum.BHP ;
    }
}
