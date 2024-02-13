using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class CuentasController:Controller
    {
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        public CuentasController(
            IRepositorioCuentas repositorioCuentas,
            IServicioUsuarios servicioUsuarios)
        {
            this.repositorioCuentas = repositorioCuentas;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {

            var cuentas=await repositorioCuentas.ListarCuentas();
            return View(cuentas);
        }

        public async Task<IActionResult> GetGuentasByIdColaborador(int Id)
        {

            var cuentas = await repositorioCuentas.GetCuentasByIdUsuario(Id);
            return View(cuentas);
        }

        public async Task<IActionResult> IndexColaborador( )
        {

            var EmailAppUsuario = servicioUsuarios.GetADUserEmail();
            var cuentas = await repositorioCuentas.GetCuentasByEmailAppUsuario(EmailAppUsuario);
            return View(cuentas);
        }
    }
}
