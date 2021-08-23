using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAspNetCore3._1.Models
{
    public class AdvPhoto
    {
        /*bu tabloda her ilanın resimlerini kaydedecez sadce resimin adını */
        public int AdvPhotoId { get; set; }
        public string AdvPhotoName { get; set; }

        /*Many To One Many photo to one ilan*/
        public int AdvId { get; set; }
        [ForeignKey("AdvId")]
        public virtual  Advertisement Advertisement { get; set; }
    }
}
