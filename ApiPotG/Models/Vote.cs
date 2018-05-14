using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPotG.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public DateTime DateTime { get; set; }
        public int VoteBatchId { get; set; }
    }
}