
using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class AsignacionesController:Controller
    {
        private readonly IRepositorioAsignaciones repositorioAsignaciones;
        private readonly IRepositorioColaboradores repositorioColaboradores;
        private readonly IRepositorioEmpresas repositorioEmpresas;
        private readonly IRepositorioProyectos repositorioProyectos;
        private readonly IMapper mapper;

        public AsignacionesController (
            IRepositorioAsignaciones repositorioAsignaciones,
            IRepositorioColaboradores repositorioColaboradores,
            IRepositorioEmpresas repositorioEmpresas,
            IRepositorioProyectos repositorioProyectos,
            IMapper mapper
            )
        {
            this.repositorioAsignaciones = repositorioAsignaciones;
            this.repositorioColaboradores = repositorioColaboradores;
            this.repositorioEmpresas = repositorioEmpresas;
            this.repositorioProyectos = repositorioProyectos;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var asignaciones=await repositorioAsignaciones.ListarAsignaciones();
            return View(asignaciones);
        }


        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var modelo = new AsignacionCreacionViewModel();
            modelo.colaboradores = await SelectColaboradores();
            modelo.empresas = await SelectEmpresas();
          //  modelo.proyectos= await SelectProyectos();
            modelo.proyectos = await GetProyectosByIdEmpresa(modelo.enumId);

            return View(modelo);
        
        }

        private async Task<IEnumerable<SelectListItem>> SelectColaboradores()
        {
            var colaboradores = await repositorioColaboradores.ListarColaboradores();
            return colaboradores.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> SelectEmpresas()
        {
            var empresas = await repositorioEmpresas.ListarEmpresas();
            return empresas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> SelectProyectos()
        {
            var proyectos = await repositorioProyectos.ListarProyectos();
            return proyectos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }



        private async Task<IEnumerable<SelectListItem>> GetProyectosByIdEmpresa(EmpresaEnum IdEmpresa)
        {

            var proyectos = await repositorioProyectos.GetProyectosByIdEmpresa(IdEmpresa);
            return proyectos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProyectosByIdEmpresa([FromBody] EmpresaEnum empresaEnum)
        {

            var proyectos = await GetProyectosByIdEmpresa(empresaEnum);
            return Ok(proyectos);
        }

        public async Task<IActionResult> Editar(int Id)
        {
            var asignacion = await repositorioAsignaciones.GetAsignacionById(Id);
            if (asignacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<AsignacionCreacionViewModel>(asignacion);
            modelo.colaboradores = await SelectColaboradores();
            modelo.empresas = await SelectEmpresas();
            modelo.proyectos = await GetProyectosByIdEmpresa(modelo.enumId);
            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(AsignacionCreacionViewModel asignacionNew)
        {
            var asignacion = await repositorioAsignaciones.GetAsignacionById(asignacionNew.Id);
            if (asignacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var colaborador = await repositorioColaboradores.GetColaboradorById(asignacionNew.IdColaborador);
            if (colaborador is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var empresa = await repositorioEmpresas.GetEmpresaById(asignacionNew.IdEmpresa);
            if (empresa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var proyecto = await repositorioProyectos.GetProyectoById(asignacionNew.IdProyecto);
            if (proyecto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioAsignaciones.Editar(asignacionNew);
            return RedirectToAction("Index");
        }


    }
}
