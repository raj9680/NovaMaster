using System;
using System.ComponentModel.DataAnnotations;

namespace NovaMaster.Models
{
    public class ModelAspNetStudentsInfo
    {
        [Required(ErrorMessage = "Contact number is required")]
        [MinLength(10, ErrorMessage = "Min. length required 10")]
        [MaxLength(15, ErrorMessage = "Max. length is 15")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "DOB value required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Address field is mandatory")]
        [MaxLength(ErrorMessage = "Address Line 1 field Should not exceed 60 words")]
        public string AddressLine1 { get; set; }

        [MaxLength(ErrorMessage = "Address Line 2 field Should not exceed 60 words")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Pin code is mandatory")]
        public int Zip { get; set; }
        public string Reference { get; set; }

        // ENGLISH EXAM(IELTS, TOFEL, PTE ETC.)
        [Required(ErrorMessage = "Please select primary language")]
        public string PrimaryLanguage { get; set; }

        [Required(ErrorMessage = "Please select exam type")]
        public string EnglishExamType { get; set; }

        // COURSE DETAILS
        [Required(ErrorMessage = "Please select intake program")]
        public string Intake { get; set; }

        [Required(ErrorMessage = "Please enter intake year")]
        public int IntakeYear { get; set; }

        [Required(ErrorMessage = "Please choose program")]
        public string Program { get; set; }
        public string ProgramCollegePreference { get; set; }

        // Education Details
        [Required(ErrorMessage = "Please select your higher education")]
        public string HighestEducation { get; set; }
        public DateTime? MastersEducationStartDate { get; set; }
        public DateTime? MastersEducationEndDate { get; set; }
        public DateTime? MastersEducationCompletionDate { get; set; }
        public string MastersInstituteInfo { get; set; }
        public string MastersEducationPercentage { get; set; }
        public string MastersEducationMathsmarks { get; set; }
        public string MastersEducationEnglishMarks { get; set; }

        // Batchelors Education Details
        public DateTime? BachelorsEducationStartDate { get; set; }
        public DateTime? BachelorsEducationEndDate { get; set; }
        public DateTime? BachelorsEducationCompletionDate { get; set; }
        public string BachelorsInstituteInfo { get; set; }
        public string BachelorsEducationPercentage { get; set; }
        public string BachelorsEducationMathsmarks { get; set; }
        public string BachelorsEducationEnglishMarks { get; set; }

        // Secondary Education Details
        public DateTime? SecondaryEducationStartDate { get; set; }
        public DateTime? SecondaryEducationEndDate { get; set; }
        public DateTime? SecondaryEducationCompletionDate { get; set; }
        public string SecondaryInstituteInfo { get; set; }
        public string SecondaryEducationPercentage { get; set; }
        public string SecondaryEducationMathsmarks { get; set; }
        public string SecondaryEducationEnglishMarks { get; set; }

        // Matriculation Education Details
        public DateTime MatricEducationStartDate { get; set; }
        public DateTime MatricEducationEndDate { get; set; }
        public DateTime MatricEducationCompletionDate { get; set; }
        public string MatricInstituteInfo { get; set; }
        public string MatricEducationPercentage { get; set; }
        public string MatricEducationMathsmarks { get; set; }
        public string MatricEducationEnglishMarks { get; set; }

        // Work Experience
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public DateTime? JobStartDate { get; set; }
        public DateTime? JobEndDate { get; set; }

        // Background Information
        public bool IsRefusedVisa { get; set; }
        public string ExplainIfRefused { get; set; }
        public string HaveStudyPermitVisa { get; set; }
    }
}
