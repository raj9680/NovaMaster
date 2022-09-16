using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imm.DAL.Data.Table
{
    public class AspNetRoles
    {
        [Required]
        [Key, ForeignKey("AspNetUsers")]
        public int UserId { get; set; }
        
        [Required]
        public string Role { get; set; }
        // public string RefreshToken { get; set; }

        // Navigation property returns the AspNetUsers Object
        public AspNetUsers AspNetUsers { get; set; }
    }
}
