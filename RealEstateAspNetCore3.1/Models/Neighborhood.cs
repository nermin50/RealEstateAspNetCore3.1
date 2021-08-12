namespace RealEstateAspNetCore3._1.Models
{
    public class Neighborhood
    {
        public int NeighborhoodId { get; set; }
        public string NeighborhoodName { get; set; }

        // Many to one  Many Neighborhood (mahalla) in one District (semt)
        public int DistrictId { get; set; }
        public virtual District District { get; set; }
    }
}
