using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class ProductosController :Controller
    {

        private readonly IRepositorioProductos repositorioProductos;
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IRepositorioReglas repositorioReglas;
        private readonly IMapper mapper;

        public ProductosController(
            IRepositorioProductos repositorioProductos,
            IRepositorioCategorias repositorioCategorias,
            IRepositorioReglas repositorioReglas,
            IMapper mapper
            )
        {
            this.repositorioProductos = repositorioProductos;
            this.repositorioCategorias = repositorioCategorias;
            this.repositorioReglas = repositorioReglas;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacionViewModel)
        {

            var productos = await repositorioProductos.ListarProductosPaginacion(paginacionViewModel);
            var totalProductos = await repositorioProductos.ContarProductos();
            var respuestaVM = new PaginacionRespuesta<Producto>
            {
                Elementos = productos,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalProductos,
                BaseURL = "/Productos"
            };
            return View(respuestaVM);
        }


        [HttpGet]
        public  async Task<IActionResult> Crear()
        {
            var modelo = new ProductoCreacionViewModel();
            modelo.categorias =await SelectCategorias();
            modelo.reglas = await SelectReglas();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ProductoCreacionViewModel producto)
        {           
            var categoria = await repositorioCategorias.GetCategoriaById(producto.IdCategoria);
            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
            if (regla is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                producto.categorias = await SelectCategorias();
                producto.reglas = await SelectReglas();
                return View(producto);
            }

            producto.Nombre.ToUpper();
            await repositorioProductos.Crear(producto);
            return RedirectToAction("Index");
            
        }


    public async Task<IActionResult> Editar(int Id)
        {
            var producto = await repositorioProductos.GetProductoById(Id);

            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<ProductoCreacionViewModel>(producto);
            
            //replaced by mapper
            // new ProductoCreacionViewModel()
            //{
            //    Id=producto.Id,
            //    Nombre=producto.Nombre,
            //    IdCategoria=producto.IdCategoria,
            //    IdRegla=producto.IdRegla,
            //};

            modelo.categorias = await SelectCategorias();
            modelo.reglas = await SelectReglas();
            return View(modelo);
        }

        [HttpPost]
    public async Task<IActionResult> Editar (ProductoCreacionViewModel productoNew)
        {
            var producto = await repositorioProductos.GetProductoById(productoNew.Id);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var categoria = await repositorioCategorias.GetCategoriaById(productoNew.IdCategoria);
            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var regla = await repositorioReglas.GetReglaById(productoNew.IdRegla);
            if (regla is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioProductos.Editar(productoNew);
            return RedirectToAction("Index");

        }


    

        public async Task<IEnumerable<SelectListItem>> SelectCategorias()
        {
            var categorias = await repositorioCategorias.ListarCategorias();
            return categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

    public async Task<IEnumerable<SelectListItem>> SelectReglas()
        {
            var reglas = await repositorioReglas.ListarReglas();
            return reglas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var producto= await repositorioProductos.GetProductoById(Id);
            return PartialView("BorrarPartialView", producto);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(Producto producto)
        {
            var productoToBorrar = await repositorioProductos.GetProductoById(producto.Id);
            if (productoToBorrar is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioProductos.Borrar(productoToBorrar.Id);
            return RedirectToAction("Index","Productos");
        }


    }
}
