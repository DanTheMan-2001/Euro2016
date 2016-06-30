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

        [Required] //this makes the field mandoratory and will make the model state invalid if not filled in
        [Display(Name ="Home Team")]
        public int HomeTeamId { get; set; }
        [Required] //this makes the field mandoratory and will make the model state invalid if not filled in
        [Display(Name = "Away Team")]
        public int AwayTeamId { get; set; }
        [Required] //this makes the field mandoratory and will make the model state invalid if not filled in
        [Display(Name = "Venue")]
        public int VenueId { get; set; }
        [Required] //this makes the field mandoratory and will make the model state invalid if not filled in
        public DateTime? FixtureDate { get; set; }

        public int Id { get; set; }
    }
}