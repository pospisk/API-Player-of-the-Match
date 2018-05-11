using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Web.WebApi;

namespace ApiPotG.Controllers
{
    public class MatchController : UmbracoApiController
    {
        /// <summary>
        /// get matches from a team id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns>list of matces with ids of sponsors and opponents</returns>
        [HttpGet]
        public List<Match> GetMatches(int teamId)
        {
            var cs = Services.ContentService;
            List<Match> res = new List<Match>();
            var matches = cs.GetDescendants(teamId);

            foreach (var match in matches)
            {
                if (match.ContentType.Alias == "match")
                {
                    var m = new Match
                    {
                        id = match.Id,
                        name = match.Name,
                        matchDate = DateTime.Parse(match.Properties["matchStart"].Value.ToString()),
                        
                    };

                    try
                    {
                        string guid = match.Properties["matchOpponent"].Value.ToString();
                        var uid = Udi.Parse(guid);
                        var media = Umbraco.GetIdForUdi(uid);
                        m.opponent = media;
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    try
                    {
                        string guid = match.Properties["matchSponsors"].Value.ToString();
                        var uid = Udi.Parse(guid);
                        var media = Umbraco.GetIdForUdi(uid);
                        m.sponsor = media;
                    }
                    catch(NullReferenceException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    //var opponent = 0;
                    //bool opponentParses = int.TryParse(match.Properties["matchOpponent"].Value.ToString(), out opponent);
                    //m.opponent = opponentParses ? opponent : 0;

                    res.Add(m);
                }
            }

            return res;
        }
    }
}