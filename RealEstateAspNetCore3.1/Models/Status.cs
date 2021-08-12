using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAspNetCore3._1.Models
{
    public class Status
    {
        [Key] /*Primary Key*/
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        /*Emlakın tiplerini listelemek için */
        public List<Tip> Tips { get; set; }
    }
}
