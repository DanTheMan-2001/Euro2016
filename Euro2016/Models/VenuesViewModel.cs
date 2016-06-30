using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Euro2016.Models
{
    public class VenuesViewModel
    {
        public List<Venue> Venues { get; set; }
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}