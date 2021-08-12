using System.Collections.Generic;

namespace RealEstateAspNetCore3._1.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }

        //Many to one = Many District(semt) in one city (şehir)
        public List<District> Districts { get; set; }
    }
}
