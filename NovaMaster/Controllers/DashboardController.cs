using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NovaMaster.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CommonController _commonController;
        public DashboardController(CommonController commonController)
        {
            _commonController = commonController;
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }


        public IActionResult AgentDashboard()
        {
            ViewBag.IsUser = false;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if(email != null)
            {
               var res =  _commonController.IsUser(email);
                if (res)
                {
                    ViewBag.IsUser = true;
                    return View();
                }
                return View();
            }
            return View();
        }


        public IActionResult ClientDashboard()
        {
            return View();
        }
    }
}
