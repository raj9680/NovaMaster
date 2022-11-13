using Imm.DAL.Data;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Imm.BLL
{
    public class ServiceClient
    {
        private readonly ILogger<AspNetUsers> _logger;
        private readonly NovaDbContext _context;

        public ServiceClient(NovaDbContext context, ILogger<AspNetUsers> logger)
        {
            _context = context;
            _logger = logger;
        }

        public AspNetStudentsInfo ClientViewInfoAsync(string email)
        {
            var res = _context.AspNetUsers.Where(o => o.Email == email).FirstOrDefault();
            if (res == null)
            {
                _logger.LogWarning("Null from ServiceIdentity.IdentityViewInfoAsync");
                return null;
            }
            return _context.AspNetStudentsInfo.Where(e => e.UserId == res.UserId).FirstOrDefault();
        }

        public bool IsClientExistAsync(string email)
        {
            var res = _context.AspNetUsers.Where(o => o.Email == email).FirstOrDefault();
            if (res != null)
            {
                return true;
            }
            _logger.LogWarning("User does not exist from ServiceClient.IsClientExistAsync");
            return false;
        }

        public async Task<int> CreateClientAsync(AspNetUsers user, AspNetStudentsInfo userr, [Optional] AspNetUsersDocs data, [Optional] string path, [Optional] int agentId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var uids = _context.AspNetUsers.Where(x => x.Email == user.Email).Select(u => u.UserId).FirstOrDefault();
                var ClientDetail = await _context.AspNetStudentsInfo.FindAsync(uids);
                
                if (ClientDetail != null)
                {
                    AspNetUsers ClientInfo = new AspNetUsers();
                    ClientInfo.FullName = user.FullName; // Client name not getting updated from ClientDashboard
                    ClientDetail.ContactNumber = userr.ContactNumber;
                    ClientDetail.DOB = userr.DOB;
                    ClientDetail.AddressLine1 = userr.AddressLine1;
                    ClientDetail.AddressLine2 = userr.AddressLine2;
                    ClientDetail.CityId = userr.CityId;
                    ClientDetail.Zip = userr.Zip;
                    ClientDetail.Reference = userr.Reference;
                    ClientDetail.PrimaryLanguage = userr.PrimaryLanguage;
                    ClientDetail.EnglishExamType = userr.EnglishExamType;
                    ClientDetail.Intake = userr.Intake;
                    ClientDetail.IntakeYear = userr.IntakeYear;
                    ClientDetail.Program = userr.Program;
                    ClientDetail.ProgramCollegePreference = userr.ProgramCollegePreference;
                    ClientDetail.HighestEducation = userr.HighestEducation;
                    // Masters
                    ClientDetail.MastersEducationStartDate = userr.MastersEducationStartDate;
                    ClientDetail.MastersEducationEndDate = userr.MastersEducationEndDate;
                    ClientDetail.MastersEducationCompletionDate = userr.MastersEducationCompletionDate;
                    ClientDetail.MastersInstituteInfo = userr.MastersInstituteInfo;
                    ClientDetail.MastersEducationPercentage = userr.MastersEducationPercentage;
                    ClientDetail.MastersEducationMathsmarks = userr.MastersEducationMathsmarks;
                    ClientDetail.MastersEducationEnglishMarks = userr.MastersEducationEnglishMarks;
                    // Bachelors
                    ClientDetail.BachelorsEducationStartDate = userr.BachelorsEducationStartDate;
                    ClientDetail.BachelorsEducationEndDate = userr.BachelorsEducationEndDate;
                    ClientDetail.BachelorsEducationCompletionDate = userr.BachelorsEducationCompletionDate;
                    ClientDetail.BachelorsInstituteInfo = userr.BachelorsInstituteInfo;
                    ClientDetail.BachelorsEducationPercentage = userr.BachelorsEducationPercentage;
                    ClientDetail.BachelorsEducationMathsmarks = userr.BachelorsEducationMathsmarks;
                    ClientDetail.BachelorsEducationEnglishMarks = userr.BachelorsEducationEnglishMarks;
                    // Secondary
                    ClientDetail.SecondaryEducationStartDate = userr.SecondaryEducationStartDate;
                    ClientDetail.SecondaryEducationEndDate = userr.SecondaryEducationEndDate;
                    ClientDetail.SecondaryEducationCompletionDate = userr.SecondaryEducationCompletionDate;
                    ClientDetail.SecondaryInstituteInfo = userr.SecondaryInstituteInfo;
                    ClientDetail.SecondaryEducationPercentage = userr.SecondaryEducationPercentage;
                    ClientDetail.SecondaryEducationMathsmarks = userr.SecondaryEducationMathsmarks;
                    ClientDetail.SecondaryEducationEnglishMarks = userr.SecondaryEducationEnglishMarks;
                    // Matriculation
                    ClientDetail.MatricEducationStartDate = userr.MatricEducationStartDate;
                    ClientDetail.MatricEducationEndDate = userr.MatricEducationEndDate;
                    ClientDetail.MatricEducationCompletionDate = userr.MatricEducationCompletionDate;
                    ClientDetail.MatricInstituteInfo = userr.MatricInstituteInfo;
                    ClientDetail.MatricEducationPercentage = userr.MatricEducationPercentage;
                    ClientDetail.MatricEducationMathsmarks = userr.MatricEducationMathsmarks;
                    ClientDetail.MatricEducationEnglishMarks = userr.MatricEducationEnglishMarks;
                    // Job
                    ClientDetail.CompanyName = userr.CompanyName;
                    ClientDetail.JobTitle = userr.JobTitle;
                    ClientDetail.JobStartDate = userr.JobStartDate;
                    ClientDetail.JobEndDate = userr.JobEndDate;
                    ClientDetail.IsRefusedVisa = userr.IsRefusedVisa;
                    ClientDetail.ExplainIfRefused = userr.ExplainIfRefused;
                    ClientDetail.HaveStudyPermitVisa = userr.HaveStudyPermitVisa;
                }
                else
                {
                    // Assigning role to user
                    var role = new AspNetRoles()
                    {
                        UserId = user.UserId,
                        Role = "client"
                    };

                    // Assigning user to agent
                    var member = new AspNetUsersManager()
                    {
                        AgentId = agentId,
                        StudentId = role.UserId,
                        DOJ = DateTime.Now.Date
                    };

                    user.IsNewRegistration = true;
                    _context.AspNetUsers.Add(user);
                    _context.SaveChanges();

                    role.UserId = user.UserId;
                    _context.AspNetRoles.Add(role);
                    _context.SaveChanges();

                    userr.UserId = role.UserId;
                    _context.AspNetStudentsInfo.Add(userr);

                    MultipleFileUploadAsync(data, path, userr.UserId);

                    member.StudentId = userr.UserId;
                    _context.AspNetUsersManager.Add(member);
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                if (ex.InnerException.Message.Contains($"The duplicate key value is ({user.Email})"))
                    user.UserId = (-1);
            }
            return user.UserId;
        }

        public List<string> MultipleFileUploadAsync(AspNetUsersDocs _file, string rootPath, int userId, [Optional] string email)
        {
            string uniqueFileName = null;
            string filePath = null;
            List<string> url = new List<string>();
            List<string> existing = new List<string>();
            var uId = userId;

            if(uId == 0 && email != null)
                uId = _context.AspNetUsers.Where(x => x.Email == email).Select(u => u.UserId).FirstOrDefault();

            if (_file.Document != null && _file.Document.Count > 0 && uId > 0)
            {
                int count = 0;
                var fName = "";
                foreach (IFormFile item in _file.Document)
                {
                    count++;
                    if (item != null)
                    {
                        if (count == 1)
                            fName = uId + "_passport";
                        if (count == 2)
                            fName = uId + "_exam";
                        if (count == 3)
                            fName = uId + "_matriculation";
                        if (count == 4)
                            fName = uId + "_secondary";
                        if (count == 5)
                            fName = uId + "_bachelors";
                        if (count == 6)
                            fName = uId + "_experience";
                        string uploadsFolder = Path.Combine(rootPath + "/images/img");
                        uniqueFileName = fName + "_" + item.FileName;
                        filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // step x-x
                        
                        _logger.LogError("File uploaded succesfully, writing to database.");
                        url.Add(filePath);

                        AspNetUsersDocs finFile = new AspNetUsersDocs()
                        {
                            DocumentName = uniqueFileName,
                            DocumentURL = filePath,
                            UserId = uId
                        };

                        if (count == 1)
                            finFile.Type = "passport";
                        if (count == 2)
                            finFile.Type = "exam";
                        if (count == 3)
                            finFile.Type = "matriculation";
                        if (count == 4)
                            finFile.Type = "secondary";
                        if (count == 5)
                            finFile.Type = "bachelors";
                        if (count == 6)
                            finFile.Type = "experience";

                        var dat = _context.AspNetUserDocs.SingleOrDefault(x => x.Type == finFile.Type && x.UserId == uId);
                        if (dat == null)
                        {
                            // step x-x
                            var obj = new FileStream(filePath, FileMode.Create);
                            item.CopyTo(obj);
                            obj.Dispose();
                            // step x-x end
                            _context.AspNetUserDocs.Add(finFile);
                            _context.SaveChanges();
                        }
                        if (dat != null)
                            existing.Add(finFile.Type);
                    }
                    continue;
                }
                return existing;
            }
            _logger.LogWarning("New client created without document upload from ServiceClient.MultipleFileUploadAsync");
            return null;
        }

        public async Task<InformationViewModel> ViewClientAsync(int id)
        {
            if(id < 0 || id == 0)
                return null;
            var role = _context.AspNetRoles.Find(id).Role;
            if (role.Equals("client"))
                return await ClientOnly(id);
            return AgentOnly(id);
        }

        public async Task<InformationViewModel> ClientOnly(int id)
        {
            if (id == 0)
                return null;
            var q = (from u in _context.AspNetUsers
                     join si in _context.AspNetStudentsInfo on u.UserId equals si.UserId
                     where u.UserId == id
                     select new { u, si });
            var doc = _context.AspNetUserDocs.Where(x => x.UserId == id).ToList();
            InformationViewModel fin = new InformationViewModel();
            foreach (var item in q)
            {
                AspNetUsers user = new AspNetUsers()
                {
                    FullName = item.u.FullName,
                    Email = item.u.Email
                };

                AspNetStudentsInfo userInfo = new AspNetStudentsInfo()
                {
                    ContactNumber = item.si.ContactNumber,
                    DOB = item.si.DOB,
                    AddressLine1 = item.si.AddressLine1,
                    AddressLine2 = item.si.AddressLine2,
                    CityId = item.si.CityId,
                    Zip = item.si.Zip,
                    Reference = item.si.Reference,
                    PrimaryLanguage = item.si.PrimaryLanguage,
                    EnglishExamType = item.si.EnglishExamType,
                    Intake = item.si.Intake,
                    IntakeYear = item.si.IntakeYear,
                    Program = item.si.Program,
                    ProgramCollegePreference = item.si.ProgramCollegePreference,
                    HighestEducation = item.si.HighestEducation,
                    MastersEducationStartDate = item.si.MastersEducationStartDate,
                    MastersEducationEndDate = item.si.MastersEducationEndDate,
                    MastersEducationCompletionDate = item.si.MastersEducationCompletionDate,
                    MastersInstituteInfo = item.si.MastersInstituteInfo,
                    MastersEducationPercentage = item.si.MastersEducationPercentage,
                    MastersEducationMathsmarks = item.si.MastersEducationMathsmarks,
                    MastersEducationEnglishMarks = item.si.MastersEducationEnglishMarks,
                    BachelorsEducationStartDate = item.si.BachelorsEducationStartDate,
                    BachelorsEducationEndDate = item.si.BachelorsEducationEndDate,
                    BachelorsEducationCompletionDate = item.si.BachelorsEducationCompletionDate,
                    BachelorsInstituteInfo = item.si.BachelorsInstituteInfo,
                    BachelorsEducationPercentage = item.si.BachelorsEducationPercentage,
                    BachelorsEducationEnglishMarks = item.si.BachelorsEducationEnglishMarks,
                    BachelorsEducationMathsmarks = item.si.BachelorsEducationMathsmarks,
                    SecondaryEducationStartDate = item.si.SecondaryEducationStartDate,
                    SecondaryEducationEndDate = item.si.SecondaryEducationEndDate,
                    SecondaryEducationCompletionDate = item.si.SecondaryEducationCompletionDate,
                    SecondaryInstituteInfo = item.si.SecondaryInstituteInfo,
                    SecondaryEducationPercentage = item.si.SecondaryEducationPercentage,
                    SecondaryEducationMathsmarks = item.si.SecondaryEducationMathsmarks,
                    SecondaryEducationEnglishMarks = item.si.SecondaryEducationEnglishMarks,
                    MatricEducationStartDate = item.si.MatricEducationStartDate,
                    MatricEducationEndDate = item.si.MatricEducationEndDate,
                    MatricEducationCompletionDate = item.si.MatricEducationCompletionDate,
                    MatricInstituteInfo = item.si.MatricInstituteInfo,
                    MatricEducationPercentage = item.si.MatricEducationPercentage,
                    MatricEducationMathsmarks = item.si.MatricEducationMathsmarks,
                    MatricEducationEnglishMarks = item.si.MatricEducationEnglishMarks,
                    CompanyName = item.si.CompanyName,
                    JobTitle = item.si.JobTitle,
                    JobStartDate = item.si.JobStartDate,
                    JobEndDate = item.si.JobEndDate,
                    IsRefusedVisa = item.si.IsRefusedVisa,
                    ExplainIfRefused = item.si.ExplainIfRefused,
                    HaveStudyPermitVisa = item.si.HaveStudyPermitVisa
                };

                fin.AspUsersModel = user;
                fin.AspStudentsInfoModel = userInfo;
                fin.AspUserDocsModel = doc;
            }

            var userr = await _context.AspNetUsers.FindAsync(id);
            if (userr != null)
            {
                userr.IsNewRegistration = false;
                await _context.SaveChangesAsync();
            }

            return fin;
        }

        public InformationViewModel AgentOnly(int id)
        {
            if (id == 0)
                return null;
            var q = (from u in _context.AspNetUsers
                     join ui in _context.AspNetUsersInfo on u.UserId equals ui.UserId
                     where u.UserId == id
                     select new { u, ui });
            var doc = _context.AspNetUserDocs.Where(x => x.UserId == id).ToList();
            InformationViewModel fin = new InformationViewModel();
            foreach (var item in q)
            {
                AspNetUsers user = new AspNetUsers()
                {
                    FullName = item.u.FullName,
                    Email = item.u.Email
                };

                AspNetUsersInfo userInfo = new AspNetUsersInfo()
                {
                    ContactNumber = item.ui.ContactNumber,
                    Website = item.ui.Website,
                    CompanyName = item.ui.CompanyName,
                    StudentSource = item.ui.StudentSource,
                    AddressLine1 = item.ui.AddressLine1,
                    AddressLine2 = item.ui.AddressLine2,
                    CityId = item.ui.CityId,
                    PinCode = item.ui.PinCode,
                    About = item.ui.PinCode
                };

                fin.AspUsersModel = user;
                fin.AspUsersInfoModel = userInfo;
                fin.AspUserDocsModel = doc;
            }
            return fin;
        }
    }
}
