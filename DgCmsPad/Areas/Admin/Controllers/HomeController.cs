using Microsoft.AspNetCore.Mvc;

namespace DgCmsPad.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
