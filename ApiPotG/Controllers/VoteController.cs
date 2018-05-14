using ApiPotG.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Net;
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

        /// <summary>
        /// method for commiting votes
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A http status code</returns>
        [HttpPost]
        public IHttpActionResult CommitVote([FromBody] Vote data)
        {
            try
            {
                //validate vote
                bool validates = ValidateVote(data.IMEI, data.MatchId, data.PlayerId);
                if (validates)
                {
                    string message = "api entry ticket for match: " + data.MatchId.ToString() + ", by: " + data.IMEI.ToString();
                //create vote object
                var vote = cs.CreateContent(message, data.VoteBatchId, "vote");

                //set vote properties
                vote.Properties["IMEI"].Value = data.IMEI;
                vote.Properties["matchId"].Value = data.MatchId;
                vote.Properties["playerId"].Value = data.PlayerId;

                cs.Publish(vote);
                cs.Save(vote);
                return StatusCode(HttpStatusCode.Created);

                }
                if (!validates)
                {
                    return StatusCode(HttpStatusCode.Conflict);
                }

            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }


        /// <summary>
        /// Private serverside validation of a vote
        /// </summary>
        /// <param name="IMEI"></param>
        /// <param name="matchId"></param>
        /// <param name="playerID"></param>
        /// <param name="voteBatchId"></param>
        /// <param name="rootId"></param>
        /// <returns>A boolean value</returns>
        private bool ValidateVote(int IMEI, int matchId,int playerId, int voteBatchId = 1116, int rootId = 1071)
        {
            // data for insert validation
            MatchController mc = new MatchController();
            List<Match> matches = mc.GetMatches(rootId);
            PlayerController pc = new PlayerController();
            List<Player> players = pc.GetPlayers(rootId);

            // data for cross checking
            List<Vote> votes = GetVotes(voteBatchId);
            
            try
            {
                bool isInsertable = (
                    matches.Exists(x => x.Id == matchId) 
                    && players.Exists(x => x.Id == playerId) )
                    ? true : false;

                if (isInsertable)
                {
                    List<Vote> matchingVotes = votes.FindAll(FindMatchingVote(matchId, playerId));
                    
                    bool usedIMEI = false;
                    foreach (var vote in matchingVotes)
                    {
                        // set USED = TRUE if IMEI is in use on the vote
                        usedIMEI = (vote.IMEI == IMEI)? true : false;

                        // if in use, end foreach
                        if (usedIMEI)
                        {
                            break;
                        }
                    }

                    // final check for return
                    if (!usedIMEI)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            
            // if something fails
            return false;
        }

        private Predicate<Vote> FindMatchingVote(int matchId, int playerId )
        {
            return delegate (Vote v)
            {
                return v.MatchId == matchId && v.PlayerId == playerId;
            };
            
        }

        /// <summary>
        /// Get the votes from a batch
        /// </summary>
        /// <param name="voteBatchId"></param>
        /// <returns>List of votes from a batch</returns>
        [HttpGet]
        public List<Vote> GetVotes(int voteBatchId)
        {
            List<Vote> res = new List<Vote>();
            var votes = cs.GetDescendants(voteBatchId);

            foreach (var vote in votes)
            {
                var v = new Vote
                {
                    Id = vote.Id,
                    IMEI = int.Parse(vote.Properties["IMEI"].Value.ToString()),
                    MatchId = int.Parse(vote.Properties["matchId"].Value.ToString()),
                    PlayerId = int.Parse(vote.Properties["playerId"].Value.ToString()),
                    DateTime = vote.CreateDate
                };
                res.Add(v);
            }

            return res;
        }
    }
}
