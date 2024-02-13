using Microsoft.AspNetCore.Mvc;

namespace GASTOS_REEMBOLSABLES_VMICA.Controllers
{
	public class ReportsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
