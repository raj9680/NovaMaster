using System;
using System.Collections.Generic;
using System.Text;

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
}
