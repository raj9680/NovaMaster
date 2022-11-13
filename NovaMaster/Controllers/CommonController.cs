using Imm.BLL;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NovaMaster.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NovaMaster.Controllers
{
    public class CommonController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment = null;
        private readonly ServiceCommon _serviceCommon;
        private readonly ILogger<CommonController> _logger;
        
        public CommonController(ILogger<CommonController> logger, ServiceCommon serviceCommon, IWebHostEnvironment hostingEnvironment)
        {
            _serviceCommon = serviceCommon;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task<List<NovaMaster.Models.Country>> GetCountryAsync()
        {
            var receivedCountry = await _serviceCommon.GetCountryAsync();
            var _country = new List<NovaMaster.Models.Country>();
            foreach (var item in receivedCountry)
            {
                var items = new NovaMaster.Models.Country()
                {
                    CountryId = item.CountryId,
                    CountryName = item.CountryName
                };
                _country.Add(items);
            }
            return _country;
        }

        public async Task<IActionResult> GetState(int CountryId)
        {
            ViewBag.State = new SelectList(await _serviceCommon.GetStateAsync(CountryId), "StateId", "StateName");
            return new JsonResult(ViewBag.State);
        }

        public async Task<IActionResult> GetCity(int StateId)
        {
            ViewBag.City = new SelectList(await _serviceCommon.GetCityAsync(StateId), "CityId", "CityName");
            return new JsonResult(ViewBag.City);
        }

        public AspNetUsers GetUserInfo(string email)
        {
            return _serviceCommon.GetUserInfoAsync(email);
        }

        [HttpPost]
        public async Task<string> MultipleFileUpload(ModelAspNetusersDocs model, string email) // async imp.
        {
           var result = new List<string>();
           try
            {
                if (ModelState.IsValid)
                {
                    AspNetUsersDocs _File = new AspNetUsersDocs()
                    {
                    Document = model.Document,
                    DocumentName = model.DocumentName,
                    DocumentURL = model.DocumentURL
                    };
                    string rootPath = _hostingEnvironment.WebRootPath;
                    result =  _serviceCommon.MultipleFileUploadAsync(_File, rootPath, email);
                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("being used by another process"))
                    _logger.LogError(ex.Message, "Image is either already in use or it is duplicate.");
                return "being used by another process";
            }
            return result.ToString();
        }

        public IActionResult InviteClient()
        {
            return RedirectToAction("InviteClient","Client");
        }

        public IActionResult ResetPassword([FromQuery] int anonymous, bool IsSuccess = false, bool IsBadRequest = false)
        {
            ViewBag.anonymous = anonymous;
            ViewBag.IsSuccess = IsSuccess;
            ViewBag.IsbadRequest = IsBadRequest;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            ViewBag.anonymous = model.annotation;
            if (model.annotation < 1 )
            {
                return RedirectToAction("ResetPassword", new { IsBadRequest = true });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    AspNetUsers user = new AspNetUsers()
                    {
                        UserId = model.annotation,
                        Password = model.NewPassword
                    };
                    await _serviceCommon.ResetPasswordAsync(user);
                    return RedirectToAction("ResetPassword", new { IsSuccess = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, "Something wrong in CommonController.ResetPassword");
                    return View(model);
                }
            }
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public List<AgentStudentViewModel> ClientAgent(string email)
        {
            return _serviceCommon.ClientAgentViewAsync(email);
        }

        [Authorize(Roles = "agent,admin")]
        public List<AgentStudentViewModel> AllClient(string email)
        {
            return _serviceCommon.AllClientViewAsync(email);
        }

        [Authorize(Roles = "admin")]
        public List<AgentStudentViewModel> Clients(int id)
        {
            return _serviceCommon.ClientsAsync(id);
        }


        [Authorize]
        public bool DeleteFile(int docId, string docName)
        {
            var _rootPath = _hostingEnvironment.WebRootPath;
            return _serviceCommon.DeleteFileAsync(docId, docName, _rootPath);
        }

        public bool IsUser(string email)
        {
            return _serviceCommon.Isuser(email);
        }

        public List<AgentStudentViewModel> NewRegistrationAsync(string email)
        {
            return _serviceCommon.NewRegistration(email);
        }
    }
}
