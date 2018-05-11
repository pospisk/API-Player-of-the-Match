using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiPotG.Models
{
    public class Vote
    {
        public int id { get; set; }
        public int IMEI { get; set; }
        public int matchId { get; set; }
        public int playerId { get; set; }
        public DateTime dateTime { get; set; }
        public int voteBatchId { get; set; }
    }
}