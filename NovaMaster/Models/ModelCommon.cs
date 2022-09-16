namespace NovaMaster.Models
{
    public class ModelCommon
    {
    }

    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }


    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
