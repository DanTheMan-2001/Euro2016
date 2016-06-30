using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Euro2016.Models
{
    public class FixturesViewModel
    {
        public List<Fixture> Fixtures { get; set; }
        public List<Team> Teams { get; set; }
        public List<Venue> Venues { get; set; }

        [Required]
        [Display(Name ="Home Team")]
        public int HomeTeamId { get; set; }
        [Required]
        [Display(Name = "Away Team")]
        public int AwayTeamId { get; set; }
        [Required]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }
        [Required]
        public DateTime? FixtureDate { get; set; }

        public int Id { get; set; }
    }
}