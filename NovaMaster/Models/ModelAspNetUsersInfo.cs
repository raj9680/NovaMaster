using System;
using System.ComponentModel.DataAnnotations;

namespace NovaMaster.Models
{
    public class ModelAspNetUsersInfo
    {
        [Required(ErrorMessage ="Contact number is required")]
        [MinLength(10, ErrorMessage ="Min. length required 10")]
        [MaxLength(15, ErrorMessage = "Max. length is 15")]
        public string ContactNumber { get; set; }
        public string Website { get; set; }
        public string CompanyName { get; set; }
        public string StudentSource { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage ="Address field is mandatory")]
        [MaxLength(ErrorMessage = "Address Line 1 field Should not exceed 60 words")]
        public string AddressLine1 { get; set; }

        [MaxLength(ErrorMessage = "Address Line 2 field Should not exceed 60 words")]
        public string AddressLine2 { get; set; }
        
        [Required(ErrorMessage ="Please select country")]
        public int CityId { get; set; }

        [Required(ErrorMessage ="Pin code is mandatory")]
        [MinLength(6, ErrorMessage ="Min. length required is 6")]
        public string PinCode { get; set; }

        [MaxLength(120, ErrorMessage ="About field Should not exceed 120 words")]
        public string About { get; set; }
    }
}
