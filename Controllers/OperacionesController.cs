using AutoMapper;
using Azure.Security.KeyVault.Certificates;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class OperacionesController : Controller

    {



        private readonly IRepositorioOperaciones repositorioOperaciones;
        private readonly IRepositorioColaboradores repositorioColaboradores;
        private readonly IRepositorioProductos repositorioProductos;
        private readonly IRepositorioReglas repositorioReglas;
        private readonly IRepositorioUsuarios repositorioUsuarios;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioAppUsuarios repositorioAppUsuarios;
        private readonly IServicioEmailSendGrid servicioEmailSendGrid;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext applicationDbContext;

        public IRepositorioUsuarios RepositorioUsuarios => repositorioUsuarios;

        public OperacionesController(
            IRepositorioOperaciones repositorioOperaciones,
            IRepositorioColaboradores repositorioColaboradores,
            IRepositorioProductos repositorioProductos,
            IRepositorioReglas repositorioReglas,
            IRepositorioUsuarios repositorioUsuarios,
            IServicioUsuarios servicioUsuarios,
            IRepositorioAppUsuarios repositorioAppUsuarios,
            IServicioEmailSendGrid servicioEmailSendGrid,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            ApplicationDbContext applicationDbContext)
        {
            this.repositorioOperaciones = repositorioOperaciones;
            this.repositorioColaboradores = repositorioColaboradores;
            this.repositorioProductos = repositorioProductos;
            this.repositorioReglas = repositorioReglas;
            this.repositorioUsuarios = repositorioUsuarios;
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioAppUsuarios = repositorioAppUsuarios;
            this.servicioEmailSendGrid = servicioEmailSendGrid;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
            this.applicationDbContext = applicationDbContext;
        }


        //public async Task<IActionResult> Index(string searchValue, PaginacionViewModel paginacionViewModel)
        //{

        //    var operaciones = await repositorioOperaciones.ListarOperacionesPendientesPaginacion(paginacionViewModel);
        //    if (operaciones is null)
        //    {
        //        return RedirectToAction("NoEncontrado", "Home");
        //    }
        //    var totalOperaciones = await repositorioOperaciones.ContarOperacionesPendientes();
        //    var respuestaVM = new PaginacionRespuesta<Operacion>
        //    {
        //        Elementos = operaciones,
        //        Pagina = paginacionViewModel.Pagina,
        //        RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
        //        CantidadTotalRecords = totalOperaciones,
        //        BaseURL = "/Operaciones"
        //    };

        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        operaciones = operaciones.Where(operacion => operacion.Id.ToString().Equals(searchValue));
        //    }

        //    return View(respuestaVM);
        //}

        public async Task<IActionResult> Index(string searchValue, PaginacionViewModel paginacionViewModel)
        {
            var totalOperaciones=0;
            PaginacionRespuesta<Operacion> respuestaVM;

            var operaciones = await repositorioOperaciones.ListarOperacionesPendientesPaginacion(paginacionViewModel);
            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
             totalOperaciones = await repositorioOperaciones.ContarOperacionesPendientes();
             respuestaVM = new PaginacionRespuesta<Operacion>
            {
                Elementos = operaciones,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalOperaciones,
                BaseURL = "/Operaciones"
            };

            if (!string.IsNullOrEmpty(searchValue))
            {
                operaciones = operaciones.Where(operacion => operacion.Id.ToString().Equals(searchValue));
                totalOperaciones = 1;
                respuestaVM = new PaginacionRespuesta<Operacion>
                {
                    Elementos = operaciones,
                    Pagina = paginacionViewModel.Pagina,
                    RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                    CantidadTotalRecords = totalOperaciones,
                    BaseURL = "/Operaciones"
                };
            }

            return View(respuestaVM);
        }


        [HttpPost]
        public async Task<IActionResult> Index(int Id)
        {
            var operaciones = await repositorioOperaciones.ListarOperaciones();
            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            if (!String.IsNullOrEmpty(Id.ToString()))
            {
                operaciones = operaciones.Where(operacion => operacion.Id == Id);
            }

            return View(operaciones);
        }

        public async Task<IActionResult> ListarOperacionesReactivadas()
        {

            var operaciones = await repositorioOperaciones.ListarOperacionesReactivadas();
            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(operaciones);
        }

        public async Task<IActionResult> GetOperacionesReactivadasByIdOperacion(int Id)
        {

            var operaciones = await repositorioOperaciones.GetOperacionesReactivadasByIdOperacion(Id);
            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(operaciones);
        }

        //public async Task<IActionResult> Historia(string searchValue, PaginacionViewModel paginacionViewModel)
        //{
        //    var operaciones = await repositorioOperaciones.ListarOperacionesHistoricasPaginacion(paginacionViewModel);
        //    if (operaciones is null)
        //    {
        //        return RedirectToAction("NoEncontrado", "Home");
        //    }
        //    var totalOperaciones = await repositorioOperaciones.ContarOperacionesHistoricas();
        //    var respuestaVM = new PaginacionRespuesta<Operacion>
        //    {
        //        Elementos = operaciones,
        //        Pagina = paginacionViewModel.Pagina,
        //        RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
        //        CantidadTotalRecords = totalOperaciones,
        //        BaseURL = "/Operaciones/Historia"
        //    };
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        operaciones = operaciones.Where(operacion => operacion.Id.ToString().Equals(searchValue));
        //    }

        //    foreach (var operacion in operaciones)
        //    {
        //        var operacionesreactivadas = await repositorioOperaciones.GetOperacionesReactivadasByIdOperacion(operacion.Id);
        //        operacion.operacionesReactivadas = operacionesreactivadas;
        //    }

        //    return View(respuestaVM);
        //}


        public async Task<IActionResult> Historia(string searchValue, PaginacionViewModel paginacionViewModel)
        {
            var totalOperaciones = 0;
            PaginacionRespuesta<Operacion> respuestaVM;

            var operaciones = await repositorioOperaciones.ListarOperacionesHistoricasPaginacion(paginacionViewModel);
            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
             totalOperaciones = await repositorioOperaciones.ContarOperacionesHistoricas();
             respuestaVM = new PaginacionRespuesta<Operacion>
            {
                Elementos = operaciones,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalOperaciones,
                BaseURL = "/Operaciones/Historia"
            };
            if (!string.IsNullOrEmpty(searchValue))
            {
                operaciones = operaciones.Where(operacion => operacion.Id.ToString().Equals(searchValue));
                totalOperaciones = 1;
                respuestaVM = new PaginacionRespuesta<Operacion>
                {
                    Elementos = operaciones,
                    Pagina = paginacionViewModel.Pagina,
                    RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                    CantidadTotalRecords = totalOperaciones,
                    BaseURL = "/Operaciones/Historia"
                };

            }

            foreach (var operacion in operaciones)
            {
                var operacionesreactivadas = await repositorioOperaciones.GetOperacionesReactivadasByIdOperacion(operacion.Id);
                operacion.operacionesReactivadas = operacionesreactivadas;
            }

            return View(respuestaVM);
        }

        //public async Task<IActionResult> GetOperacionesByIdColaborador()
        //{


        //    var IdUsuario=servicioUsuarios.GetIdUsuario();

        //    var operaciones = await repositorioOperaciones.GetOperacionesByIdUsuario(IdUsuario);
        //    return View(operaciones);
        //}


        public async Task<IActionResult> GetOperacionesByCorreoUsuario(PaginacionViewModel paginacionViewModel)
        {
            var correoAppUsuario = servicioUsuarios.GetADUserEmail().ToUpper();
            var operaciones = await repositorioOperaciones.GetOperacionesByCorreoUsuarioPaginacion(correoAppUsuario, paginacionViewModel);

            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var totalOperaciones = await repositorioOperaciones.ContarOperacionesPendientesByCorreoUsuario(correoAppUsuario);

            var respuestaVM = new PaginacionRespuesta<Operacion>
            {

                Elementos = operaciones,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalOperaciones,
                BaseURL = "/Operaciones/GetOperacionesByCorreoUsuario",
                appUsuario = await repositorioAppUsuarios.GetAppUsuarioByEmail(correoAppUsuario)
            };
            if (operaciones != null)
            {
                return View(respuestaVM);
            }
            else
            {
                return View("No hay solicitudes");
            }

        }

        public async Task<IActionResult> GetOperacionesReactivadasByCorreoUsuario()
        {
            var correoAppUsuario = servicioUsuarios.GetADUserEmail().ToUpper();
            var operaciones = await repositorioOperaciones.GetOperacionesReactivadasByCorreoUsuario(correoAppUsuario);
            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            if (operaciones != null)
            {
                return View(operaciones);
            }
            else
            {
                return View("No hay solicitudes");
            }

        }

        public async Task<AppUsuario> GetAppUsuarioByEmail()
        {
            var emailNormalizado = servicioUsuarios.GetADUserEmail().ToUpper();
            var appUsuario = repositorioAppUsuarios.GetAppUsuarioByEmail(emailNormalizado);


            return await appUsuario;
        }



        public async Task<IActionResult> HistoriaByIdAppUsuario(PaginacionViewModel paginacionViewModel)

        {
            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var operaciones = await repositorioOperaciones.GetOperacionesHistoricasByCorreoUsuarioPaginacion(CorreoUsuario, paginacionViewModel);
            if (operaciones is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var totalOperaciones = await repositorioOperaciones.ContarOperacionesHistoricasByCorreoUsuario(CorreoUsuario);
            var respuestaVM = new PaginacionRespuesta<Operacion>
            {
                Elementos = operaciones,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalOperaciones,
                BaseURL = "/Operaciones/HistoriaByIdAppUsuario"
            };
            foreach (var operacion in operaciones)
            {
                var operacionesreactivadas = await repositorioOperaciones.GetOperacionesReactivadasByIdOperacion(operacion.Id);
                operacion.operacionesReactivadas = operacionesreactivadas;
            }

            return View(respuestaVM);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var modelo = new OperacionCreacionViewModel();
            modelo.productos = await SelectProductos();
            modelo.reglas = await repositorioReglas.ListarReglas();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(OperacionCreacionViewModel operacion)
        {

            //  string FileName = UploadFile(operacion);

            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
            var tope = regla.Tope;
            var appUsuario = await repositorioAppUsuarios.GetAppUsuarioByEmail(CorreoUsuario);
            operacion.productos = await SelectProductos();
            operacion.reglas = await repositorioReglas.ListarReglas();

            if (appUsuario is null)
            {
                return RedirectToAction("UsuarioNoExiste", "Home");
            }

            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                
                return View(operacion);
            }


            if (await Validacion(operacion))
            {

                operacion.CorreoUsuario = CorreoUsuario;
                operacion.FechaCreacion = DateTime.Now;
                operacion.yearmonth = String.Concat(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                if (DateTime.Now.Month.ToString().Length < 2)
                {
                    operacion.yearmonth = String.Concat(DateTime.Now.Year.ToString(), "0"+DateTime.Now.Month.ToString());
                }
                else
                {
                    operacion.yearmonth = String.Concat(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                }

                UploadFile(operacion);

                operacion.Id = await repositorioOperaciones.Crear(operacion);
                if (string.IsNullOrEmpty(operacion.Id.ToString()))
                {
                    return RedirectToAction("OperacionFallida", "Home");
                }

                await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnNuevaSolicitud(operacion);

                await servicioEmailSendGrid.Solicitar(operacion);
                //return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");
                TempData["AlertForBlock"] = "Felicitación! " +
                    "La solicitud con ID "+$"{operacion.Id } "+"  esta creada exitosamente. El tiempo de espera de procedimiento esta estimado a 3 dias laborales";
                return View("Creada", operacion);

            }
            else

            {


                operacion.UltimoOperaciondelProductoDelUsuario = await repositorioOperaciones.GetLastOperacionByCorreoUsuarioAndIdProducto(CorreoUsuario, operacion.IdProducto);
                if (regla.Id == 9)
                {
                    
                    
                    TempData["AlertForBlock"] =
                        "la solicitud no esta autorizada. "
                        + "El total del costo de categoria consumo oficina a superado el tope.  "
                        + "El total de sus solicitudes de categoria consumo oficina para este mes es de:" + $"{await repositorioOperaciones.GetCurrentMonthTotalDeOperacionesConsumoOficinaDelColaborador(CorreoUsuario, operacion.IdProducto, regla.Id)}";

                    return View("Crear", operacion);
                    //return RedirectToAction("Crear", "Operaciones");                    
                }
                else if (regla.Id == 1)
                {
                    if (operacion.Cantidad>1)
                    {
                        TempData["AlertForBlock"] =
                         "la solicitud no esta autorizada. Este producto se puede solicitar solamente 1 unidad por vez ";
                    }
                    else
                    {
                        TempData["AlertForBlock"] =
                             "la solicitud no esta autorizada. Habia solicitado un reembolso de telefono dentro de espacio de 2 anos. "
                         + "Su ultima solicitud para este mismo producto fue creado el " + $"{operacion.UltimoOperaciondelProductoDelUsuario.FechaCreacion}. ";
                    }
                    return View("Crear", operacion);
                    //return RedirectToAction("Crear", "Operaciones");
                }
                else if (regla.Id == 3 || regla.Id == 4 || regla.Id == 5 || regla.Id == 6 || regla.Id == 7 || regla.Id == 8 || regla.Id == 11 || regla.Id == 12)
                {
                    if (operacion.Cantidad > 1)
                    {
                        TempData["AlertForBlock"] =
                         "la solicitud no esta autorizada. Este producto se puede solicitar solamente 1 unidad por vez ";
                    }
                    else { 
                    TempData["AlertForBlock"] =
                         "la solicitud no esta autorizada. Este tipo de producto se solicita solamente una vez. "
                         + "Su ultima solicitud para este mismo producto fue creado el " + $"{operacion.UltimoOperaciondelProductoDelUsuario.FechaCreacion}. ";
                    }
                    return View("Crear", operacion);
                    //return RedirectToAction("Crear", "Operaciones");


                }
                // return View(operacion);
                return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");

            }
        }




        private string UploadFile(OperacionCreacionViewModel operacion)
        {
            // string fileName = null;
            string path = null;
            string fileName = null;
            string extension = null;
            if (operacion.Documento != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string[] delimiters = { "/", " ", ":" };
                string[] array = DateTime.Now.ToString().Split(delimiters, StringSplitOptions.None);
                string str = string.Join("_", array);
                fileName = Path.GetFileNameWithoutExtension(operacion.Documento.FileName) + str;


                extension = Path.GetExtension(operacion.Documento.FileName);
                //operacion.UrlDocumento = "~/folder/" + operacion.Documento.FileName;
                operacion.UrlDocumento = "~/folder/" + fileName + extension;
                //path =Path.Combine(wwwRootPath+ @"\folder\"+ operacion.Documento.FileName);
                path = Path.Combine(wwwRootPath + @"\folder\" + fileName + extension);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    operacion.Documento.CopyTo(fileStream);
                }




                //string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "folder");
                //fileName=Guid.NewGuid().ToString()+"-"+operacion.Documento.FileName;
                //string filePath=Path.Combine(uploadDir, fileName);
                //using (var fileStream=new FileStream(filePath, FileMode.Create))
                //{
                //    operacion.Documento.CopyTo(fileStream);
                //}
            }
            return path;
        }

        public async Task<IActionResult> Editar(int Id)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }



            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
            modelo.productos = await SelectProductos();
            return View(modelo);
        }

        public async Task<IActionResult> EditarOperacionReactivada(int Id)
        {
            var operacion = await repositorioOperaciones.GetOperacionReactivadaById(Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
            modelo.productos = await SelectProductos();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(OperacionCreacionViewModel operacionNew)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(operacionNew.Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            if (operacionNew.Documento != null)
            {
                UploadFile(operacionNew);
            }
            else
            {
                operacionNew.UrlDocumento = operacion.UrlDocumento;
            }


            await repositorioOperaciones.Editar(operacionNew);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnEditarSolicitud(operacion, operacionNew);
            var emailNormalizado = servicioUsuarios.GetADUserEmail().ToUpper();

            await servicioEmailSendGrid.Actualizar(operacionNew);


            return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");

        }

        [HttpPost]
        public async Task<IActionResult> EditarOperacionReactivada(OperacionCreacionViewModel operacionNew)
        {

            var operacionReactivada = await repositorioOperaciones.GetOperacionReactivadaById(operacionNew.Id);
            if (operacionReactivada is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var producto = await repositorioProductos.GetProductoById(operacionReactivada.IdProducto);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (operacionNew.Documento != null)
            {
                UploadFile(operacionNew);
            }
            else
            {
                operacionNew.UrlDocumento = operacionReactivada.UrlDocumento;
            }


            operacionReactivada.Descripcion = operacionNew.Descripcion;
            operacionReactivada.FechaCompra = operacionNew.FechaCompra;

            await repositorioOperaciones.EditarOperacionReactivada(operacionReactivada);
            await servicioEmailSendGrid.Actualizar(operacionReactivada);

            return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");

        }

        [HttpPost]
        public async Task<IActionResult> Borrar(OperacionCreacionViewModel operacionNew)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(operacionNew.Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioOperaciones.Borrar(operacionNew.Id);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnBorrarSolicitud(operacion);
            //await servicioEmailSendGrid.Actualizar(operacionNew);
            return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");

        }

        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(Id);
            return PartialView("BorrarOperacionPartialView", operacion);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreacionBloqueadaPartialView(Operacion operacion)
        //{           
        //    return PartialView("CreacionBloqueadaPartialView",operacion);
        //}


        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(OperacionCreacionViewModel operacionNew)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(operacionNew.Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioOperaciones.Borrar(operacionNew.Id);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnBorrarSolicitud(operacion);
            //await servicioEmailSendGrid.Actualizar(operacionNew);
            return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");

        }

        //[HttpPost]
        //public async Task<IActionResult> AprobarPartialView(OperacionCreacionViewModel operacionNew)
        //{
        //    var operacion = await repositorioOperaciones.GetOperacionById(operacionNew.Id);
        //    if (operacion is null)
        //    {
        //        return RedirectToAction("NoEncontrado", "Home");
        //    }

        //    await repositorioOperaciones.Borrar(operacionNew.Id);
        //    await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnBorrarSolicitud(operacion);
        //    //await servicioEmailSendGrid.Actualizar(operacionNew);
        //    return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");

        //}



        public async Task<IActionResult> Reactivar(int Id)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
            modelo.productos = await SelectProductos();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Reactivar(OperacionCreacionViewModel operacionNew)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(operacionNew.Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                operacionNew.productos = await SelectProductos();
                return View(operacion);
            }
            var ifThereisOperacionReactivadaPendiente = false;
            var operacionesReactivadas = await repositorioOperaciones.GetOperacionesReactivadasByIdOperacion(operacionNew.IdOperacion);
            if (operacionesReactivadas.Count() > 0)
            {
                foreach (Operacion item in operacionesReactivadas)
                {
                    if (item.Estado == 3)
                    {
                        ifThereisOperacionReactivadaPendiente = true;

                    }
                    
                }
            }

            if (ifThereisOperacionReactivadaPendiente)
            {
                return RedirectToAction("HistoriaByIdAppUsuario", "Operaciones");

            }
            else {
                var emailNormalizado = servicioUsuarios.GetADUserEmail().ToUpper();
                operacionNew.CorreoUsuario = emailNormalizado;
                operacionNew.IdOperacion = operacion.Id;
                operacionNew.yearmonth = operacion.yearmonth;
                operacionNew.FechaCreacion = operacion.FechaCreacion;

                //    var modelo = mapper.Map<Operacion>(operacion);

                if (operacionNew.Documento != null)
                {
                    UploadFile(operacionNew);
                }
                else
                {
                    operacionNew.UrlDocumento = operacion.UrlDocumento;
                }

                await repositorioOperaciones.CrearOperacionReactivada(operacionNew);
                await repositorioOperaciones.Reactivar(operacion);


                await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnSolicitudreactivada(operacion);
                await servicioEmailSendGrid.Solicitar(operacionNew);


                return RedirectToAction("GetOperacionesByCorreoUsuario", "Operaciones");
            }
        }

        public async Task<IActionResult> Procesar(int Id)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(Id);

            

            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

             if (operacion.Estado == 1 || operacion.Estado == 2 || operacion.Estado==4)
            {
                return RedirectToAction("Procesado");
            }

            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
            var tope = regla.Tope;

            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
            modelo.productos = await SelectProductos();
            modelo.reglas = await repositorioReglas.ListarReglas();

            if (tope==0)
            {
                modelo.MontoReembolsado = operacion.TotalCosto;
            }
           else if (modelo.TotalCosto<tope)
            {
            modelo.MontoReembolsado = operacion.TotalCosto;

            }
            else
            {
                modelo.MontoReembolsado = tope;
            }
            return View(modelo);
        }

        public async Task<IActionResult> ProcesarOperacionReactivada(int Id)

        {
            var operacion = await repositorioOperaciones.GetOperacionReactivadaById(Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);

            modelo.productos = await SelectProductos();
            return View(modelo);
        }




        public async Task<IActionResult> OperacionNotValidado(Operacion operacion)
        {

            return View(operacion);

        }

        public async Task<IEnumerable<SelectListItem>> SelectProductos()
        {
            var productos = await repositorioProductos.ListarProductos();
            return productos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        //are operacion check

        public async Task<bool> Validacion(OperacionCreacionViewModel operacion)
        {
            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
            var tope = regla.Tope;

            var LatestOperacionOfProducto = await repositorioOperaciones.GetLastOperacionByCorreoUsuarioAndIdProducto(CorreoUsuario, operacion.IdProducto);
            switch (regla.Id)
            {

                case 1:       // 1 times per 2 year
                    if (operacion.Cantidad > 1)
                    {
                        return false;
                    }
                    if (!await CheckUsuarioProductoExiste(operacion)) { return true; }                    
                    else if (await CheckUsuarioProductoExiste(operacion) && (DateTime.Now - LatestOperacionOfProducto.FechaCreacion).TotalDays / 365 > 2) { return true; }
                    else if (await CheckUsuarioProductoExiste(operacion) && (DateTime.Now - LatestOperacionOfProducto.FechaCreacion).TotalDays / 365 <= 2) { return false; }
                    else { return false; }
                    break;
                case 3: case 4: case 5: case 6: case 7: case 8: case 11: case 12: //only one time
                    if (operacion.Cantidad>1)
                    {
                        return false;
                    }
                    if (await CheckUsuarioProductoExiste(operacion)) { return false; } else { return true; }
                    break;
                case 9: //6000 per month
                    if (!await CheckUsuarioProductoExiste(operacion)) { return true; } else
                    {
                        var sum = await repositorioOperaciones.GetCurrentMonthTotalDeOperacionesConsumoOficinaDelColaborador(CorreoUsuario, operacion.IdProducto, producto.IdRegla);

                        if (sum < tope) { return true; } else { return false; };
                    }

                    break;
                case 10: case 13: case 18: case 20:// manual
                    return true;
                    break;

                default:
                    return false;
                    break;
            }

        }


        public async Task<Regla> GetReglaByIdProdyctoFromOperacionCreacion(OperacionCreacionViewModel operacion)
        {
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            return await repositorioReglas.GetReglaById(producto.IdRegla);
        }

        public async Task<bool> CheckUsuarioProductoExiste(OperacionCreacionViewModel operacion)
        {
            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var operaciones = await repositorioOperaciones.GetOperacionesPendientesAprobadasOnHoldReactivadasByCorreoUsuario(CorreoUsuario);
            var idsProductoOfOperaciones = operaciones.Select(item => item.IdProducto).ToList();
            if (idsProductoOfOperaciones.Contains(operacion.IdProducto))
            {
                return true;
            }
            else
            {
                return false;
            }

            //var boolean = true;

            //foreach (var item in operaciones)
            //{
            //    //to make the loop check every element, have so creat a local variable,
            //    // then asign a value(true or false). 
            //    // not to just return true or return false, or the loop will stop from the first step
            //    if (!(item.IdProducto==operacion.IdProducto))
            //    {
            //        boolean= true;
            //    }
            //    else
            //    {
            //        boolean = false;
            //    }                           
            //}
            //return boolean;


        }

        public async Task<bool> CheckAntiguedad(OperacionCreacionViewModel operacion)
        {
            var usuario = await GetAppUsuarioByEmail();

            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
            if (usuario.Antiguedad > regla.Antiguedad)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> CalcularPeriodoFromLastOperacionIfExiste(OperacionCreacionViewModel operacion)
        {

            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var LatestOperacionOfProducto = await repositorioOperaciones.GetLastOperacionByCorreoUsuarioAndIdProducto(CorreoUsuario, operacion.IdProducto);

            int periodo = 12 * (DateTime.Now.Year - LatestOperacionOfProducto.FechaCreacion.Year) + DateTime.Now.Month - LatestOperacionOfProducto.FechaCreacion.Month;
            periodo = Math.Abs(periodo);

            return periodo;

        }

        public async Task<bool> CheckVecesPorPeriodo(OperacionCreacionViewModel operacion)
        {
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
            var periodoOperacion = await CalcularPeriodoFromLastOperacionIfExiste(operacion);
            var boolean = true;

            if (periodoOperacion == 0)
            {
                return false;
            }
            else
            {
                if (regla != null)
                    try
                    {

                        var vecesPorPeriodoOperacion = 1 / await CalcularPeriodoFromLastOperacionIfExiste(operacion);
                        var vecesPorPeriodoRelga = 1 / await CalcularPeriodoFromLastOperacionIfExiste(operacion);
                        if (vecesPorPeriodoRelga > vecesPorPeriodoOperacion)
                        {
                            boolean = true;
                        }
                        else
                        {
                            boolean = false;
                        }
                    }
                    catch (DivideByZeroException e)
                    {
                        Console.WriteLine("Exception caught: {0}", e);
                    }
                    finally
                    {
                        Console.WriteLine("Result: {0}");
                    }
            }




            return boolean;
        }


        //public async Task<IActionResult> Aprobar(int Id)
        //{
        
        //    var operacion = await repositorioOperaciones.GetOperacionById(Id);
        //    if (operacion is null)
        //    {
        //        return RedirectToAction("NoEncontrado", "Home");
        //    }

        //    var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
        //    modelo.productos = await SelectProductos();
        //    return View(modelo);
        //}

        public async Task<IActionResult> OperacionAprobada(OperacionCreacionViewModel operacion)
        {

            return View(operacion);
        }

        public async Task<IActionResult> Rechazado(OperacionCreacionViewModel operacion)
        {

            return View(operacion);
        }

        public async Task<IActionResult> Procesado(Operacion operacion)
        {

            return View(operacion);
        }

        [HttpPost]
        public async Task<IActionResult> Aprobar(OperacionCreacionViewModel operacionToAprobar)
        {
           
            var operacion = await repositorioOperaciones.GetOperacionById(operacionToAprobar.Id);
            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
           
            

            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
            var tope = regla.Tope;
            modelo.productos = await SelectProductos();
            modelo.MontoReembolsado = operacionToAprobar.MontoReembolsado;
            modelo.Comentario = operacionToAprobar.Comentario;
            modelo.CorreoAdministrador = servicioUsuarios.GetADUserEmail();
            modelo.reglas = await repositorioReglas.ListarReglas();


            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            if (regla.Id == 9)
            {
                var montoreembolsado = await repositorioOperaciones.GetTotalConsomoOficinaAprobadoByMonthByUsuario(operacion, DateTime.Now);
                if ((montoreembolsado + operacionToAprobar.MontoReembolsado) == 6000 || (montoreembolsado + operacionToAprobar.MontoReembolsado) < 6000)
                {               
                    
                    await repositorioOperaciones.Aprobar(modelo);
                    await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnAprobar(modelo);
                    await servicioEmailSendGrid.NotificarSolicitudAprobada(operacion);
                    TempData["AlertForBlock"] = "Le confirmo que la solicitud con ID " + $"{operacion.Id}  esta aprobada ";
                    return View("OperacionAprobada", modelo);
                }
                else
                {
                    var resto = 6000 - montoreembolsado;
                    TempData["AlertForBlock"] =
                         "El monto que esta aprobando, supera el tope. Se puede solamente aprobar " + $"{resto} pesos. ";
                    return View("Procesar", modelo);

                }
            }
            else if (regla.Id == 1 || regla.Id == 3 || regla.Id == 4 || regla.Id == 5 || regla.Id == 6 || regla.Id == 7 || regla.Id == 8 || regla.Id == 12)
            {

                if (operacionToAprobar.MontoReembolsado <= tope)
                {                   
                    await repositorioOperaciones.Aprobar(modelo);
                    await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnAprobar(modelo);
                    await servicioEmailSendGrid.NotificarSolicitudAprobada(operacion);
                    TempData["AlertForBlock"] = "Le confirmo que la solicitud con ID " + $"{operacion.Id}  esta aprobada ";
                    return View("OperacionAprobada", modelo);
                }
                else
                {
                    TempData["AlertForBlock"] =
                     "El monto que esta aprobando, supera el tope. Se puede solamente aprobar " + $"{tope} pesos. ";
                    //return RedirectToAction("Procesar", new { operacion.Id });
                    return View("Procesar", modelo);
                }
            }
           
            await repositorioOperaciones.Aprobar(modelo);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnAprobar(modelo);
            await servicioEmailSendGrid.NotificarSolicitudAprobada(operacion);
            TempData["AlertForBlock"] = "Le confirmo que la solicitud con ID " + $"{operacion.Id}  esta aprobada ";
            return View("OperacionAprobada", modelo);
        }

        public IActionResult Creada(OperacionCreacionViewModel operacion)
        {
            return View(operacion);
        }

        public IActionResult Aprobada()
        {

            return View();
        }
        public async Task<IActionResult> Rechazar(int Id)
        {

            var operacion = await repositorioOperaciones.GetOperacionById(Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
            modelo.productos = await SelectProductos();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Rechazar(OperacionCreacionViewModel operacionToRechazar)
        {
            
            var operacion = await repositorioOperaciones.GetOperacionById(operacionToRechazar.Id);
            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
            modelo.CorreoAdministrador = servicioUsuarios.GetADUserEmail();
            modelo.productos= await SelectProductos();  
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            modelo.MontoReembolsado = 0;
            await repositorioOperaciones.Rechazar(modelo);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnRechazar(modelo);
            await servicioEmailSendGrid.NotificarSolicitudRechazada(modelo);
            

            TempData["AlertForBlock"] = "Le confirmo que la solicitud con ID " + $"{operacion.Id}  esta rechazada ";
            return View("Rechazada", modelo);
        }

        public IActionResult Rechazada(OperacionCreacionViewModel operacion)
        {


            return View(operacion);
        }

        public IActionResult OperacionOnHold(OperacionCreacionViewModel operacion)
        {


            return View(operacion);
        }



        [HttpPost]
        public async Task<IActionResult> OnHold(OperacionCreacionViewModel operacionToOnhold)
        {
            
            var operacion = await repositorioOperaciones.GetOperacionById(operacionToOnhold.Id);
            var modelo = mapper.Map<OperacionCreacionViewModel>(operacion);
            modelo.CorreoAdministrador= servicioUsuarios.GetADUserEmail();
            modelo.productos = await SelectProductos();

            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            modelo.FechaProcessed = DateTime.Now;
            modelo.MontoReembolsado = 0;
            await repositorioOperaciones.OnHold(modelo);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnHold(modelo);
            await servicioEmailSendGrid.NotificarSolicitudOnHold(modelo);

            TempData["AlertForBlock"] = "Le confirmo que la solicitud con ID " + $"{operacion.Id}  esta on hold ";
            return View("OperacionOnHold", modelo);
        }

        public async Task<IActionResult> GenerateSolicitudExcelForm(int Id)
        {
            var operacion = await repositorioOperaciones.GetOperacionById(Id);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Solicitud");
                var currentRow = 1;

                #region Header
                worksheet.Cell(currentRow, 1).Value = "Producto";
                worksheet.Cell(currentRow, 2).Value = "Cantidad";
                worksheet.Cell(currentRow, 3).Value = "Precio";
                #endregion


                #region Body
                worksheet.Cell(currentRow + 1, 1).Value = operacion.IdProducto;
                worksheet.Cell(currentRow + 1, 2).Value = operacion.Cantidad;
                worksheet.Cell(currentRow + 1, 3).Value = operacion.Precio;


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Solicitud.xlsx"

                        );
                }
                #endregion
            }

        }


        [HttpPost]
        public async Task<IActionResult> AprobarOperacionReactivada(OperacionCreacionViewModel operacionRecibido)
        {
            var operacionToAprobar = await repositorioOperaciones.GetOperacionReactivadaById(operacionRecibido.Id);
            operacionToAprobar.CorreoAdministrador = servicioUsuarios.GetADUserEmail();

            var operacion = await repositorioOperaciones.GetOperacionReactivadaById(operacionToAprobar.Id);
            if (operacion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var operacionOriginal = await repositorioOperaciones.GetOperacionById(operacionToAprobar.IdOperacion);
            operacionToAprobar.MontoReembolsado = operacionRecibido.MontoReembolsado;
            operacionToAprobar.Comentario = operacionRecibido.Comentario;
            operacionToAprobar.FechaProcessed = DateTime.Now;
            await repositorioOperaciones.AprobarOperacionReactivada(operacionToAprobar);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnAprobarOperacionReactivada(operacionToAprobar);

            await repositorioOperaciones.Aprobar(operacionOriginal);
            await servicioEmailSendGrid.NotificarSolicitudAprobada(operacion);
            return RedirectToAction("Historia", "Operaciones");
        }


        [HttpPost]
        public async Task<IActionResult> RechazarOperacionReactivada(OperacionCreacionViewModel operacionRecibido)
        {

            var operacionToRechazar = await repositorioOperaciones.GetOperacionReactivadaById(operacionRecibido.Id);
            operacionToRechazar.CorreoAdministrador = servicioUsuarios.GetADUserEmail();

            if (operacionToRechazar is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var producto = await repositorioProductos.GetProductoById(operacionToRechazar.IdProducto);
            if (producto is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var operacionOriginal = await repositorioOperaciones.GetOperacionById(operacionToRechazar.IdOperacion);
            operacionToRechazar.FechaProcessed = DateTime.Now;
            operacionToRechazar.Comentario = operacionRecibido.Comentario;
            operacionToRechazar.MontoReembolsado = 0;
            await repositorioOperaciones.RechazarOperacionReactivada(operacionToRechazar);
            await repositorioOperaciones.EditarSumatorioOperacionesDelUsuarioOnRechazarOperacionReactivada(operacionToRechazar);
            await repositorioOperaciones.Rechazar(operacionOriginal);
            await servicioEmailSendGrid.NotificarSolicitudAprobada(operacionToRechazar);




            return RedirectToAction("Historia", "Operaciones");
        }

        public async Task<IActionResult> Reactivaciones(int id)
        {
            var operacionesreactivadas = await repositorioOperaciones.GetOperacionesReactivadasByIdOperacion(id);
            return View(operacionesreactivadas);
        }

        public async Task<IActionResult> SumatorioDelUsuario()
        {
            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var sumatorioOperacionesPendientesDelUsuario = await repositorioOperaciones.GetSumatorioOperacionesPenditesByCorreoUsuario(CorreoUsuario);
            var sumatorioOperacionesProcesadasDelUsuario = await repositorioOperaciones.GetSumatorioOperacionesProcesadasByCorreoUsuario(CorreoUsuario);
            SumatorioDelUsuario sumatorioDelUsuario = new SumatorioDelUsuario();
            sumatorioDelUsuario.SumatorioOperacionesPendientes = sumatorioOperacionesPendientesDelUsuario;
            sumatorioDelUsuario.SumatorioOperacionesProcesadas = sumatorioOperacionesProcesadasDelUsuario;
            return View(sumatorioDelUsuario);
        }


        public async Task<IActionResult> SumatorioDelUsuarioPendientes(PaginacionViewModel paginacionViewModel)
        {
            
            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var sumatorioOperacionesPendientesDelUsuario = await repositorioOperaciones.GetSumatorioOperacionesPenditesByCorreoUsuario(CorreoUsuario);

            var TotalSumatorioOperacionesPendientesDelUsuario = await repositorioOperaciones.ContarSumatorioOperacionesPenditesByCorreoUsuario(CorreoUsuario);
            var respuestaVM = new PaginacionRespuesta<Sumatorio>
            {
                Elementos = sumatorioOperacionesPendientesDelUsuario,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = TotalSumatorioOperacionesPendientesDelUsuario,
                BaseURL = "/SumatorioDelUsuarioPendientes"
            };

            return View(respuestaVM);
        }

        public async Task<IActionResult> SumatorioDelUsuarioProcesadas( PaginacionViewModel paginacionViewModel)
        {
            var CorreoUsuario = servicioUsuarios.GetADUserEmail();
            var sumatorioOperacionesProcesadasDelUsuario = await repositorioOperaciones.GetSumatorioOperacionesProcesadasByCorreoUsuario(CorreoUsuario);
            var TotalSumatorioOperacionesProcesadasDelUsuario = await repositorioOperaciones.ContarSumatorioOperacionesProcesadasByCorreoUsuario(CorreoUsuario);
            var respuestaVM = new PaginacionRespuesta<Sumatorio>
            {
                Elementos = sumatorioOperacionesProcesadasDelUsuario,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = TotalSumatorioOperacionesProcesadasDelUsuario,
                BaseURL = "/SumatorioDelUsuarioProcesadas"
            };
            return View(respuestaVM);
        }


        public async Task<IActionResult> Sumatorio()
        {
            
            var sumatorioOperacionesPendientes = await repositorioOperaciones.GetSumatorioOperacionesPendites();
            var sumatorioOperacionesProcesadas = await repositorioOperaciones.GetSumatorioOperacionesProcesadas();
            SumatorioDelUsuario sumatorioDelUsuario = new SumatorioDelUsuario();
            sumatorioDelUsuario.SumatorioOperacionesPendientes = sumatorioOperacionesPendientes;
            sumatorioDelUsuario.SumatorioOperacionesProcesadas = sumatorioOperacionesProcesadas;
            return View(sumatorioDelUsuario);
        }

        //public async Task<IActionResult> SumatorioPendientes()
        //{

        //    var sumatorioOperacionesPendientes = await repositorioOperaciones.GetSumatorioOperacionesPendites();
        //    var sumatorioOperacionesProcesadas = await repositorioOperaciones.GetSumatorioOperacionesProcesadas();
        //    SumatorioDelUsuario sumatorioDelUsuario = new SumatorioDelUsuario();
        //    sumatorioDelUsuario.SumatorioOperacionesPendientes = sumatorioOperacionesPendientes;
        //    sumatorioDelUsuario.SumatorioOperacionesProcesadas = sumatorioOperacionesProcesadas;
        //    return View(sumatorioDelUsuario);
        //}

        public async Task<IActionResult> SumatorioPendientes(PaginacionViewModel paginacionViewModel)
        {
            PaginacionRespuesta<Sumatorio> respuestaVM;
            var TotalSumatorioOperacionesPendientes = 0;

            var sumatorioOperacionesPendientes = await repositorioOperaciones.GetSumatorioOperacionesPendites();
            TotalSumatorioOperacionesPendientes = await repositorioOperaciones.ContarSumatorioOperacionesPendites();

            respuestaVM = new PaginacionRespuesta<Sumatorio>
            {
                Elementos = sumatorioOperacionesPendientes,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = TotalSumatorioOperacionesPendientes,
                BaseURL = "/SumatorioPendientes"
            };
            

            return View(respuestaVM);
        }
        public async Task<IActionResult> SumatorioProcesadas()
        {

            var sumatorioOperacionesPendientes = await repositorioOperaciones.GetSumatorioOperacionesPendites();
            var sumatorioOperacionesProcesadas = await repositorioOperaciones.GetSumatorioOperacionesProcesadas();
            SumatorioDelUsuario sumatorioDelUsuario = new SumatorioDelUsuario();
            sumatorioDelUsuario.SumatorioOperacionesPendientes = sumatorioOperacionesPendientes;
            sumatorioDelUsuario.SumatorioOperacionesProcesadas = sumatorioOperacionesProcesadas;
            return View(sumatorioDelUsuario);
        }

        //public IActionResult AprobarPartialView(OperacionCreacionViewModel operacion)
        //{
        //    TempData["AlertForBlock"] = "sdfasdfasdfsd";
        //    return View(operacion);
        //}


    }


   


    //public async Task<bool> Validacion(OperacionCreacionViewModel operacion)
    //{
    //    var producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
    //    var regla = await repositorioReglas.GetReglaById(producto.IdRegla);
    //    var tope = regla.Tope;


    //    if (producto.IdRegla == 9)
    //    {


    //        var sum = await repositorioOperaciones.GetCurrentMonthTotalDeOperacionesConsumoOficinaDelColaborador(operacion.CorreoUsuario, producto.IdRegla);
    //        if (
    //            sum < tope)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }


    //    else if (regla.Veces > 0 && regla.Periodo > 0)
    //    {


    //        if (await CheckUsuarioProductoExiste(operacion) && await CheckAntiguedad(operacion))
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            if (await CheckAntiguedad(operacion) && await CheckVecesPorPeriodo(operacion))
    //            {
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //    }

    //    else if (regla.Veces > 0 && regla.Periodo == null)
    //    {
    //        if (await CheckUsuarioProductoExiste(operacion))
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    else if (regla.Veces == null && regla.Periodo == null)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }

    //}


}
    