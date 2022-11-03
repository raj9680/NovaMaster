using Microsoft.AspNetCore.Mvc;

namespace NovaMaster.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }


        public IActionResult AgentDashboard()
        {
            return View();
        }


        public IActionResult ClientDashboard()
        {
            return View();
        }
    }
}
