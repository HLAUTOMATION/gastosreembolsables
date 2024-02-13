using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class PerfilesController : Controller
    {

        private readonly IRepositorioPerfiles repositorioPerfiles;

        public PerfilesController(IRepositorioPerfiles repositorioPerfiles)
        {
            this.repositorioPerfiles = repositorioPerfiles;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacionViewModel)
        {
            var perfiles = await repositorioPerfiles.ListarPerfilesPaginacion(paginacionViewModel);
            var totalPerfiles = await repositorioPerfiles.ContarPerfiles();
            var respuestaVM = new PaginacionRespuesta<Perfile>
            {
                Elementos = perfiles,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalPerfiles,
                BaseURL = "/Perfiles"
            };
            return View(respuestaVM);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Perfile perfile)
        {
            perfile.Nombre.ToUpper();
            await repositorioPerfiles.Crear(perfile);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int Id)
        {
            var perfile = await repositorioPerfiles.GetPerfileById(Id);
            if (perfile is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }   
            return View(perfile);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Perfile perfileNew)
        {
            var perfile = await repositorioPerfiles.GetPerfileById(perfileNew.Id);
            if (perfile is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioPerfiles.Editar(perfileNew);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var perfile = await repositorioPerfiles.GetPerfileById(Id);
            return PartialView("BorrarPartialView", perfile);
        }


        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(Perfile perfile)
        {
            var perfileToBorrar = await repositorioPerfiles.GetPerfileById(perfile.Id);
            if (perfileToBorrar is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioPerfiles.Borrar(perfileToBorrar.Id);

            //await servicioEmailSendGrid.Actualizar(operacionNew);
            return RedirectToAction("Index", "Perfiles");

        }
    }
}
