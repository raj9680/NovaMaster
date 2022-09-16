using Imm.DAL.Data;
using Imm.DAL.Data.Table;
using JWT.Auth;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imm.BLL
{
    public class ServiceAccess
    {
        private readonly ILogger<AspNetUsers> _logger;
        private readonly NovaDbContext _context;
        private readonly JwtAuthenticationManager _jwtAuth;

        public ServiceAccess(NovaDbContext context, JwtAuthenticationManager jwtAuth, ILogger<AspNetUsers> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtAuth = jwtAuth ?? throw new ArgumentNullException(nameof(jwtAuth));
            _logger = logger;
        }

        

        public IDictionary<string, string> LoginAsync(AspNetUsers user)
        {
            var _user = _context.AspNetUsers.FirstOrDefault(o => o.Email == user.Email && o.Password == user.Password);
            if (_user == null)
                return null;
            try
            {
                var _role = _context.AspNetRoles.SingleOrDefault(s => s.UserId == _user.UserId);
                if (_role == null)
                    return null;
                AspNetRoles role = new AspNetRoles()
                {
                    Role = _role.Role
                };
                user.FullName = _role.AspNetUsers.FullName;
                return _jwtAuth.Authenticate(user, role);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetUserId(string email)
        {
            int userId = _context.AspNetUsers.Where(x => x.Email == email).Select(o => o.UserId).FirstOrDefault();
            return userId;
        }


        public async Task<int> RegisterAsync(AspNetUsers user)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                user.Password = user.Password;
                await _context.AspNetUsers.AddAsync(user);
                var err = await _context.SaveChangesAsync();

                var role = new AspNetRoles()
                {
                    UserId = user.UserId,
                    Role = "agent"
                };

                await _context.AspNetRoles.AddAsync(role);
                await _context.SaveChangesAsync();
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


        public async Task<string> ChangePasswordAsync(string email, string Password)
        {
            int Id = _context.AspNetUsers.Where(x => x.Email == email).Select(o => o.UserId).FirstOrDefault();
            if(Id > 0)
            {
                var user = await _context.AspNetUsers.FindAsync(Id);
                user.Password = Password;

                await _context.SaveChangesAsync();
                return user.Password;
            }

            return null;
        }
    }
}
