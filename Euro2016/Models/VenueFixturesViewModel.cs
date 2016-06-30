using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Euro2016.Models
{
    public class VenueFixturesViewModel
    {
        public List<Fixture> Fixtures { get; set; }
        public Venue Venue { get; set; }
    }
}