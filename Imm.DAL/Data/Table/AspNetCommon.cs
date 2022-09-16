using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imm.DAL.Data.Table
{
    class AspNetCommon
    {
    }

    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        // Navigation property returns the AspNetRoles Address
        public ICollection<State> State { get; set; }
    }

    public class State
    {
        [Key]
        public int StateId { get; set; }
        
        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        public string StateName { get; set; }

        // Navigation property returns the AspNetUsers Object
        public Country Country { get; set; }
        public ICollection<City> City { get; set; }
    }


    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [ForeignKey("State")]
        public int StateId { get; set; }

        public string CityName { get; set; }

        // Navigation property returns the AspNetUsers Object
        public State State { get; set; }
        public List<AspNetUsersInfo> AspNetUsersInfo { get; set; }
    }
}
