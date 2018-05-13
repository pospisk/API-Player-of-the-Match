using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web.WebApi;
namespace ApiPotG.Controllers
{
    public class ClubController : UmbracoApiController
    {
       
        // eg. /umbraco/Api/Clubs/GetClubs?id=1071
        [System.Web.Http.HttpGet]
        public List<Club> GetClubs(int id)
        {
            var cs = Services.ContentService;
            List<Club> res = new List<Club>();
            var clubs = cs.GetChildren(id);

            var tc = new TeamController();

            foreach (var club in clubs)
            {
                List<Team> teams = tc.GetTeams(club.Id);

                var c = new Club
                {
                    Id = club.Id,
                    Name = club.Properties["clubTitle"].Value.ToString(),
                    Description = club.Properties["clubDescription"].Value.ToString(),
                    Teams = teams
                };

                try
                {
                    string guid = club.Properties["clubLogo"].Value.ToString();
                    var udi = Udi.Parse(guid);
                    var media = Umbraco.GetIdForUdi(udi);
                    var content = Umbraco.Media(media);
                    var imgPath = content.Url;
                    c.ClubLogo = imgPath;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }

                res.Add(c);
            }

            return res;
        }
    }
}