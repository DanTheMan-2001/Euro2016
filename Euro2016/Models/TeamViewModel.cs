using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Euro2016.Models
{
    public class TeamViewModel
    {
        public List<Team> Teams { get; set; }
        public List<Group> Groups { get; set; }

        [Required] //this makes the field mandoratory and will make the model state invalid if not filled in
        public string Name { get; set; }
        [Required] //this makes the field mandoratory and will make the model state invalid if not filled in
        public string Flag { get; set; }

        [Display(Name="Group")]
        public int GroupId { get; set; }

        public int Id { get; set; }
    }
}