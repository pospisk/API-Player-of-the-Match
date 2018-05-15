using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace ApiPotG.Controllers
{
    public class MatchController : UmbracoApiController
    {
        private readonly IContentService cs;
        private readonly string mediaBaseUri = "http://api.potg-dev.org";

        public MatchController()
        {
            cs = Services.ContentService;
        }

        /// <summary>
        /// get matches from a parrent id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns>list of matces with ids of sponsors and opponents</returns>
        [HttpGet]
        public List<Match> GetMatches(int parentId)
        {
            
            List<Match> res = new List<Match>();
            var matches = cs.GetDescendants(parentId);

            foreach (var match in matches)
            {
                if (match.ContentType.Alias == "match")
                {
                    var m = new Match
                    {
                        Id = match.Id,
                        Name = match.Name,
                        MatchDate = DateTime.Parse(match.Properties["matchStart"].Value.ToString()),
                        TeamId =  cs.GetParent(cs.GetParent(match.Id).Id).Id
                    };

                    // TeamId has to be defined
                    m.ClubId = cs.GetParent(cs.GetParent(m.TeamId).Id).Id;

                    try
                    {
                        string guid = match.Properties["matchOpponent"].Value.ToString();
                        var uid = Udi.Parse(guid);
                        var media = Umbraco.GetIdForUdi(uid);
                        m.OpponentId = media;
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
                        m.SponsorId = media;
                    }
                    catch(NullReferenceException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    res.Add(m);
                }
            }

            return res;
        }

        [HttpGet]
        public Match GetMatch(int matchId)
        {
            var match = cs.GetById(matchId);

            Match res = new Match
            {
                Id = match.Id,
                Name = match.Name,
                MatchDate = DateTime.Parse(match.Properties["matchStart"].Value.ToString()),
                TeamId = cs.GetParent(cs.GetParent(match.Id).Id).Id
            };

            // TeamId has to be defined
            res.ClubId = cs.GetParent(cs.GetParent(res.TeamId).Id).Id;

            if (match.ContentType.Alias == "match")
            {
                try
                {
                    string guid = match.Properties["matchOpponent"].Value.ToString();
                    var uid = Udi.Parse(guid);
                    var media = Umbraco.GetIdForUdi(uid);
                    res.OpponentId = media;
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
                    res.SponsorId = media;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    if (res.SponsorId != 0)
                    {
                        var sponsor = cs.GetById(res.SponsorId);
                        res.SponsorName = sponsor.Name;

                        string guid = sponsor.Properties["logo"].Value.ToString();
                        var udi = Udi.Parse(guid);
                        var media = Umbraco.GetIdForUdi(udi);
                        var content = Umbraco.Media(media);
                        var imgPath = content.Url;
                        res.SponsorImagePath = mediaBaseUri + imgPath;
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    var team = cs.GetById(res.TeamId);
                    res.TeamName = team.Name;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    var club = cs.GetById(res.ClubId);
                    string guid = club.Properties["clubLogo"].Value.ToString();
                    var udi = Udi.Parse(guid);
                    var media = Umbraco.GetIdForUdi(udi);
                    var content = Umbraco.Media(media);
                    var imgPath = content.Url;
                    res.ClubImagePath = mediaBaseUri + imgPath;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    var opponent = cs.GetById(res.OpponentId);
                    res.OpponentName = opponent.Name;

                    var opponentClub = cs.GetParent(cs.GetParent(opponent.Id).Id);

                    string guid = opponentClub.Properties["clubLogo"].Value.ToString();
                    var udi = Udi.Parse(guid);
                    var media = Umbraco.GetIdForUdi(udi);
                    var content = Umbraco.Media(media);
                    var imgPath = content.Url;
                    res.OpponentClubImagePath = mediaBaseUri + imgPath;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }


                return res;

            }

            return new Match();
        }
    }
}