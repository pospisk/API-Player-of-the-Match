using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPotG.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime MatchDate { get; set; }
        public int Opponent { get; set; }
        public int Sponsor { get; set; }
    }
}