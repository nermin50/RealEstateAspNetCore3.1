using System.ComponentModel.DataAnnotations;

namespace RealEstateAspNetCore3._1.Models
{
    public class Tip
    {
        [Key] /*Primary Key */
        public int TypeId { get; set; }
        public string TypeName { get; set; }


        /*Status Type Many to one **/
        public int StatusId { get; set; }
        public Status Status { get; set; }
    }
}
