using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imm.DAL.Data.Table
{
    public class AspNetUsersInfo
    {
        [Key, ForeignKey("AspNetUsers")]
        public int UserId { get; set; }
        public DateTime DOB { get; set; }
        public string ContactNumber { get; set; }
        public string Website { get; set; }
        public string CompanyName { get; set; }
        public string StudentSource { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        
        [ForeignKey("City")]
        public int CityId { get; set; }
        public string PinCode { get; set; }
        public string About { get; set; }

        // Navigation property returns the AspNetUsers Object
        public AspNetUsers AspNetUsers { get; set; }
        public City City { get; set; }
        public ICollection<AspNetUsersDocs> AspNetUsersDocs { get; set; }
    }


    // Outer 
    //public class _Information
    //{
    //    public AspNetUsers Users { get; set; }
    //    public AspNetUsersInfo Info { get; set; }
    //    public List<AspNetUsersDocs> Docs { get; set; }
    //}
}
