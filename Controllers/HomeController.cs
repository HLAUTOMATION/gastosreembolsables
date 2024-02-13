using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using GASTOS_REEMBOLSABLES_VMICA.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioAppUsuarios repositorioAppUsuarios;
        private readonly string connectionString;

        public HomeController(
            ILogger<HomeController> logger, IConfiguration configuration,
            IServicioUsuarios servicioUsuarios,
            IRepositorioAppUsuarios repositorioAppUsuarios
            )
        {
            _logger = logger;
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioAppUsuarios = repositorioAppUsuarios;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    var query = connection.Query("SELECT 1").FirstOrDefault();
            //}

            var correoAppUsuario = servicioUsuarios.GetADUserEmail().ToUpper();
            var appUsuario=await repositorioAppUsuarios.GetAppUsuarioByEmail(correoAppUsuario);
            
            return View(appUsuario);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NoEncontrado()
        {
            return View();
        }

        public IActionResult OperacionFallida()
        {
            return View();
        }

        public IActionResult UsuarioNoExiste()
        {
            return View();
        }
    }
}