using Imm.DAL.Data;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<int> CreateClientAsync(AspNetUsers user, AspNetStudentsInfo userr, AspNetUsersDocs data, string path, int agentId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.AspNetUsers.Add(user);
                var err =  _context.SaveChanges();
                
                // Assigning role to user
                var role = new AspNetRoles()
                {
                    UserId = user.UserId,
                    Role = "client"
                };

                _context.AspNetRoles.Add(role);
                _context.SaveChanges();

                userr.UserId = role.UserId;
                _context.AspNetStudentsInfo.Add(userr);

                // Uploading Docs
                await MultipleFileUploadAsync(data, path, userr.UserId);

                // Assigning user to agent
                var member = new AspNetUsersManager()
                {
                    AgentId = agentId,
                    StudentId = role.UserId,
                };

                _context.AspNetUsersManager.Add(member);
                await _context.SaveChangesAsync();

                // Committing transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                if (ex.InnerException.Message.Contains("duplicate"))
                    user.UserId = (-1);
            }
            return user.UserId;
        }

        public async Task<List<string>> MultipleFileUploadAsync(AspNetUsersDocs _file, string rootPath, int userId)
        {
            string uniqueFileName = null;
            string filePath = null;
            List<string> url = new List<string>();
            var uId = userId;
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

                        item.CopyTo(new FileStream(filePath, FileMode.Create));
                        _logger.LogError("File uploaded succesfully, writing to database.");
                        url.Add(filePath);

                        AspNetUsersDocs finFile = new AspNetUsersDocs()
                        {
                            DocumentName = uniqueFileName,
                            DocumentURL = filePath,
                            UserId = uId
                        };

                        if (count == 1)
                            finFile.Type = "Passport";
                        if (count == 2)
                            finFile.Type = "English Exam Certificate";
                        if (count == 3)
                            finFile.Type = "Matriculation Certificate";
                        if (count == 4)
                            finFile.Type = "Secondary Certificate";
                        if (count == 5)
                            finFile.Type = "Bachelors Certificate";
                        if (count == 6)
                            finFile.Type = "Experience Certificate";

                        _context.AspNetUserDocs.Add(finFile);
                        await _context.SaveChangesAsync();
                    }
                    continue;
                }
                return url;
            }
            _logger.LogError("Something error in ServiceClient");
            return null;
        }
    }
}
