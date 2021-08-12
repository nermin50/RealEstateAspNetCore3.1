using System.Collections.Generic;

namespace RealEstateAspNetCore3._1.Models
{
    public class District
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        //Many to one = Many District in one  City
        public int CityId { get; set; }
        public virtual City City { get; set; }

        //LList Mahalla 

        public List<Neighborhood> Neighborhoods { get; set; }
    }
}
