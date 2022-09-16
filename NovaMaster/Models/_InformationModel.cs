using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NovaMaster.Models
{
    public class _InformationModel
    {
        public ModelAspNetUsers Users { get; set; }
        public ModelAspNetUsersInfo Info { get; set; }
        public ModelAspNetStudentsInfo StudentsInfo { get; set; }
        public List<ModelAspNetusersDocs> Docs { get; set; }
        public _AgentDocs AgentDocs { get; set; }
        public _ClientDocs ClientDocs { get; set; }
    }

    public class _AgentDocs
    {
        // For Agents
        public IFormFile CompanyLogo { get; set; }
        public IFormFile CoverPhoto { get; set; }
        public IFormFile BusinessCertificate { get; set; }
    }

    public class _ClientDocs
    {
        public IFormFile Passport { get; set; }
        public IFormFile EnglishExam { get; set; }
        public IFormFile Matriculation { get; set; }
        public IFormFile SeniorSecondary { get; set; }
        public IFormFile BachelorsDegree { get; set; }
        public IFormFile WorkExperience { get; set; }
    }
}
