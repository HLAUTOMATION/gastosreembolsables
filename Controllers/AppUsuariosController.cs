using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.EntityFrameworkCore;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class AppUsuariosController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepositorioColaboradores repositorioColaboradores;
        private readonly IRepositorioAppUsuarios repositorioAppUsuarios;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IRepositorioPerfiles repositorioPerfiles;
        private readonly IRepositorioProyectos repositorioProyectos;
        private readonly IMapper mapper;

        public IRepositorioTipoContratos RepositorioTipoContratos { get; }

        public AppUsuariosController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IRepositorioColaboradores repositorioColaboradores,
            IRepositorioAppUsuarios repositorioAppUsuarios,
            IServicioUsuarios servicioUsuarios,
            IRepositorioCuentas repositorioCuentas,
            IRepositorioTipoContratos repositorioTipoContratos,
            IRepositorioPerfiles repositorioPerfiles,
            IRepositorioProyectos repositorioProyectos,
            IMapper mapper
            
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.repositorioColaboradores = repositorioColaboradores;
            this.repositorioAppUsuarios = repositorioAppUsuarios;
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioCuentas = repositorioCuentas;
            RepositorioTipoContratos = repositorioTipoContratos;
            this.repositorioPerfiles = repositorioPerfiles;
            this.repositorioProyectos = repositorioProyectos;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(PaginacionViewModel paginacionViewModel)
        {
            // var appUsuarios = await context.AppUsuario.ToListAsync();
           var appUsuarios = await repositorioAppUsuarios.ListarAppUsuariosPaginacion(paginacionViewModel);
            if (appUsuarios is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var totalAppUsuarios = await repositorioAppUsuarios.ContarUsuarios();
            
            var respuestaVM = new PaginacionRespuesta<AppUsuario>
            {
                Elementos = appUsuarios,
                Pagina = paginacionViewModel.Pagina,
                RecordsPorPagina = paginacionViewModel.RecordsPorPagina,
                CantidadTotalRecords = totalAppUsuarios,
                BaseURL = "/AppUsuarios"
            };
            return View(respuestaVM);
        }

        public async Task<IActionResult> IndexColaborador()
        {
            // var appUsuarios = await context.AppUsuario.ToListAsync();
            var EmailAppUsuario=servicioUsuarios.GetADUserEmail();
            var appUsuario = await repositorioAppUsuarios.GetAppUsuarioByEmail(EmailAppUsuario);
            if (appUsuario is null )
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(appUsuario);
        }

        //[AllowAnonymous]
        public async Task<IActionResult> Crear()
        {

            //if (!await roleManager.RoleExistsAsync("Administrador"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Administrador"));
            //}

            //if (!await roleManager.RoleExistsAsync("Consultor"))
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Consultor"));
            //}
            var model = new AppUsuarioCreacionViewModel();
            model.colaboradores = await SelectColaboradores();
            model.tipocontratos = await SelectTipoContratos();
            model.perfiles=await SelectPerfiles();  
            model.proyectos=await SelectProyectos();
            return View(model);
        }

        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> Crear(AppUsuarioCreacionViewModel modelo)
        
        {


            //var tipocontrato = await RepositorioTipoContratos.GetTipoContratoById(modelo.TipoContrato);
            //var perfile = await repositorioPerfiles.GetPerfileById(modelo.IdPerfile);
            //var proyecto = await repositorioProyectos.GetProyectoById(modelo.IdProyecto);

            //if (tipocontrato is null)
            //{
            //    return RedirectToAction("NoEncontrado", "Home");
            //}

            //if (perfile is null)
            //{
            //    return RedirectToAction("NoEncontrado", "Home");
            //}

            //if (proyecto is null)
            //{
            //    return RedirectToAction("NoEncontrado", "Home");
            //}

            //if (!ModelState.IsValid)
            //{
                //modelo.tipocontratos = await SelectTipoContratos();
                //modelo.perfiles = await SelectPerfiles();
                //modelo.proyectos = await SelectProyectos();
                //return View(modelo);
            //}

            modelo.EmailUsuario.ToUpper();
            modelo.FechaCreacion = DateTime.Now;

            await repositorioAppUsuarios.Crear(modelo);
            return RedirectToAction("Index", "AppUsuarios");

            //ModelState.Remove("IdRole");
            //var usuario = new AppUsuario() { Email = modelo.Email,UserName=modelo.Email };
            //var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            //if (resultado.Succeeded)
            //{

            //    await userManager.AddToRoleAsync(usuario, "Consultor");

            //    await signInManager.SignInAsync(usuario, isPersistent: false);
            //    return RedirectToAction("Index", "Operaciones");
            //}
            //else
            //{
            //    foreach (var error in resultado.Errors)
            //    {
            //        ModelState.AddModelError(String.Empty, error.Description);
            //    }
            //    modelo.colaboradores = await SelectColaboradores();
            //    return View(modelo);

            //}
        }

        public async Task<IActionResult> Editar(int Id)
        {
            var appUsuario = await repositorioAppUsuarios.GetAppUsuarioById(Id);

            if (appUsuario is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<AppUsuarioCreacionViewModel>(appUsuario);
            modelo.tipocontratos = await SelectTipoContratos();
            modelo.proyectos = await SelectProyectos();

            //var EmailAppUsuario = servicioUsuarios.GetADUserEmail();  
            //var modelo = await repositorioAppUsuarios.GetAppUsuarioByEmail(EmailAppUsuario);

            //if (modelo is null)
            //{
            //    return RedirectToAction("NoEncontrado", "Home");
            //}
            return View(modelo);
        }

        public async Task<IActionResult> Ver(string Id)
        {
            
            var modelo = await repositorioAppUsuarios.GetAppUsuarioByEmail(Id);

            if (modelo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            modelo.cuentas =await GetCuentasByUsuario(Id);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(AppUsuario appUsuarioNew)
        {
            //var EmailAppUsuario = servicioUsuarios.GetADUserEmail();

            if (!ModelState.IsValid)
            {
                return View(appUsuarioNew);
            }

            //var appUsuario = await repositorioAppUsuarios.GetAppUsuarioByEmail(EmailAppUsuario);
            var appUsuario = await repositorioAppUsuarios.GetAppUsuarioById(appUsuarioNew.Id);
            if (appUsuario is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioAppUsuarios.Editar(appUsuarioNew);
            return RedirectToAction("Index");
        }

        //[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string? returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AppUsuarioLoginViewModel modelo, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(
                modelo.Email,
                modelo.Password,
                modelo.Recuerdame,
                lockoutOnFailure: false
                );

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Operaciones");
               // return RedirectToAction(returnurl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto");
                return View(modelo);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login");

        }

        

        //[AllowAnonymous]
        public async Task<IActionResult> CearByAdmin()
        {

            if (!await roleManager.RoleExistsAsync("Administrador"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrador"));
            }

            if (!await roleManager.RoleExistsAsync("Consultor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Consultor"));
            }

            
            var model = new AppUsuarioCreacionViewModel();
            model.colaboradores = await SelectColaboradores();
            
            return View(model);
        }

        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CearByAdmin(AppUsuarioCreacionViewModel modelo)
        {

            //ModelState.Remove("UserName");
            if (!ModelState.IsValid)
            {
                modelo.colaboradores = await SelectColaboradores();
                return View(modelo);
            }

            var usuario = new AppUsuario() { Email = modelo.Email, UserName = modelo.Email };
            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {

                await userManager.AddToRoleAsync(usuario, "Consultor");

                await signInManager.SignInAsync(usuario, isPersistent: false);
                return RedirectToAction("Index", "Operaciones");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                modelo.colaboradores = await SelectColaboradores();
                return View(modelo);

            }
        }


        public async Task<IEnumerable<SelectListItem>> SelectTipoContratos()
        {
            
            var tipocontratos = await RepositorioTipoContratos.ListarTipoContratos();
            return tipocontratos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }


        public async Task<IEnumerable<SelectListItem>> SelectPerfiles()
        {

            var perfiles = await repositorioPerfiles.ListarPerfiles();
            return perfiles.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IEnumerable<SelectListItem>> SelectTipoContrato()
        {

            var tipocontratos = await RepositorioTipoContratos.ListarTipoContratos();
            return tipocontratos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IEnumerable<SelectListItem>> SelectProyectos()
        {

            var proyectos = await repositorioProyectos.ListarProyectos();
            return proyectos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IEnumerable<SelectListItem>> SelectColaboradores()
        {
            //var colaboradores = await repositorioColaboradores.ListarColaboradores();
            var colaboradores = await repositorioAppUsuarios.ListarAppUsuarios();
            return colaboradores.Select(x => new SelectListItem(x.ApellidoNombre, x.Id.ToString()));
        }

        public IEnumerable<SelectListItem> SelectRoles()
        {
            var roles = new List<SelectListItem>();
            roles.Add(
                new SelectListItem()
                {
                    Value = "Registrado",
                    Text = "Registrado"
                });
            roles.Add(
                new SelectListItem()
                {
                    Value = "Administrador",
                    Text = "Administrador"
                });
            return roles;
        }

        public async Task<IEnumerable<Cuenta>> GetCuentasByUsuario(string email)
        {

            
            var cuentas = await repositorioCuentas.GetCuentasByEmailAppUsuario(email);
            return cuentas;
        }


        public async Task<IActionResult> BorrarPartialView(int Id)
        {
            var cliente = await repositorioAppUsuarios.GetAppUsuarioById(Id);
            return PartialView("BorrarPartialView", cliente);
        }


        [HttpPost]
        public async Task<IActionResult> BorrarPartialView(Cliente cliente)
        {
            var appUsuarioToBorrar = await repositorioAppUsuarios.GetAppUsuarioById(cliente.Id);
            if (appUsuarioToBorrar is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioAppUsuarios.Borrar(appUsuarioToBorrar.Id);

            //await servicioEmailSendGrid.Actualizar(operacionNew);
            return RedirectToAction("Index", "AppUsuarios");

        }
    }
    }

