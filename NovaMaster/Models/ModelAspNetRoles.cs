using System.ComponentModel.DataAnnotations;

namespace NovaMaster.Models
{
    public class ModelAspNetRoles
    {
        public int UserId { get; set; }

        [Required]
        public string Role { get; set; }
        // public string RefreshToken { get; set; }
    }
}
