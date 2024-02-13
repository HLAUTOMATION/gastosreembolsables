

using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class ColaboradoresController : Controller
    {

        private readonly IRepositorioColaboradores repositorioColaboradores;
        private readonly IRepositorioPerfiles repositorioPerfiles;
        private readonly IRepositorioTipoContratos repositorioTipoContratos;
        private readonly IMapper mapper;

        public ColaboradoresController(
            IRepositorioColaboradores repositorioColaboradores,
            IRepositorioPerfiles repositorioPerfiles,
            IRepositorioTipoContratos repositorioTipoContratos,
            IMapper mapper)
        {
            this.repositorioColaboradores = repositorioColaboradores;
            this.repositorioPerfiles = repositorioPerfiles;
            this.repositorioTipoContratos = repositorioTipoContratos;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var colaboradores = await repositorioColaboradores.ListarColaboradores();
            return View(colaboradores);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var modelo = new ColaboradorCreacionViewModel();
            modelo.perfiles = await SelectPerfiles();
            modelo.tipocontratos=await SelectTipoContratos();
            return View(modelo);
                
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ColaboradorCreacionViewModel colaborador)
        {
            var perfile = await repositorioPerfiles.GetPerfileById(colaborador.IdPerfile);
            if (perfile is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                colaborador.perfiles = await SelectPerfiles();
                return View(colaborador);
            }

        await repositorioColaboradores.Crear(colaborador);
            return RedirectToAction("Index");

        }

        public async Task<IEnumerable<SelectListItem>> SelectPerfiles()
        {
            var perfiles = await repositorioPerfiles.ListarPerfiles();
            return perfiles.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IEnumerable<SelectListItem>> SelectTipoContratos()
        {
            var tipocontratos = await repositorioTipoContratos.ListarTipoContratos();
            return tipocontratos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IActionResult> Editar(int Id)
        {
            var colaborador = await repositorioColaboradores.GetColaboradorById(Id);
            if (colaborador is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<ColaboradorCreacionViewModel>(colaborador);
            modelo.perfiles = await SelectPerfiles();
            modelo.tipocontratos = await SelectTipoContratos();
            return View(modelo);

        }

        public async Task<IActionResult> Ver(int Id)
        {
            var colaborador = await repositorioColaboradores.GetColaboradorById(Id);
            if (colaborador is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<ColaboradorCreacionViewModel>(colaborador);
            modelo.perfiles = await SelectPerfiles();
            modelo.tipocontratos = await SelectTipoContratos();
            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(ColaboradorCreacionViewModel colaboradorNew)
        {
            var perfile = await repositorioPerfiles.GetPerfileById(colaboradorNew.IdPerfile);
            if (perfile is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var tipocontrato = await repositorioTipoContratos.GetTipoContratoById(colaboradorNew.IdTipoContrato);
            if (tipocontrato is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var colaborador = await repositorioColaboradores.GetColaboradorById(colaboradorNew.Id);
            if (colaborador is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioColaboradores.Editar(colaboradorNew);
            return RedirectToAction("Index");
        }
    }
}
