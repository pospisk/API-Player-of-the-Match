using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPotG.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Team> Teams { get; set; }
    }
}