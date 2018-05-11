using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace ApiPotG.Controllers
{
  
    public class VoteController : UmbracoApiController
    {
        private readonly IContentService cs;

        public VoteController()
        {
            cs = Services.ContentService;
        }

        [HttpPost]
        public string CommitVote([FromBody] Vote data)
        {
            
            try
            {
                //validate vote
                //bool validates = ValidateVote(ticketID, projectID);
                //if (validates)
                //{
                string message = "api entry ticket for match: " + data.matchId.ToString() + ", by: " + data.IMEI.ToString();
                //create vote object
                var vote = cs.CreateContent(message, data.voteBatchId, "vote");

                //set vote properties
                vote.Properties["IMEI"].Value = data.IMEI;
                vote.Properties["matchId"].Value = data.matchId;
                vote.Properties["playerId"].Value = data.playerId;

                cs.Publish(vote);
                cs.Save(vote);
                return "success";
                //}
                //if (!validates)
                //{
                //    return "The vote is invalid";
                //}

            }
            catch (Exception)
            {

                throw;
            }
            return "failed to vote";
        }

        [HttpGet]
        public List<Vote> GetVotes(int voteBatchId)
        {
            List<Vote> res = new List<Vote>();
            var votes = cs.GetDescendants(voteBatchId);

            foreach (var vote in votes)
            {
                var v = new Vote
                {
                    id = vote.Id,
                    IMEI = int.Parse(vote.Properties["IMEI"].Value.ToString()),
                    matchId = int.Parse(vote.Properties["matchId"].Value.ToString()),
                    playerId = int.Parse(vote.Properties["playerId"].Value.ToString()),
                    dateTime = vote.CreateDate
                };
                res.Add(v);
            }

            return res;
        }
    }
}
