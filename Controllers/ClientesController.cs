using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [AllowAnonymous]
    public class ClientesController : Controller
    {
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IRepositorioEmpresas repositorioEmpresas;
        private readonly IMapper mapper;

        public ClientesController(
            IRepositorioClientes repositorioClientes,
            IRepositorioEmpresas repositorioEmpresas,
            IMapper mapper)
        {
            this.repositorioClientes = repositorioClientes;
            this.repositorioEmpresas = repositorioEmpresas;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await repositorioClientes.ListarClientes();
            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {

            var modelo = new ClienteCreacionViewModel();
            modelo.empresas = await SelectEmpresas();
            return View(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Crear(ClienteCreacionViewModel cliente)
        {

            var empresa = await repositorioEmpresas.GetEmpresaById(cliente.IdEmpresa);
            if (empresa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            if (!ModelState.IsValid)
            {
                cliente.empresas = await SelectEmpresas();
                return View(cliente);
            }


            await repositorioClientes.Crear(cliente);
            return RedirectToAction("Index");
        }

        public async Task<IEnumerable<SelectListItem>> SelectEmpresas()
        {
            var empresas = await repositorioEmpresas.ListarEmpresas();
            return empresas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IActionResult> Editar(int Id)
        {
            var cliente = await repositorioClientes.GetClienteById(Id);
            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<ClienteCreacionViewModel>(cliente);
            modelo.empresas = await SelectEmpresas();
            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(ClienteCreacionViewModel clienteNew)
        {
            var cliente=await repositorioClientes.GetClienteById(clienteNew.Id);
            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var empresa = await repositorioEmpresas.GetEmpresaById(clienteNew.IdEmpresa);
            if (empresa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioClientes.Editar(clienteNew);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var cliente = await repositorioClientes.GetClienteById(Id);
            return PartialView("BorrarPartialView", cliente);
        }


        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(Cliente cliente)
        {
            var ClienteToBorrar = await repositorioClientes.GetClienteById(cliente.Id);
            if (ClienteToBorrar is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioClientes.Borrar(ClienteToBorrar.Id);

            //await servicioEmailSendGrid.Actualizar(operacionNew);
            return RedirectToAction("Index", "Clientes");

        }




    }
}
