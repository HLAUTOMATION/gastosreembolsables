using DocumentFormat.OpenXml.Office2010.Excel;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class EmpresasController :Controller
    {

        private readonly IRepositorioEmpresas repositorioEmpresas;

        public EmpresasController(IRepositorioEmpresas repositorioEmpresas)
        {
            this.repositorioEmpresas = repositorioEmpresas;
        }

        public async Task<IActionResult> Index()
        {
            var empresas= await repositorioEmpresas.ListarEmpresas();
            return View(empresas);
        }


        [HttpGet]
        public async Task<IActionResult> Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return View(empresa);
            }

            empresa.Nombre.ToUpper();
            await repositorioEmpresas.Crear(empresa);
            return RedirectToAction("Index");
        }

        public  async Task<IActionResult> Editar(int Id)
        {
            var empresa = await repositorioEmpresas.GetEmpresaById(Id);
            if (empresa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Empresa empresaNew)
        {
            var empresa = await repositorioEmpresas.GetEmpresaById(empresaNew.Id);
            if (empresa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioEmpresas.Editar(empresaNew);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var empresa = await repositorioEmpresas.GetEmpresaById(Id);
            return PartialView("BorrarPartialView", empresa);
        }


        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(Empresa empresa)
        {
            var empresaToBorrar = await repositorioEmpresas.GetEmpresaById(empresa.Id);
            if (empresaToBorrar is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioEmpresas.Borrar(empresaToBorrar.Id);

            //await servicioEmailSendGrid.Actualizar(operacionNew);
            return RedirectToAction("Index", "Empresas");

        }
    }
}
