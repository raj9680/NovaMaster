using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imm.DAL.Data.Table
{
    public class AspNetStudentsInfo
    {
        [Key, ForeignKey("AspNetUsers")]
        public int UserId { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public int? Zip { get; set; }
        public string Reference { get; set; }

        // ENGLISH EXAM(IELTS, TOFEL, PTE ETC.)
        public string PrimaryLanguage { get; set; }
        public string EnglishExamType { get; set; }

        // COURSE DETAILS
        public string Intake { get; set; }
        public int? IntakeYear { get; set; }
        public string Program { get; set; }
        public string ProgramCollegePreference { get; set; }

        // Education Details
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

        // Navigation property returns the AspNetUsers Object
        public AspNetUsers AspNetUsers { get; set; }
        public City City { get; set; }
    }
}
