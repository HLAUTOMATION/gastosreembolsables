using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {

        private readonly IRepositorioCategorias repositorioCategorias;

        public CategoriasController(IRepositorioCategorias repositorioCategorias)
        {
            this.repositorioCategorias = repositorioCategorias;
        }

        public async Task<IActionResult> Index()
        {

            var clientes = await repositorioCategorias.ListarCategorias();
            return View(clientes);
        }


        [HttpGet]
        public async Task<IActionResult> Crear()
        {
   
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.Nombre.ToUpper();
            await repositorioCategorias.Crear(categoria);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Editar(int Id)
        {
            var categoria = await repositorioCategorias.GetCategoriaById(Id);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoriaNew)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaNew);
            }
            
            var categoria = await repositorioCategorias.GetCategoriaById(categoriaNew.Id);
            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCategorias.Editar(categoriaNew);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var categoria = await repositorioCategorias.GetCategoriaById(Id);
            return PartialView("BorrarPartialView", categoria);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(Categoria categoria)
        {
            var categoriaToBorrar = await repositorioCategorias.GetCategoriaById(categoria.Id);
                if (categoriaToBorrar is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }
                await repositorioCategorias.Borrar(categoria.Id);
                return RedirectToAction("Index", "Categorias");   
            }
        }
    }

