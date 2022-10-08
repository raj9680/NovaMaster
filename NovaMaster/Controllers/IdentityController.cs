using Imm.BLL;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NovaMaster.Controllers._Helpers;
using NovaMaster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NovaMaster.Controllers
{
    public class IdentityController: Controller
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly _userClaims _userClaims;
        private readonly ServiceIdentity _serviceIdentity;
        private readonly CommonController _commonController;
        private readonly ServiceCommon _serviceCommon;

        public IdentityController(_userClaims userClaims, ServiceIdentity serviceIdentity, CommonController commonController,
            ILogger<IdentityController> logger, ServiceCommon serviceCommon)
        {
            _userClaims = userClaims;
            _serviceIdentity = serviceIdentity;
            _commonController = commonController;
            _serviceCommon = serviceCommon;
            _logger = logger;
        }

        public async Task<ModelAspNetUsers> GetUsers()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email != null)
            {
                var result = await _serviceIdentity.IdentityViewAsync(email);
                var res = new ModelAspNetUsers()
                {
                    UserId = result.UserId,
                    FullName = result.FullName,
                    Email = result.Email,
                    Password = result.Password,
                    IsActive = result.IsActive
                };
                return res;
            }
            return null;
        }

        public async Task<ModelAspNetUsersInfo> GetUsersInfo()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            var ifExist = _serviceIdentity.IdentityViewInfoAsync(email);
            if (ifExist.Result != null)
            {
                var result = await _serviceIdentity.IdentityViewInfoAsync(email);
                var res = new ModelAspNetUsersInfo()
                {
                    PinCode = result.PinCode,
                    About = result.About,
                    DOB = result.DOB,
                    Website = result.Website,
                    CompanyName = result.CompanyName,
                    StudentSource = result.StudentSource,
                    ContactNumber = result.ContactNumber,
                    AddressLine1 = result.AddressLine1,
                    AddressLine2 = result.AddressLine2,
                    
                };
                res.DOB.ToString();
                return res;
            }
            return null;
        }


        public async Task<ModelAspNetStudentsInfo> GetStudentsInfo()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            var ifExist = _serviceIdentity.ClientViewInfoAsync(email);
            if (ifExist.Result != null)
            {
                AspNetStudentsInfo result = await _serviceIdentity.ClientViewInfoAsync(email);
                ModelAspNetStudentsInfo res = new ModelAspNetStudentsInfo()
                {
                    ContactNumber = result.ContactNumber,
                    DOB = result.DOB == null ? new DateTime() : result.DOB.Value,
                    AddressLine1 = result.AddressLine1,
                    AddressLine2 = result.AddressLine2,
                    Zip = result.Zip == 0 ? 0 : result.Zip.Value,
                    Reference = result.Reference,
                    PrimaryLanguage = result.PrimaryLanguage,
                    EnglishExamType = result.EnglishExamType,
                    Intake = result.Intake,
                    IntakeYear = result.IntakeYear == 0 ? 0 : result.IntakeYear.Value,
                    Program = result.Program,
                    ProgramCollegePreference = result.ProgramCollegePreference,
                    CompanyName = result.CompanyName,
                    JobTitle = result.JobTitle,
                    JobStartDate = result.JobStartDate == null ? new DateTime() : result.JobStartDate.Value,
                    JobEndDate = result.JobEndDate == null ? new DateTime() : result.JobEndDate.Value,
                    IsRefusedVisa = result.IsRefusedVisa,
                    ExplainIfRefused = result.ExplainIfRefused,
                    HaveStudyPermitVisa = result.HaveStudyPermitVisa
                };
                res.DOB.ToString();
                return res;
            }
            return null;
        }


        public List<ModelAspNetusersDocs> GetUsersDocs()
        {
            var finalData = new List<ModelAspNetusersDocs>();
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email != null)
            {
                var res = _serviceIdentity.GetDocsAsync(email);
                foreach (var item in res)
                {
                    ModelAspNetusersDocs dataList = new ModelAspNetusersDocs()
                    {
                        DocId = item.DocId,
                        DocumentName = item.DocumentName,
                        DocumentURL = item.DocumentURL,
                        Type = item.Type,
                        Comments = item.Comments,
                        IsVerified = item.IsVerified
                    };
                    var data = dataList.DocumentName.Split('_')[2];
                    dataList.DocumentName = data;
                    finalData.Add(dataList);
                }
                return finalData;
            }
            return null;
        }

        // [Authorize(Roles ="agent")]
        public async Task<IActionResult> IdentityView(bool IsSuccess = false, bool IsDuplicate = false, bool IsFailed = false)
        {
            _InformationModel myModel = new _InformationModel();
            ViewBag.IsSuccess = IsSuccess;
            ViewBag.IsDuplicate = IsDuplicate;
            ViewBag.IsFailed = IsFailed;
            ViewBag.Country = new SelectList(await _commonController.GetCountryAsync(), "CountryId", "CountryName");
            myModel.Users = await GetUsers();
            if (myModel.Users == null)
                return View("error");
            myModel.Info = await GetUsersInfo();
            myModel.Docs = GetUsersDocs();
            myModel.StudentsInfo = await GetStudentsInfo();
            return View(myModel);
        }

        [HttpPost]
        public async Task<IActionResult> IdentityView(_InformationModel model)
        {
            int myInt;
            var Idd = await GetUsers();
            if (Idd == null)
                return View("Error");
            
            bool isNumerical = int.TryParse(model.Users.FullName, out myInt);
            model.Docs = GetUsersDocs();
            ViewBag.Country = new SelectList(await _commonController.GetCountryAsync(), "CountryId", "CountryName");
            if (model.Users.FullName == null || isNumerical || model.Info.ContactNumber == null || model.Info.PinCode == null || model.Info.AddressLine1 == null || model.Info.ContactNumber.Length < 10 || model.Info.ContactNumber.Length > 15 || model.Info.PinCode.Length < 6 || model.Info.PinCode.Length > 9 || model.Info.AddressLine1.Length > 60)
            {
                if (isNumerical)
                    ModelState.AddModelError("FullName", "Name is not valid");
                if (model.Info.AddressLine2 != null && model.Info.AddressLine2.Length > 60)
                    ModelState.AddModelError("AddressLine2", "Address Line 2 should not exceed 60 characters");
                if (model.Info.About != null && model.Info.About.Length > 120)
                    ModelState.AddModelError("AddressLine2", "Address Line 2 should not exceed 60 characters");
                return View(model);
            }

            if (model.AgentDocs != null && model.AgentDocs.CoverPhoto != null)
            {
                if (model.AgentDocs.CoverPhoto.Length > 500000)
                {
                    ModelState.AddModelError("coverDoc", "Cover Photo should not exceed 500Kb of size");
                    return View("IdentityView", model);
                }
            }

            if (model.AgentDocs != null && model.AgentDocs.CompanyLogo != null)
            {
                if (model.AgentDocs.CompanyLogo.Length > 500000)
                {
                    ModelState.AddModelError("logoDoc", "Logo should not exceed 500Kb of size");
                    return View("IdentityView", model);
                }
            }

            if (model.AgentDocs != null && model.AgentDocs.BusinessCertificate != null)
            {
                if (model.AgentDocs.BusinessCertificate.Length > 4000000)
                {
                    ModelState.AddModelError("businessDoc", "Business Certificate should not exceed 4MB of size");
                    return View("IdentityView", model);
                }
            }

            if (model.AgentDocs != null && model.AgentDocs.CoverPhoto != null && model.AgentDocs.CoverPhoto.FileName != null)
            {
                string ext = Path.GetExtension(model.AgentDocs.CoverPhoto.FileName);
                ext = ext.ToLower();
                if (ext != ".jpg" && ext != ".png")
                {
                    ModelState.AddModelError("invalidCover", "Kindly upload files in jpg/png format");
                    return View("IdentityView", model);
                }
            }

            if (model.AgentDocs != null && model.AgentDocs.CompanyLogo != null && model.AgentDocs.CompanyLogo.FileName != null)
            {
                string ext = Path.GetExtension(model.AgentDocs.CompanyLogo.FileName);
                ext = ext.ToLower();
                if (ext != ".jpg" && ext != ".png")
                {
                    ModelState.AddModelError("invalidLogo", "Kindly upload files in jpg/png format");
                    return View("IdentityView", model);
                }
            }

            if (model.AgentDocs != null && model.AgentDocs.BusinessCertificate != null && model.AgentDocs.BusinessCertificate.FileName != null)
            {
                string ext = Path.GetExtension(model.AgentDocs.BusinessCertificate.FileName);
                ext = ext.ToLower();
                if (ext != ".pdf" && ext != ".jpg" && ext != ".png")
                {
                    ModelState.AddModelError("invalidCertificate", "Kindly upload files in jpg/png/pdf format");
                    return View("IdentityView", model);
                }
            }

            var _res = SubmitAsync(model);
            if (_res == "duplicate")
                return RedirectToAction("IdentityView", new { IsDuplicate = true, IsSuccess = false });

            if (_res != null)
                return RedirectToAction("IdentityView", new { IsDuplicate = false, IsSuccess = true });

            var model1 = new AspNetUsers()
            {
                UserId = Idd.UserId,
                FullName = model.Users.FullName,
                Email = model.Users.Email
            };

            var model2 = new AspNetUsersInfo()
            {
                UserId = Idd.UserId,
                DOB = model.Info.DOB,
                CityId = model.Info.CityId,
                PinCode = model.Info.PinCode,
                About = model.Info.About,
                Website = model.Info.Website,
                StudentSource = model.Info.StudentSource,
                CompanyName = model.Info.CompanyName,
                AddressLine1 = model.Info.AddressLine1,
                AddressLine2 = model.Info.AddressLine2,
                ContactNumber = model.Info.ContactNumber
            };

            var res = await _serviceIdentity.UpdateAsync(model1, model2);
            if (res > 0)
            {
                return RedirectToAction("IdentityView", new { IsSuccess = true });
            }
            // If fails
            return View("IdentityView", model);
        }

        public string SubmitAsync(_InformationModel model)
        {
            if (model.AgentDocs != null)
            {
                ModelAspNetusersDocs doc = new ModelAspNetusersDocs();
                doc.Document = new List<Microsoft.AspNetCore.Http.IFormFile>();
                doc.Document.Add(model.AgentDocs.CompanyLogo);
                doc.Document.Add(model.AgentDocs.CoverPhoto);
                doc.Document.Add(model.AgentDocs.BusinessCertificate);
                var ress = _commonController.MultipleFileUpload(doc, model.Users.Email);
                if(ress.Result.Contains("being used by another process"))
                {
                    return "duplicate";
                }
                if (ress != null)
                {
                    _logger.LogInformation("Agent files uploaded successfully");
                    return ress.ToString();
                }
                if (ress == null)
                    _logger.LogError("Something error with file agent uploading");
            }
            return null;
        }

        [Authorize]
        public async Task<string> ViewFile(int id)
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            if(email != null)
            {
                string _url = await _serviceCommon.ViewFileAsync(id);
                return _url;
            }
            return null;
        }
    }
}
