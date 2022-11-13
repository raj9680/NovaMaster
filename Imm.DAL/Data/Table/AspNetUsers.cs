using Imm.DAL.Data.Table;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Imm.DAL.Data.Table
{
    public class AspNetUsers
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        public string FullName { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        public bool CnfEmail { get; set; }
        public bool IsActive { get; set; }
        public bool IsNewRegistration { get; set; }



        // Navigation property returns the AspNetRoles Address
        public AspNetRoles AspNetRoles { get; set; }
        public AspNetUsersInfo AspNetUsersInfo { get; set; }
        public AspNetUsersManager AspNetUsersManager { get; set; }
        public ICollection<AspNetUsersDocs> AspNetUsersDocs { get; set; }

    }
}
