using System.ComponentModel.DataAnnotations;

namespace NovaMaster.Models
{
    public class ModelAspNetUsers
    {
        public int AgentId { get; set; } // optional
        public int UserId { get; set; }

        [Required(ErrorMessage = "Agent name required.")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [MinLength(8, ErrorMessage = ("Password must contain at least 8 characters"))]
        [MaxLength(32)]
        public string CnfPassword { get; set; }
        public bool CnfEmail { get; set; }
        public bool IsActive { get; set; }
        public bool IsNewRegistration { get; set; }
    }

    public class ResetPassword
    {
        public int annotation { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password does not match")]
        [MinLength(8, ErrorMessage = ("Password must contain at least 8 characters"))]
        [MaxLength(32)]
        public string ConfirmPassword { get; set; }
    }
}
