using ApiPotG.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace ApiPotG.Controllers
{
    public class TeamController : UmbracoApiController
    {
        /// <summary>
        /// Get all the decendant teams of a parrent
        /// </summary>
        /// <param name="parrentId"></param>
        /// <returns>Teams from the defined club</returns>
        [HttpGet]
        public List<Team> GetTeams(int parrentId)
        {
            var cs = Services.ContentService;
            List<Team> res = new List<Team>();
            var teams = cs.GetDescendants(parrentId);

            foreach (var team in teams)
            {
                if (team.ContentType.Alias == "team")
                {
                    var t = new Team
                    {
                        id = team.Id,
                        name = team.Name

                    };
                    res.Add(t);
                }
            }
            return res;
        }
    }
}