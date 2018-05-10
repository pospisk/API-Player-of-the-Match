using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPotG.Models
{
    public class Match
    {
        public int id { get; set; }
        public DateTime matchDate { get; set; }
        public string opponent { get; set; }
    }
}