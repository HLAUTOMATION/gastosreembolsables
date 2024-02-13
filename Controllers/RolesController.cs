using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class RolesController : Controller
	{
		private readonly UserManager<AppUsuario> userManager;
		private readonly SignInManager<AppUsuario> signInManager;
		private readonly RoleManager<AppUsuario> roleManager;
		private readonly IRepositorioUsuarios repositorioUsuarios;
		private readonly IRepositorioTipoUsuario repositorioTipoUsuario;

		public RolesController(
			UserManager<AppUsuario> userManager,
            SignInManager<AppUsuario> signInManager,
           RoleManager<AppUsuario> roleManager,
            IRepositorioUsuarios repositorioUsuarios,
			IRepositorioTipoUsuario repositorioTipoUsuario)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
			this.repositorioUsuarios = repositorioUsuarios;
			this.repositorioTipoUsuario = repositorioTipoUsuario;
		}
		public async Task<IActionResult> Index()
		{
            var tipoUsuarios = await repositorioTipoUsuario.ListarTipoUsuario();
            return View(tipoUsuarios);
        }


	}
}
