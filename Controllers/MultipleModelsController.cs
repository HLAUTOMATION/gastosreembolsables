using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
	public class MultipleModelsController : Controller
	{
		private readonly IServicioUsuarios servicioUsuarios;
		private readonly IRepositorioOperaciones repositorioOperaciones;
		private readonly IRepositorioAppUsuarios repositorioAppUsuarios;
		private readonly IMapper mapper;
        private readonly IRepositorioProductos repositorioProductos;

        public MultipleModelsController(
			IServicioUsuarios servicioUsuarios,
			IRepositorioOperaciones repositorioOperaciones,
            IRepositorioAppUsuarios repositorioAppUsuarios
            )
		{
			this.servicioUsuarios = servicioUsuarios;
			this.repositorioOperaciones = repositorioOperaciones;
			this.repositorioAppUsuarios = repositorioAppUsuarios;
			this.mapper = mapper;
            this.repositorioProductos = repositorioProductos;
        }
		public async Task<IActionResult> IndexMultipleModels(PaginacionViewModel paginacionViewModel)
		{
            var modelo = new MultipleModels();
            var correoUsuario = servicioUsuarios.GetADUserEmail();

           
            var totalOperaciones = await repositorioOperaciones.ContarOperacionesPendientesByCorreoUsuario(correoUsuario);
            var respuestaVM = new PaginacionRespuesta<Operacion>
            {
                Elementos = await GetOperacionesByCorreoUsuario(correoUsuario, paginacionViewModel),
            Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalOperaciones,
                BaseURL = "/MultipleModels/IndexMultipleModels"
            };
            modelo.operaciones =  respuestaVM;
            modelo.appUsuario = await GetAppUsuarioByEmail();



            return View(modelo);
		}

		public async Task<IEnumerable<Operacion>> GetOperacionesByCorreoUsuario(string correoUsuario, PaginacionViewModel paginacionViewModel)
		{

            
            var operaciones = await repositorioOperaciones.GetOperacionesByCorreoUsuarioPaginacion(correoUsuario, paginacionViewModel);
            return operaciones;

        }

        public async Task<AppUsuario> GetAppUsuarioByEmail()
        {

            var emailNormalizado = servicioUsuarios.GetADUserEmail().ToUpper();
            var appUsuario = repositorioAppUsuarios.GetAppUsuarioByEmail(emailNormalizado);


            return await appUsuario;

        }

     

        public async Task<IEnumerable<SelectListItem>> SelectProductos()
        {
            var productos = await repositorioProductos.ListarProductos();
            return productos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }


    }
}
