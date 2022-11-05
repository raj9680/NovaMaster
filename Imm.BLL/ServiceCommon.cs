using Imm.DAL.Data;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Imm.BLL
{
    public class ServiceCommon
    {
        private readonly ILogger<AspNetUsers> _logger = null;
        private readonly NovaDbContext _context = null;

        public ServiceCommon(NovaDbContext context, ILogger<AspNetUsers> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public bool Isuser(string email)
        {
            var uId = _context.AspNetUsers.Where(o => o.Email == email).Select(x => x.UserId).FirstOrDefault();
            var IsUser = _context.AspNetUsersInfo.Where(o => o.UserId == uId).Select(x => x.UserId).FirstOrDefault();
            if(IsUser>0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Country>> GetCountryAsync()
        {
            return await _context.Country.Select(x => new Country()
            {
                CountryId = x.CountryId,
                CountryName = x.CountryName

            }).ToListAsync();
        }


        public async Task<List<State>> GetStateAsync(int countryId)
        {
            return await _context.State.Where(x => x.CountryId == countryId).Select(state => new State()
            {
                StateId = state.StateId,
                StateName = state.StateName
            }).ToListAsync();
        }


        public async Task<List<City>> GetCityAsync(int stateId)
        {
            return await _context.City.Where(x => x.StateId == stateId).Select(city => new City()
            {
                CityId = city.CityId,
                CityName = city.CityName
            }).ToListAsync();
        }

        public AspNetUsers GetUserInfoAsync(string email)
        {
            return _context.AspNetUsers.Where(x => x.Email == email)
                .Select(o => new AspNetUsers()
                {
                    Email = o.Email,
                    FullName = o.FullName,
                    UserId = o.UserId
                }).FirstOrDefault();
        }


        public List<string> MultipleFileUploadAsync(AspNetUsersDocs _file, string rootPath, string email)
        {
            var uId = _context.AspNetUsers.Where(x => x.Email == email).Select(o => o.UserId).FirstOrDefault();
            if (_file.Document != null && _file.Document.Count > 0 && uId > 0)
            {
                string uniqueFileName = null;
                string filePath = null;
                List<string> url = new List<string>();
                List<string> existing = new List<string>();

                int count = 0;
                var fName = "";
                foreach (IFormFile item in _file.Document)
                {
                    count++;
                    if(item != null)
                    {
                        if (count == 1)
                            fName = uId + "_logo";
                        if (count == 2)
                            fName = uId + "_cover";
                        if (count == 3)
                            fName = uId + "_certificate";
                        string uploadsFolder = Path.Combine(rootPath + "/images/img");
                        uniqueFileName = fName + "_" + item.FileName;
                        filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // step x-x
                        
                        _logger.LogInformation("File uploaded succesfully, writing to database.");
                        url.Add(filePath);

                        AspNetUsersDocs finFile = new AspNetUsersDocs()
                        {
                            DocumentName = uniqueFileName,
                            DocumentURL = filePath,
                            UserId = uId
                        };

                        if (count == 1)
                            finFile.Type = "logo";
                        if (count == 2)
                            finFile.Type = "cover";
                        if (count == 3)
                            finFile.Type = "certificate";

                        var dat = _context.AspNetUserDocs.SingleOrDefault(x => x.Type == finFile.Type);
                        if (dat == null)
                        {
                            // step x-x
                            var obj = new FileStream(filePath, FileMode.Create);
                            item.CopyTo(obj);
                            obj.Dispose();
                            // end

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
            _logger.LogError("Something error in ServiceCommon");
            return null;
        }

        
        public async Task<string> ViewFileAsync(int docId)
        {
            var res = await _context.AspNetUserDocs.FindAsync(docId);
            return res.DocumentURL.ToString();
        }


        public async Task<bool> ResetPasswordAsync(AspNetUsers model)
        {
            try
            {
                var user = await _context.AspNetUsers.FindAsync(model.UserId);
                if (user != null)
                {
                    user.Password = model.Password;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Something wrong with ServiceCommon.ResetPasswordAsync");
                return false;
            }
        }


        public string Role(int id)
        {
            if (id < 0 || id == 0)
                return null;
            var role = _context.AspNetRoles.Find(id).Role;
            return role.ToString();
        }


        public List<AgentStudentViewModel> ClientAgentViewAsync(string email)
        {
            if (email == null)
                return null;
            int userId = _context.AspNetUsers.Where(x => x.Email == email).Select(o => o.UserId).FirstOrDefault();
            List<AgentStudentViewModel> res = new List<AgentStudentViewModel>();
            string role = Role(userId);
            if (role.Contains("admin"))
            {
                var q = (from um in _context.AspNetUsersManager
                         join si in _context.AspNetUsersInfo on um.StudentId equals si.UserId
                         join u in _context.AspNetUsers on si.UserId equals u.UserId
                         where um.AgentId == userId
                         select new { um.DOJ, si.ContactNumber, si.DOB, si.UserId, u.FullName, u.Email, u.CnfEmail }
                        ).ToList();

                foreach (var item in q)
                {
                    AgentStudentViewModel data = new AgentStudentViewModel()
                    {
                        UserId = item.UserId,
                        ContactNumber = item.ContactNumber,
                        FullName = item.FullName,
                        Email = item.Email,
                        DOB = item.DOB,
                        //DOJ = item.DOJ,
                        Confirmed = item.CnfEmail
                    };
                    res.Add(data);
                }
            }
            else // all roles other than admin
            {
                var q = (from um in _context.AspNetUsersManager
                         join si in _context.AspNetStudentsInfo on um.StudentId equals si.UserId
                         join u in _context.AspNetUsers on si.UserId equals u.UserId
                         where um.AgentId == userId
                         select new { um.DOJ, si.ContactNumber, si.DOB, si.UserId, u.FullName, u.Email, u.CnfEmail }
                        ).ToList();

                foreach (var item in q)
                {
                    AgentStudentViewModel data = new AgentStudentViewModel()
                    {
                        UserId = item.UserId,
                        ContactNumber = item.ContactNumber,
                        FullName = item.FullName,
                        Email = item.Email,
                        DOB = item.DOB,
                        DOJ = item.DOJ,
                        Confirmed = item.CnfEmail
                    };
                    res.Add(data);
                }
            }
            return res;
        }

        public List<AgentStudentViewModel> ClientsAsync(int id)
        {
            if (id == 0 || id < 0)
                return null;
            List<AgentStudentViewModel> res = new List<AgentStudentViewModel>();
            var q = (from um in _context.AspNetUsersManager
                     join si in _context.AspNetStudentsInfo on um.StudentId equals si.UserId
                     join u in _context.AspNetUsers on si.UserId equals u.UserId
                     where um.AgentId == id
                     select new { um.DOJ, si.ContactNumber, si.DOB, si.UserId, u.FullName, u.Email, u.CnfEmail }
                        ).ToList();

            foreach (var item in q)
            {
                AgentStudentViewModel data = new AgentStudentViewModel()
                {
                    UserId = item.UserId,
                    ContactNumber = item.ContactNumber,
                    FullName = item.FullName,
                    Email = item.Email,
                    DOB = item.DOB,
                    DOJ = item.DOJ,
                    Confirmed = item.CnfEmail
                };
                res.Add(data);
            }
            return res;
        }

        public bool DeleteFileAsync(int docId, string docName, string _rootPath)
        {
            if(docId > 0)
            {
                AspNetUsersDocs Id = new AspNetUsersDocs()
                {
                    UserId = 4, 
                    DocId = docId,
                    DocumentName = docName
                };
                var id = _context.AspNetUserDocs.AsNoTracking().Where(x => x.DocId == Id.DocId).Select(o => new { o.DocId, o.Type }).FirstOrDefault();
                if (id.DocId > 0)
                {
                    if (File.Exists(_rootPath+"/images/img/" + Id.UserId + "_" + id.Type + "_" + Id.DocumentName))
                    {
                        File.Delete(_rootPath + "/images/img/" + Id.UserId + "_" + id.Type + "_" + Id.DocumentName);
                    }
                    _context.AspNetUserDocs.Remove(Id);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}