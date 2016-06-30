using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Euro2016.Models
{
    public class GroupViewModel
    {
        public List<Group> Groups { get; set; }
        [Required] //this makes the field mandoratory and will make the model state invalid if not filled in
        public string Name { get; set; }
        public int Id { get; set; }

    }
}