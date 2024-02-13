using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class ReglasController : Controller
    {
        private readonly IRepositorioReglas repositorioReglas;
        private readonly IMapper mapper;

        public ReglasController(
            IRepositorioReglas repositorioReglas,
            IMapper mapper)
        {
            this.repositorioReglas = repositorioReglas;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacionViewModel)
        {

            var reglas=await repositorioReglas.ListarReglasPaginacion(paginacionViewModel);
            var totalReglas = await repositorioReglas.ContarReglas();

            var respuestaVM = new PaginacionRespuesta<Regla>
            {
                Elementos = reglas,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalReglas,
                BaseURL = "/Reglas"
            };
            return View(respuestaVM);
        }

        public async Task<IActionResult> ReglasPartialView()
        {

            var reglas = await repositorioReglas.ListarReglas();
           
            return PartialView(reglas);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Regla regla)
        {
            regla.Nombre.ToUpper();
            await repositorioReglas.Crear(regla);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Editar(int Id)
        {
            var regla = await repositorioReglas.GetReglaById(Id);
            if (regla is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(regla);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(Regla reglaNew)
        {
            var regla = await repositorioReglas.GetReglaById(reglaNew.Id);
            if (regla is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

         

            await repositorioReglas.Editar(reglaNew);
            return RedirectToAction("Index");
        }
    }
}
