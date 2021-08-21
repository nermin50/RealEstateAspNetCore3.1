using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAspNetCore3._1.Models
{
    public class Advertisement
    {
        [Key]
        public int AdvId { get; set; }

        public string Description { get; set; }
        public double Price { get; set; }
        public string NumOfRoom { get; set; }
        public string NumOfBath { get; set; }
        public bool Credit { get; set; }
        public int Area { get; set; }
        public int Floor { get; set; }
        public string Feature { get; set; }
        public string Telephone { get; set; }
        public string Addres { get; set; }

        /*Şehir adı ve semt ve mahalla adıları ilan tablosunda görmek için Id'lerini burya yazdık */
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int StatusId { get; set; }


        /*Şehir  ve semt adına ulaşmak için Mahalla(NeighborhoodId) id'isi üzerinden ulaşablirilz*/
        public int NeighborhoodId { get; set; }
        public virtual Neighborhood Neighborhood { get; set; }

        /********************************************************************************/

        /*Her ilanın Tipini ve durumunu listelemek için */

        public int TypeId { get; set; }
        public virtual Tip Tip { get; set; }

        /*ilanın resimlerini çağrmak için */
        public List<AdvPhoto> AdvPhotos { get; set; }
    }
}
