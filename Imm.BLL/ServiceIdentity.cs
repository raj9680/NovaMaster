using Imm.DAL.Data;
using Imm.DAL.Data.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imm.BLL
{
    public class ServiceIdentity
    {
        private readonly ILogger<AspNetUsers> _logger;
        private readonly NovaDbContext _context;
        public ServiceIdentity(NovaDbContext context, ILogger<AspNetUsers> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<AspNetUsers> IdentityViewAsync(string email)
        {
            var res = _context.AspNetUsers.Where(o => o.Email == email).FirstOrDefault();
            if(res != null)
            {
                return res;
            }
            _logger.LogWarning("Null from ServiceIdentity.IdentityViewAsync");
            return res;
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

        public async Task<AspNetUsersInfo> IdentityViewInfoAsync(string email)
        {
            var res = _context.AspNetUsers.Where(o => o.Email == email).FirstOrDefault();
            if(res == null)
            {
                _logger.LogWarning("Null from ServiceIdentity.IdentityViewInfoAsync");
                return null;
            }
            return _context.AspNetUsersInfo.Where(e => e.UserId == res.UserId).FirstOrDefault();
        }

        public async Task<int> UpdateAsync(AspNetUsers model1, AspNetUsersInfo model2)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var user = await _context.AspNetUsersInfo.FindAsync(model2.UserId);
                if (user != null)
                {
                    user.DOB = Convert.ToDateTime(model2.DOB).Date;
                    user.CityId = 3;
                    user.ContactNumber = model2.ContactNumber;
                    user.AddressLine1 = model2.AddressLine1;
                    user.AddressLine2 = model2.AddressLine2;
                    user.CompanyName = model2.CompanyName;
                    user.StudentSource = model2.StudentSource;
                    user.Website = model2.Website;
                    user.About = model2.About;
                    user.PinCode = model2.PinCode;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    var userr = new AspNetUsersInfo();

                    userr.UserId = model2.UserId;
                    userr.DOB = Convert.ToDateTime(model2.DOB).Date;
                    userr.CityId = 3;
                    userr.ContactNumber = model2.ContactNumber;
                    userr.AddressLine1 = model2.AddressLine1;
                    userr.AddressLine2 = model2.AddressLine2;
                    userr.Website = model2.Website;
                    userr.StudentSource = model2.StudentSource;
                    userr.CompanyName = model2.CompanyName;
                    userr.About = model2.About;
                    userr.PinCode = model2.PinCode;

                    _context.AspNetUsersInfo.Add(userr);

                    int IsManager = _context.AspNetUsersManager.Where(o => o.StudentId == userr.UserId).Select(x => x.StudentId).FirstOrDefault();
                    if (IsManager == 0 || IsManager < 0)
                    {
                        var userManager = new AspNetUsersManager();
                        userManager.StudentId = userr.UserId;
                        userManager.AgentId = 1;
                        userManager.DOJ = DateTime.Now.Date;
                        _context.AspNetUsersManager.Add(userManager);
                    }

                    // Activate account
                    AspNetUsers data = await _context.AspNetUsers.FindAsync(model1.UserId);
                    data.IsActive = true;

                    await _context.SaveChangesAsync();
                }

                /// ASPNetUsers
                var _user = await _context.AspNetUsers.FindAsync(model1.UserId);
                if (_user != null)
                {
                    _user.FullName = model1.FullName;
                    _user.Email = model1.Email;

                    await _context.SaveChangesAsync();
                }
                /// need to use transaction in upload file as well.
                transaction.Commit();
            }
            catch (Exception)
            {
                _logger.LogError("There is something issue in committing transaction of AspNetUsers or AspNetUsersInfo");
                return 0;
            }
            return 1;
        }

        public List<AspNetUsersDocs> GetDocsAsync(string email)
        {
            var res = _context.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
            var result = _context.AspNetUserDocs.Where(i => i.UserId == res.UserId);
            return result.ToList();
        }
    }
}