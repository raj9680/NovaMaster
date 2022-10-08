using Imm.BLL;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NovaMaster.Controllers;
using NovaMaster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

public class ClientController : Controller
    {
        private readonly ServiceClient _serviceClient;
        private readonly IWebHostEnvironment _hostingEnvironment = null;
        private readonly CommonController _commonController;
        private readonly ILogger<ClientController> _logger;
        public ClientController(ServiceClient serviceClient, IWebHostEnvironment hostingEnvironment, CommonController commonController, ILogger<ClientController> logger)
        {
            _serviceClient = serviceClient ??
              throw new ArgumentNullException(nameof(serviceClient));
            _commonController = commonController;
            _hostingEnvironment = hostingEnvironment;
            _serviceClient = serviceClient;
            _logger = logger;
        }


        [Authorize(Roles = "agent,admin")]
        public IActionResult AllClient()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
                return View("error");
            var data = _commonController.ClientAgent(email);
            List<AspNetStudentsInfo> si = new List<AspNetStudentsInfo>();
            List<AspNetUsers> ui = new List<AspNetUsers>();
            List<AspNetUsersManager> um = new List<AspNetUsersManager>();
            foreach (var item in data)
            {
                AspNetStudentsInfo tat = new AspNetStudentsInfo()
                {
                    UserId = item.UserId,
                    ContactNumber = item.ContactNumber,
                    DOB = item.DOB.Value.Date
                };

                AspNetUsers tat1 = new AspNetUsers()
                {
                    FullName = item.FullName,
                    Email = item.Email,
                    CnfEmail = item.Confirmed
                };

                AspNetUsersManager tat2 = new AspNetUsersManager()
                {
                    DOJ = item.DOJ.Value.Date
                };

                si.Add(tat);
                ui.Add(tat1);
                um.Add(tat2);
            }

            List<object> stuff = new List<object>();
            stuff.Add(si);
            stuff.Add(ui);
            stuff.Add(um);

            object ClientInfo = JsonConvert.SerializeObject(stuff);
            TempData["ClientInfo"] = ClientInfo;

            if (ClientInfo == null)
                return View("error");
            ViewBag.ClientInfo = JsonConvert.DeserializeObject<List<Object>>(ClientInfo.ToString());
            return View();
        }


        public bool IsClientExist(string ClientEmail)
        {
            var IsExist = _serviceClient.IsClientExistAsync(ClientEmail);
            if (IsExist)
                return true;
            return false;
        }

        public IActionResult InviteClient()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
                return View("error");
            var info = _commonController.GetUserInfo(email);
            ViewBag.Id = info.UserId;
            return View();
        }

        public async Task<IActionResult> AddClient([FromQuery] int anonymous)
        {
            ViewBag.AgentId = anonymous;
            ViewBag.Country = new SelectList(await _commonController.GetCountryAsync(), "CountryId", "CountryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(_InformationModel model)
        {
            int myInt;
            bool isNumerical = int.TryParse(model.Users.FullName, out myInt);
            ViewBag.Country = new SelectList(await _commonController.GetCountryAsync(), "CountryId", "CountryName");
            // Validate modals
            if (model.Users.FullName == null || model.Users.FullName.Length < 3 || isNumerical || model.Users.Email == null || model.Users.Password == null || model.Users.Password.Length < 6 || model.Users.CnfPassword == null || model.Users.CnfPassword.Length < 6 || model.StudentsInfo.ContactNumber == null || model.StudentsInfo.ContactNumber.Length < 10 || model.StudentsInfo.ContactNumber.Length > 15 || model.StudentsInfo.DOB == null || model.StudentsInfo.AddressLine1 == null || model.StudentsInfo.AddressLine1.Length > 120 || (model.StudentsInfo.AddressLine2 != null && model.StudentsInfo.AddressLine2.Length > 120) || model.StudentsInfo.PrimaryLanguage.ToString().Length < 3 || model.StudentsInfo.EnglishExamType.ToString().Length < 3 || model.StudentsInfo.Intake.ToString().Length < 3 || model.StudentsInfo.IntakeYear.ToString() == null || model.StudentsInfo.Program == null)
            {
                if (isNumerical || model.Users.FullName == null || model.Users.FullName.Length < 3)
                    ModelState.AddModelError("FullName", "Name is not valid");
                if (model.Users.Email == null || model.Users.ToString().Length < 3)
                    ModelState.AddModelError("Email", "Invalid email");
                if (model.Users.Password == null || model.Users.Password.Length < 6)
                    ModelState.AddModelError("Password", "Invalid password");
                if (model.Users.CnfPassword == null || model.Users.CnfPassword.Length < 6)
                    ModelState.AddModelError("ConfirmPassword", "Password & confirm password not matched");
                if (model.StudentsInfo.ContactNumber == null || model.StudentsInfo.ContactNumber.Length < 10 || model.StudentsInfo.ContactNumber.Length > 15)
                    ModelState.AddModelError("ContactNumber", "Min. 10 & Max. 15 lenght of contact number is required");
                if (model.StudentsInfo.DOB == null || model.StudentsInfo.DOB.ToString().Length < 3)
                    ModelState.AddModelError("DOB", "DOB is required");
                if (model.StudentsInfo.AddressLine1 == null || model.StudentsInfo.AddressLine1.Length > 120)
                    ModelState.AddModelError("Address1", "Address Line 1 required (max. of 120 characters or less");
                if (model.StudentsInfo.AddressLine2 != null && model.StudentsInfo.AddressLine2.Length > 120)
                    ModelState.AddModelError("Address2", "Address Line 2 should not exceed 120 characters");
                if (model.StudentsInfo.PrimaryLanguage.ToString().Length < 3)
                    ModelState.AddModelError("PrimaryLanguage", "Please select primary language");
                if (model.StudentsInfo.EnglishExamType.ToString().Length < 3)
                    ModelState.AddModelError("EnglishExamType", "Please select english exam type");
                if (model.StudentsInfo.Intake.ToString().Length < 3)
                    ModelState.AddModelError("Intake", "Please select intake exam");
                return View(model);
            }

            if (model.ClientDocs != null)
            {
                if (model.ClientDocs.Passport != null)
                {
                    string passport = Path.GetExtension(model.ClientDocs.Passport.FileName);
                    passport = passport.ToLower();
                    if (passport != ".jpg" && passport != ".png" && passport != ".pdf" && passport != ".jpeg")
                    {
                        ModelState.AddModelError("Passport", "Invalid file format");
                        return View(model);
                    }
                }

                if (model.ClientDocs.EnglishExam != null)
                {
                    string englishExam = Path.GetExtension(model.ClientDocs.EnglishExam.FileName);
                    englishExam = englishExam.ToLower();
                    if (englishExam != ".jpg" && englishExam != ".png" && englishExam != ".pdf" && englishExam != ".jpeg")
                    {
                        ModelState.AddModelError("English", "Invalid file format");
                        return View(model);
                    }
                }

                if (model.ClientDocs.Matriculation != null)
                {
                    string matriculation = Path.GetExtension(model.ClientDocs.Matriculation.FileName);
                    matriculation = matriculation.ToLower();
                    if (matriculation != ".jpg" && matriculation != ".png" && matriculation != ".pdf" && matriculation != ".jpeg")
                    {
                        ModelState.AddModelError("Matriculation", "Invalid file format");
                        return View(model);
                    }
                }

                if (model.ClientDocs.SeniorSecondary != null)
                {
                    string senior = Path.GetExtension(model.ClientDocs.SeniorSecondary.FileName);
                    senior = senior.ToLower();
                    if (senior != ".jpg" && senior != ".png" && senior != ".pdf" && senior != ".jpeg")
                    {
                        ModelState.AddModelError("Senior", "Invalid file format");
                        return View(model);
                    }
                }

                if (model.ClientDocs.BachelorsDegree != null)
                {
                    string bachelors = Path.GetExtension(model.ClientDocs.BachelorsDegree.FileName);
                    bachelors = bachelors.ToLower();
                    if (bachelors != ".jpg" && bachelors != ".png" && bachelors != ".pdf" && bachelors != ".jpeg")
                    {
                        ModelState.AddModelError("Bachelors", "Invalid file format");
                        return View(model);
                    }
                }

                if (model.ClientDocs.WorkExperience != null)
                {
                    string workExperience = Path.GetExtension(model.ClientDocs.WorkExperience.FileName);
                    workExperience = workExperience.ToLower();
                    if (workExperience != ".jpg" && workExperience != ".png" && workExperience != ".pdf" && workExperience != ".jpeg")
                    {
                        ModelState.AddModelError("WorkExperience", "Invalid file format");
                        return View(model);
                    }
                }
            }

            if (model.ClientDocs != null)
            {
                if (model.ClientDocs.EnglishExam != null && model.ClientDocs.EnglishExam.Length > 3145728)
                {
                    ModelState.AddModelError("EnglishExam", "File size should not exceed 3MB");
                    return View(model);
                }
                    
                if (model.ClientDocs.Matriculation != null && model.ClientDocs.Matriculation.Length > 3145728)
                {
                    ModelState.AddModelError("Matriculation", "File size should not exceed 3MB");
                    return View(model);
                }
                    
                if (model.ClientDocs.SeniorSecondary != null && model.ClientDocs.SeniorSecondary.Length > 3145728)
                {
                    ModelState.AddModelError("Secondary", "File size should not exceed 3MB");
                    return View(model);
                }
                    
                if (model.ClientDocs.BachelorsDegree != null && model.ClientDocs.BachelorsDegree.Length > 3145728)
                {
                    ModelState.AddModelError("Bachelors", "File size should not exceed 3MB");
                    return View(model);
                }
                    
                if (model.ClientDocs.WorkExperience != null && model.ClientDocs.WorkExperience.Length > 3145728)
                {
                    ModelState.AddModelError("WorkExperience", "File size should not exceed 3MB");
                    return View(model);
                }
            }

            if (model.Users.AgentId < 1)
            {
                model.Users.AgentId = 1;
            }
            ViewBag.AgentId = model.Users.AgentId;
            var data1 = new AspNetUsers()
            {
                FullName = model.Users.FullName,
                Email = model.Users.Email,
                Password = model.Users.Password
            };

            var data2 = new AspNetStudentsInfo()
            {
                ContactNumber = model.StudentsInfo.ContactNumber,
                DOB = model.StudentsInfo.DOB,
                AddressLine1 = model.StudentsInfo.AddressLine1,
                AddressLine2 = model.StudentsInfo.AddressLine2,
                CityId = model.Info.CityId,
                Zip = model.StudentsInfo.Zip,
                Reference = model.StudentsInfo.Reference,
                PrimaryLanguage = model.StudentsInfo.PrimaryLanguage,
                EnglishExamType = model.StudentsInfo.EnglishExamType,
                Intake = model.StudentsInfo.Intake,
                IntakeYear = model.StudentsInfo.IntakeYear,
                Program = model.StudentsInfo.Program,
                ProgramCollegePreference = model.StudentsInfo.ProgramCollegePreference,
                HighestEducation = model.StudentsInfo.HighestEducation,
                // Masters
                MastersEducationStartDate = model.StudentsInfo.MastersEducationStartDate,
                MastersEducationEndDate = model.StudentsInfo.MastersEducationEndDate,
                MastersEducationCompletionDate = model.StudentsInfo.MastersEducationCompletionDate,
                MastersInstituteInfo = model.StudentsInfo.MastersInstituteInfo,
                MastersEducationPercentage = model.StudentsInfo.MastersEducationPercentage,
                MastersEducationMathsmarks = model.StudentsInfo.MastersEducationMathsmarks,
                MastersEducationEnglishMarks = model.StudentsInfo.MastersEducationEnglishMarks,
                // Bacheors
                BachelorsEducationStartDate = model.StudentsInfo.BachelorsEducationStartDate,
                BachelorsEducationEndDate = model.StudentsInfo.BachelorsEducationEndDate,
                BachelorsEducationCompletionDate = model.StudentsInfo.BachelorsEducationCompletionDate,
                BachelorsInstituteInfo = model.StudentsInfo.BachelorsInstituteInfo,
                BachelorsEducationPercentage = model.StudentsInfo.BachelorsEducationPercentage,
                BachelorsEducationMathsmarks = model.StudentsInfo.BachelorsEducationMathsmarks,
                BachelorsEducationEnglishMarks = model.StudentsInfo.BachelorsEducationEnglishMarks,
                // Secondary
                SecondaryEducationStartDate = model.StudentsInfo.SecondaryEducationStartDate,
                SecondaryEducationEndDate = model.StudentsInfo.SecondaryEducationEndDate,
                SecondaryEducationCompletionDate = model.StudentsInfo.SecondaryEducationCompletionDate,
                SecondaryInstituteInfo = model.StudentsInfo.SecondaryInstituteInfo,
                SecondaryEducationPercentage = model.StudentsInfo.SecondaryEducationPercentage,
                SecondaryEducationMathsmarks = model.StudentsInfo.SecondaryEducationMathsmarks,
                SecondaryEducationEnglishMarks = model.StudentsInfo.SecondaryEducationEnglishMarks,
                // Matriculation
                MatricEducationStartDate = model.StudentsInfo.MatricEducationStartDate,
                MatricEducationEndDate = model.StudentsInfo.MatricEducationEndDate,
                MatricEducationCompletionDate = model.StudentsInfo.MatricEducationCompletionDate,
                MatricInstituteInfo = model.StudentsInfo.MatricInstituteInfo,
                MatricEducationPercentage = model.StudentsInfo.MatricEducationPercentage,
                MatricEducationMathsmarks = model.StudentsInfo.MatricEducationMathsmarks,
                MatricEducationEnglishMarks = model.StudentsInfo.MatricEducationEnglishMarks,
                // Other Educations
                // Job Details if Any()
                CompanyName = model.StudentsInfo.CompanyName,
                JobTitle = model.StudentsInfo.JobTitle,
                JobStartDate = model.StudentsInfo.JobStartDate,
                JobEndDate = model.StudentsInfo.JobEndDate,
                IsRefusedVisa = true,
                ExplainIfRefused = model.StudentsInfo.ExplainIfRefused,
                HaveStudyPermitVisa = model.StudentsInfo.HaveStudyPermitVisa

            };

            // Documents setup
            AspNetUsersDocs data3 = new AspNetUsersDocs();
            string rootPath = _hostingEnvironment.WebRootPath;
            if (model.ClientDocs != null)
            {
                data3.Document = new List<Microsoft.AspNetCore.Http.IFormFile>();
                data3.Document.Add(model.ClientDocs.Passport);
                data3.Document.Add(model.ClientDocs.EnglishExam);
                data3.Document.Add(model.ClientDocs.SeniorSecondary);
                data3.Document.Add(model.ClientDocs.BachelorsDegree);
                data3.Document.Add(model.ClientDocs.Matriculation);
                data3.Document.Add(model.ClientDocs.WorkExperience);
            }
            //
            var ExistingUser = User.FindFirst(ClaimTypes.Email)?.Value;
            if(ExistingUser != null)
            {
                data1.Email = ExistingUser;
            }
            //
            var res = await _serviceClient.CreateClientAsync(data1, data2, data3, rootPath, model.Users.AgentId);
            if (res < 0)
            {
                _logger.LogError("Email is already in use. Exception raised from ClientController.AddClient");
                ModelState.AddModelError("ExistingEmail", "Email already exist.");
                return View();
            }
            if(res == 0)
            {
                ViewBag.Failed = true;
                return View();
            }
            ViewBag.IsSuccess = true;
            return RedirectToAction("Login", "Access", ViewBag.IsSuccess);
        }

        public IActionResult UpdateClient(_InformationModel model, bool IsSuccess = false)
        {
            return RedirectToAction("IdentityView", "Identity", model);
        }

        // Update
        [HttpPost]
        public async Task<IActionResult> UpdateClient(_InformationModel model)
        {
            int myInt;
            bool isNumerical = int.TryParse(model.Users.FullName, out myInt);
            ViewBag.Country = new SelectList(await _commonController.GetCountryAsync(), "CountryId", "CountryName");
            // Validate modals
            if (model.Users.FullName == null || model.Users.FullName.Length < 3 || isNumerical || model.Users.Email == null || model.Users.Email.Length < 3 || model.StudentsInfo.ContactNumber == null || model.StudentsInfo.ContactNumber.Length < 10 || model.StudentsInfo.ContactNumber.Length > 15 || model.StudentsInfo.DOB == null || model.StudentsInfo.AddressLine1 == null || model.StudentsInfo.AddressLine1.Length > 120 || (model.StudentsInfo.AddressLine2 != null && model.StudentsInfo.AddressLine2.Length > 120) || model.Info.CityId == -1 || model.StudentsInfo.Zip == 0 || model.StudentsInfo.Zip.ToString().Length < 6 ||  model.StudentsInfo.PrimaryLanguage.ToString().Length < 3 || model.StudentsInfo.EnglishExamType.ToString().Length < 3 || model.StudentsInfo.Intake.ToString().Length < 3 || model.StudentsInfo.IntakeYear.ToString() == null || model.StudentsInfo.Program == null || model.StudentsInfo.Program.ToString() == "-1" || model.StudentsInfo.Program.ToString().Length < 3 || model.StudentsInfo.IsRefusedVisa == null || model.StudentsInfo.HaveStudyPermitVisa == null || model.StudentsInfo.HaveStudyPermitVisa.ToString() == "-1" || model.StudentsInfo.HaveStudyPermitVisa.ToString().Length < 3)
            {
                if (isNumerical || model.Users.FullName == null || model.Users.FullName.Length < 3)
                    ModelState.AddModelError("FullName", "Name is not valid");
                if (isNumerical || model.Users.Email == null || model.Users.Email.Length < 3)
                    ModelState.AddModelError("Email", "Email is not valid");
                if (model.StudentsInfo.ContactNumber == null || model.StudentsInfo.ContactNumber.Length < 10 || model.StudentsInfo.ContactNumber.Length > 15)
                    ModelState.AddModelError("ContactNumber", "Min. 10 & Max. 15 lenght of contact number is required");
                if (model.StudentsInfo.DOB == null || model.StudentsInfo.DOB.ToString().Length < 3)
                    ModelState.AddModelError("DOB", "DOB is required");
                if (model.StudentsInfo.AddressLine1 == null || model.StudentsInfo.AddressLine1.Length > 120)
                    ModelState.AddModelError("Address1", "Address Line 1 required (max. of 120 characters or less");
                if (model.StudentsInfo.AddressLine2 != null && model.StudentsInfo.AddressLine2.Length > 120)
                    ModelState.AddModelError("Address2", "Address Line 2 should not exceed 120 characters");
                if (model.StudentsInfo.PrimaryLanguage.Length < 3)
                    ModelState.AddModelError("PrimaryLanguage", "Please select primary language");
                if (model.StudentsInfo.EnglishExamType.Length < 3)
                    ModelState.AddModelError("EnglishExamType", "Please select english exam type");
                if (model.StudentsInfo.Intake.Length < 3)
                    ModelState.AddModelError("Intake", "Please select intake exam");
                if (model.StudentsInfo.Program == null || model.StudentsInfo.Program == "-1" || model.StudentsInfo.Program.Length < 3)
                    ModelState.AddModelError("Program", "Please select program type");
                if (model.StudentsInfo.IsRefusedVisa == null)
                    ModelState.AddModelError("Refused", "Please select (Have you refused any visa?)");
                if (model.StudentsInfo.HaveStudyPermitVisa == null || model.StudentsInfo.HaveStudyPermitVisa == "-1" || model.StudentsInfo.HaveStudyPermitVisa.Length < 3)
                    ModelState.AddModelError("Permit", "Please select (Do you have a valid study permit / visa? *)");
                return View("../Identity/IdentityView", model);
            }

            var data1 = new AspNetUsers()
            {
                FullName = model.Users.FullName
            };

            var data2 = new AspNetStudentsInfo()
            {
                ContactNumber = model.StudentsInfo.ContactNumber,
                DOB = model.StudentsInfo.DOB,
                AddressLine1 = model.StudentsInfo.AddressLine1,
                AddressLine2 = model.StudentsInfo.AddressLine2,
                CityId = model.Info.CityId,
                Zip = model.StudentsInfo.Zip,
                Reference = model.StudentsInfo.Reference,
                PrimaryLanguage = model.StudentsInfo.PrimaryLanguage,
                EnglishExamType = model.StudentsInfo.EnglishExamType,
                Intake = model.StudentsInfo.Intake,
                IntakeYear = model.StudentsInfo.IntakeYear,
                Program = model.StudentsInfo.Program,
                ProgramCollegePreference = model.StudentsInfo.ProgramCollegePreference,
                /*
                HighestEducation = model.StudentsInfo.HighestEducation,
                MastersEducationStartDate = model.StudentsInfo.MastersEducationStartDate,
                MastersEducationEndDate = model.StudentsInfo.MastersEducationEndDate,
                MastersEducationCompletionDate = model.StudentsInfo.MastersEducationCompletionDate,
                MastersInstituteInfo = model.StudentsInfo.MastersInstituteInfo,
                MastersEducationPercentage = model.StudentsInfo.MastersEducationPercentage,
                MastersEducationMathsmarks = model.StudentsInfo.MastersEducationMathsmarks,
                MastersEducationEnglishMarks = model.StudentsInfo.MastersEducationEnglishMarks,
                BachelorsEducationStartDate = model.StudentsInfo.BachelorsEducationStartDate,
                BachelorsEducationEndDate = model.StudentsInfo.BachelorsEducationEndDate,
                BachelorsEducationCompletionDate = model.StudentsInfo.BachelorsEducationCompletionDate,
                BachelorsInstituteInfo = model.StudentsInfo.BachelorsInstituteInfo,
                BachelorsEducationPercentage = model.StudentsInfo.BachelorsEducationPercentage,
                BachelorsEducationMathsmarks = model.StudentsInfo.BachelorsEducationMathsmarks,
                BachelorsEducationEnglishMarks = model.StudentsInfo.BachelorsEducationEnglishMarks,
                SecondaryEducationStartDate = model.StudentsInfo.SecondaryEducationStartDate,
                SecondaryEducationEndDate = model.StudentsInfo.SecondaryEducationEndDate,
                SecondaryEducationCompletionDate = model.StudentsInfo.SecondaryEducationCompletionDate,
                SecondaryInstituteInfo = model.StudentsInfo.SecondaryInstituteInfo,
                SecondaryEducationPercentage = model.StudentsInfo.SecondaryEducationPercentage,
                SecondaryEducationMathsmarks = model.StudentsInfo.SecondaryEducationMathsmarks,
                SecondaryEducationEnglishMarks = model.StudentsInfo.SecondaryEducationEnglishMarks,
                MatricEducationStartDate = model.StudentsInfo.MatricEducationStartDate,
                MatricEducationEndDate = model.StudentsInfo.MatricEducationEndDate,
                MatricEducationCompletionDate = model.StudentsInfo.MatricEducationCompletionDate,
                MatricInstituteInfo = model.StudentsInfo.MatricInstituteInfo,
                MatricEducationPercentage = model.StudentsInfo.MatricEducationPercentage,
                MatricEducationMathsmarks = model.StudentsInfo.MatricEducationMathsmarks,
                MatricEducationEnglishMarks = model.StudentsInfo.MatricEducationEnglishMarks,
                */
                CompanyName = model.StudentsInfo.CompanyName,
                JobTitle = model.StudentsInfo.JobTitle,
                JobStartDate = model.StudentsInfo.JobStartDate,
                JobEndDate = model.StudentsInfo.JobEndDate,
                IsRefusedVisa = true,
                ExplainIfRefused = model.StudentsInfo.ExplainIfRefused,
                HaveStudyPermitVisa = model.StudentsInfo.HaveStudyPermitVisa
            };

            
            var ExistingUser = User.FindFirst(ClaimTypes.Email)?.Value;
            if (ExistingUser != null)
            {
                data1.Email = ExistingUser;
            }
            //
            var res = await _serviceClient.CreateClientAsync(data1, data2);
            if (res < 0)
            {
                _logger.LogError("Email is already in use. Exception raised from ClientController.UpdateClient");
                ModelState.AddModelError("ExistingEmail", "Email already exist.");
                return View(model);
            }
            if (res == 0 && ExistingUser == null)
            {
                _logger.LogError("Res & ExistingUser returned form ClientController.UpdateClient");
                return RedirectToAction("IdentityView", "Identity");
            }
            return RedirectToAction("IdentityView", "Identity", new { IsSuccess = true });
        }

        
        [HttpPost]
        public IActionResult UploadAsyncc(_InformationModel model)
        {
            if(model.ClientDocs == null)
            {
                return RedirectToAction("IdentityView", "Identity", new { IsFailed = true });
            }
            if (model.ClientDocs != null)
            {
                if (model.ClientDocs.Passport != null)
                {
                    string passport = Path.GetExtension(model.ClientDocs.Passport.FileName);
                    passport = passport.ToLower();
                    if (passport != ".jpg" && passport != ".png" && passport != ".pdf" && passport != ".jpeg")
                    {
                        ModelState.AddModelError("Passport", "Invalid file format");
                        return View("../IdentityView/Identity", model);
                    }
                }

                if (model.ClientDocs.EnglishExam != null)
                {
                    string englishExam = Path.GetExtension(model.ClientDocs.EnglishExam.FileName);
                    englishExam = englishExam.ToLower();
                    if (englishExam != ".jpg" && englishExam != ".png" && englishExam != ".pdf" && englishExam != ".jpeg")
                    {
                        ModelState.AddModelError("English", "Invalid file format");
                        return View("../IdentityView/Identity", model);
                    }
                }

                if (model.ClientDocs.Matriculation != null)
                {
                    string matriculation = Path.GetExtension(model.ClientDocs.Matriculation.FileName);
                    matriculation = matriculation.ToLower();
                    if (matriculation != ".jpg" && matriculation != ".png" && matriculation != ".pdf" && matriculation != ".jpeg")
                    {
                        ModelState.AddModelError("Matriculation", "Invalid file format");
                        return View("../IdentityView/Identity", model);
                    }
                }

                if (model.ClientDocs.SeniorSecondary != null)
                {
                    string senior = Path.GetExtension(model.ClientDocs.SeniorSecondary.FileName);
                    senior = senior.ToLower();
                    if (senior != ".jpg" && senior != ".png" && senior != ".pdf" && senior != ".jpeg")
                    {
                        ModelState.AddModelError("Senior", "Invalid file format");
                        return View("../IdentityView/Identity", model);
                    }
                }

                if (model.ClientDocs.BachelorsDegree != null)
                {
                    string bachelors = Path.GetExtension(model.ClientDocs.BachelorsDegree.FileName);
                    bachelors = bachelors.ToLower();
                    if (bachelors != ".jpg" && bachelors != ".png" && bachelors != ".pdf" && bachelors != ".jpeg")
                    {
                        ModelState.AddModelError("Bachelors", "Invalid file format");
                        return View("../IdentityView/Identity", model);
                    }
                }

                if (model.ClientDocs.WorkExperience != null)
                {
                    string workExperience = Path.GetExtension(model.ClientDocs.WorkExperience.FileName);
                    workExperience = workExperience.ToLower();
                    if (workExperience != ".jpg" && workExperience != ".png" && workExperience != ".pdf" && workExperience != ".jpeg")
                    {
                        ModelState.AddModelError("WorkExperience", "Invalid file format");
                        return View("../IdentityView/Identity", model);
                    }
                }
            }
            if (model.ClientDocs != null)
            {
                if (model.ClientDocs.EnglishExam != null && model.ClientDocs.EnglishExam.Length > 3145728)
                {
                    ModelState.AddModelError("EnglishExam", "File size should not exceed 3MB");
                    return View("../IdentityView/Identity", model);
                }

                if (model.ClientDocs.Matriculation != null && model.ClientDocs.Matriculation.Length > 3145728)
                {
                    ModelState.AddModelError("Matriculation", "File size should not exceed 3MB");
                    return View("../IdentityView/Identity", model);
                }

                if (model.ClientDocs.SeniorSecondary != null && model.ClientDocs.SeniorSecondary.Length > 3145728)
                {
                    ModelState.AddModelError("Secondary", "File size should not exceed 3MB");
                    return View("../IdentityView/Identity", model);
                }

                if (model.ClientDocs.BachelorsDegree != null && model.ClientDocs.BachelorsDegree.Length > 3145728)
                {
                    ModelState.AddModelError("Bachelors", "File size should not exceed 3MB");
                    return View("../IdentityView/Identity", model);
                }

                if (model.ClientDocs.WorkExperience != null && model.ClientDocs.WorkExperience.Length > 3145728)
                {
                    ModelState.AddModelError("WorkExperience", "File size should not exceed 3MB");
                    return View("../IdentityView/Identity", model);
                }
            }
            AspNetUsersDocs data3 = new AspNetUsersDocs();
            string rootPath = _hostingEnvironment.WebRootPath;
            var UserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (model.ClientDocs != null)
            {
                data3.Document = new List<Microsoft.AspNetCore.Http.IFormFile>();
                data3.Document.Add(model.ClientDocs.Passport);
                data3.Document.Add(model.ClientDocs.EnglishExam);
                data3.Document.Add(model.ClientDocs.SeniorSecondary);
                data3.Document.Add(model.ClientDocs.BachelorsDegree);
                data3.Document.Add(model.ClientDocs.Matriculation);
                data3.Document.Add(model.ClientDocs.WorkExperience);
            }
            var res = _serviceClient.MultipleFileUploadAsync(data3, rootPath, 0, UserEmail);
            if(res.Count > 0)
                return RedirectToAction("IdentityView", "Identity", new { IsDuplicate = true });
            return RedirectToAction("IdentityView", "Identity", new { IsSuccess = true });
        }
    }