using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace ApiPotG.Controllers
{
    public class ClubController : UmbracoApiController
    {
        private readonly IContentService cs;
        private readonly TeamController tc;

        public ClubController()
        {
            cs = Services.ContentService;
            tc = new TeamController();
        }

        // eg. /umbraco/Api/Clubs/GetClubs?rootId=1071
        [HttpGet]
        public List<Club> GetClubs(int rootId)
        {
            List<Club> res = new List<Club>();
            var clubs = cs.GetChildren(rootId);
            

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

        [HttpGet]
        public Club GetClub(int clubId)
        {
            var club = cs.GetById(clubId);

            List<Team> teams = tc.GetTeams(club.Id);

            Club res = new Club
            {
                Id = club.Id,
                Name = club.Properties["clubTitle"].Value.ToString(),
                Description = club.Properties["clubDescription"].Value.ToString(),
                Teams = teams
            };

            if (club.ContentType.Alias == "club")
            {
                try
                {
                    string guid = club.Properties["clubLogo"].Value.ToString();
                    var udi = Udi.Parse(guid);
                    var media = Umbraco.GetIdForUdi(udi);
                    var content = Umbraco.Media(media);
                    var imgPath = content.Url;
                    res.ClubLogo = imgPath;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }

                return res;
            }

            return new Club();

        }
    }
}