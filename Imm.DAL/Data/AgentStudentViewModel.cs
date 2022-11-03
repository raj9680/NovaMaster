using System;
using System.Collections.Generic;

namespace Imm.DAL.Data.Table
{
    public class AgentStudentViewModel
    {
        public int UserId { get; set; }
        public string ContactNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOJ { get; set; }
        public bool Confirmed { get; set; }
    }

    public class InformationViewModel
    {
        public AspNetUsers AspUsersModel { get; set; }
        public AspNetStudentsInfo AspStudentsInfoModel { get; set; }
        public AspNetUsersInfo AspUsersInfoModel { get; set; }
        public List<AspNetUsersDocs> AspUserDocsModel { get; set; }
    }
}
