using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Web.WebApi;

namespace ApiPotG.Controllers
{
    public class SponsorController : UmbracoApiController
    {
        [HttpGet]
        public Sponsor GetSponsor(int sponsorId)
        {
            var cs = Services.ContentService;
            var sponsor = cs.GetById(sponsorId);

            Sponsor res = new Sponsor
            {
                id = sponsor.Id,
                sponsorName = sponsor.Properties["sponsorName"].Value.ToString()
            };

            try
            {
                string guid = sponsor.Properties["logo"].Value.ToString();
                var udi = Udi.Parse(guid);
                var media = Umbraco.GetIdForUdi(udi);
                var content = Umbraco.Media(media);
                var imgPath = content.Url;
                res.logo = imgPath;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return res;
        }
    }
}
