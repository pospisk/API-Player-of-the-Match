using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web.WebApi;

namespace ApiPotG.Controllers
{
    public class PlayerController : UmbracoApiController
    {
        [HttpGet]
        public List<Player> GetPlayers(int parentId)
        {
            var cs = Services.ContentService;
            List<Player> res = new List<Player>();
            var players = cs.GetDescendants(parentId);

            foreach( var player in players)
            {
                if (player.ContentType.Alias == "player")
                {
                    var p = new Player
                    {
                        Id = player.Id,
                        FirstName = player.Properties["firstName"].Value.ToString(),
                        LastName = player.Properties["lastName"].Value.ToString(),
                        JerseyNumber = int.Parse(player.Properties["jerseyNumber"].Value.ToString())
                    };

                    try
                    {
                        string guid = player.Properties["playerImage"].Value.ToString();
                        var udi = Udi.Parse(guid);
                        var media = Umbraco.GetIdForUdi(udi);
                        var content = Umbraco.Media(media);
                        var imgPath = content.Url;
                        p.PlayerImage = imgPath;
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    res.Add(p);
                }
            }

            return res;
        }

    }
}
