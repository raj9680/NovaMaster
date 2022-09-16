using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imm.DAL.Data.Table
{
    public class AspNetUsersManager
    {
        [Key, ForeignKey("AspNetUsers")]
        public int StudentId { get; set; }
        public int AgentId { get; set; }
        public DateTime DOJ { get; set; }

        // Navigation to AspNetUsersInfo
        public AspNetUsers AspNetUsers { get; set; }
    }
}
