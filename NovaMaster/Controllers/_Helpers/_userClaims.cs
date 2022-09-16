using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NovaMaster.Controllers._Helpers
{
    public class _userClaims: Controller
    {
        // Returns user Email
        public string UserEmail()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            return userEmail;
        }

        // Returns user FullName
        public string FullName()
        {
            var FullName = User.FindFirst(ClaimTypes.Name)?.Value;
            return FullName;
        }

        // Return user role
        public string Role()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return userRole;
        }
    }
}
