using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace NovaMaster.Models
{
    public class ModelAspNetusersDocs
    {
        public int DocId { get; set; }
        public int UserId { get; set; }
        public List<IFormFile> Document { get; set; }
        public string DocumentName { get; set; }
        public string DocumentURL { get; set; }
        public string Type { get; set; }
        public bool IsVerified { get; set; }
        public string Comments { get; set; }
    }
}
