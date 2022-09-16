using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imm.DAL.Data.Table
{
    public class AspNetUsersDocs
    {
        [Key]
        public int DocId { get; set; }
 
        [Required]
        [ForeignKey("AspNetUsersInfo")]
        public int UserId { get; set; }

        [NotMapped]
        public List<IFormFile> Document { get; set; }
        public string DocumentName { get; set; }
        public string DocumentURL { get; set; }
        public string Type { get; set; }
        public bool IsVerified { get; set; }
        public string Comments { get; set; }

        // Navigation
        // public AspNetUsersInfo AspNetUsersInfo { get; set; }
    }
}
