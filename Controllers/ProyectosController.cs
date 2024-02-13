using AutoMapper;
using Azure;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class ProyectosController : Controller
    {
        private readonly IRepositorioProyectos repositorioProyectos;
        private readonly IRepositorioEmpresas repositorioEmpresas;
        private readonly IMapper mapper;

        public ProyectosController(
            IRepositorioProyectos repositorioProyectos,
            IRepositorioEmpresas repositorioEmpresas,
            IMapper mapper)
        {
            this.repositorioProyectos = repositorioProyectos;
            this.repositorioEmpresas = repositorioEmpresas;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacionViewModel)
        {
            var proyectos=await repositorioProyectos.ListarProyectosPaginacion(paginacionViewModel);
            var totalProyectos = await repositorioProyectos.ContarProyectos();
            var respuestaVM = new PaginacionRespuesta<Proyecto>
            {
                Elementos = proyectos,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalProyectos,
                BaseURL = "/Proyectos"
            };
            return View(respuestaVM);
        }

    [HttpGet]
        public  async Task<IActionResult> Crear()
        {
            var modelo = new ProyectoCreacionViewModel();
            modelo.empresas =await SelectEmpresas();
            return View(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Crear(ProyectoCreacionViewModel proyecto)
        {

            var empresa = await repositorioEmpresas.GetEmpresaById(proyecto.IdEmpresa);
            if (empresa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            if (!ModelState.IsValid)
            {
                proyecto.empresas = await SelectEmpresas();
                return View(proyecto);
            }


            await repositorioProyectos.Crear(proyecto);
            return RedirectToAction("Index");
        }

        public async Task<IEnumerable<SelectListItem>> SelectEmpresas()
        {
            var empresas = await repositorioEmpresas.ListarEmpresas();
            return empresas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IActionResult> Editar(int Id)
        {
            var proyecto=await repositorioProyectos.GetProyectoById(Id);
            if (proyecto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<ProyectoCreacionViewModel>(proyecto);
      
            modelo.empresas = await SelectEmpresas();
            return View(modelo);


        }

        [HttpPost]
        public async Task<IActionResult> Editar(Proyecto proyectoNew)
        {
            var proyecto = repositorioProyectos.GetProyectoById(proyectoNew.Id);
            if (proyecto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var empresa = repositorioEmpresas.GetEmpresaById(proyectoNew.IdEmpresa);
            if (empresa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioProyectos.Editar(proyectoNew);
            return RedirectToAction("Index");

        }


        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var proyecto = await repositorioProyectos.GetProyectoById(Id);
            return PartialView("BorrarPartialView", proyecto);
        }


        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(Proyecto proyecto)
        {
            var ProyectoToBorrar = await repositorioProyectos.GetProyectoById(proyecto.Id);
            if (ProyectoToBorrar is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioProyectos.Borrar(ProyectoToBorrar.Id);

            //await servicioEmailSendGrid.Actualizar(operacionNew);
            return RedirectToAction("Index", "Proyectos");

        }
    }
}
