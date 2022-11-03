using Imm.BLL;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovaMaster.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NovaMaster.Controllers
{
    public class AccessController : Controller
    {
        private readonly ServiceAccess _serviceAccess = null;
        public AccessController(ServiceAccess serviceAccess)
        {
            _serviceAccess = serviceAccess;
        }

        public IActionResult Register()
        {
            var logegedIn= this.HttpContext.Session.GetString("Token");
            if (logegedIn != null && User.Identity.IsAuthenticated)
                return RedirectToAction("IdentityView", "Identity");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ModelAspNetUsers user)
        {
            int myInt;
            bool isNumerical = int.TryParse(user.FullName, out myInt);
            if (isNumerical)
            {
                ModelState.AddModelError("FullName", "Name is not valid");
                return View(user);
            }
            if (ModelState.IsValid)
            {
                AspNetUsers users = new AspNetUsers()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = user.Password,
                    CnfEmail = false,
                    IsActive = false
                };
                int result = await _serviceAccess.RegisterAsync(users);
                if (result > 0)
                {
                    return RedirectToAction("Login", new
                    {
                        isSuccess = true
                    });
                }
                else if (result < 0)
                {
                    ModelState.AddModelError("Email", "Email already exist.");
                }
            }
            return View(user);
        }

        public IActionResult Login(bool isSuccess = false)
        {
            var logegedIn = this.HttpContext.Session.GetString("Token");
            if (logegedIn != null && User.Identity.IsAuthenticated)
                return RedirectToAction("IdentityView", "Identity");
            ViewBag.IsSuccess = isSuccess;
            return View();
        }


        [HttpPost]
        public IActionResult Login(ModelAspNetUsers user)
        {
            if (user.Password == null)
            {
                ModelState.AddModelError("Password", "Password required.");
                return View(user);
            }

            AspNetUsers users = new AspNetUsers()
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password
            };

            IDictionary<string,string> token = _serviceAccess.LoginAsync(users);
            if (token == null)
            {
                ModelState.AddModelError("WrongCredentials", "Something wrong with user credentials.");
                return View();
            }
            
            if(token["token"] != null && token["role"] != null && token["role"] == "agent")
            {
                HttpContext.Session.SetString("Token", token["token"]);
                HttpContext.Session.SetString("Role", token["role"]);
                return RedirectToAction("AgentDashboard", "Dashboard");
            }
            
            if(token["token"] != null && token["role"] != null && token["role"] == "client")
            {
                HttpContext.Session.SetString("Token", token["token"]);
                HttpContext.Session.SetString("Role", token["role"]);
                return RedirectToAction("ClientDashboard", "Dashboard");
            }
            
            if (token["token"] != null && token["role"] != null && token["role"] == "admin")
            {
                HttpContext.Session.SetString("Token", token["token"]);
                HttpContext.Session.SetString("Role", token["role"]);
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
            return View();
        }


        public IActionResult ChangePassword(bool send = false, bool NotExist = false, bool Failed = false)
        {
            ViewBag.IsSent = send;
            ViewBag.NotExist = NotExist;
            ViewBag.Failed = Failed;
            return View();
        }


        [HttpPost]
        public IActionResult changePasswordAction(string email)
        {
            if (email != null)
            {
                int userId = _serviceAccess.GetUserId(email);
                if (userId > 0)
                {
                    var res = Email.EmailVerification.Main(email, userId);
                    if(res == "success")
                        return RedirectToAction("ChangePassword", new { send = true });
                    if(res == "failed")
                        return RedirectToAction("ChangePassword", new { Failed = true });
                }
                return RedirectToAction("ChangePassword", new { NotExist = true });
            }
            return RedirectToAction("ChangePassword", new { NotExist = true });
        }


        [HttpPost]
        public async Task<string> ChangePassword(string pass)
        {
            if(pass != null)
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (email != null)
                {
                    await _serviceAccess.ChangePasswordAsync(email, pass);
                    HttpContext.Session.Remove("Token");
                    return "success";
                }
            }
            return "failed";
        }


        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login");
        }
    }
}