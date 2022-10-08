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

        public async Task<AspNetStudentsInfo> ClientViewInfoAsync(string email)
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
                        DOJ = DateTime.UtcNow.Date
                    };

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
            _logger.LogError("Something error in ServiceClient/Document not uploaded");
            return null;
        }
    }
}
