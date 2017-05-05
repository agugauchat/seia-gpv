namespace Services.VoteServices
{
    using Contracts.VoteContracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.VoteContracts.Request;
    using Contracts.VoteContracts.Response;
    using Data;

    public class VoteService : IVoteService
    {
        public SaveVoteResponse SaveVote(SaveVoteRequest request)
        {
            using (var db = new DotWEntities())
            {
                var previousVote = db.Votes.FirstOrDefault(x => x.IdUser == request.UserId
                                                                && x.IdPost == request.PostId);
                Votes vote;

                if (previousVote == null)
                {
                    vote = new Votes
                    {
                        IdPost = request.PostId,
                        IdUser = request.UserId,
                        God = request.Good,
                        Bad = request.Bad
                    };

                    db.Votes.Add(vote);
                    db.SaveChanges();
                }
                else
                {
                    vote = previousVote;

                    vote.God = request.Good;
                    vote.Bad = request.Bad;

                    db.SaveChanges();
                }

                var response = new SaveVoteResponse
                {
                    VoteId = vote.Id,
                    PostId = vote.IdPost,
                    PostGoodVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.God.HasValue && x.God.Value),
                    PostBadVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.Bad.HasValue && x.Bad.Value)
                };

                return response;
            }
        }

        public GetVotesCountByPostIdResponse GetVotesCountByPostId(GetVotesCountByPostIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new GetVotesCountByPostIdResponse();

                result.GoodVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.God.HasValue && x.God.Value);
                result.BadVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.Bad.HasValue && x.Bad.Value);

                return result;
            }
        }

        public GetVoteByUserAndPostIdResponse GetVoteByUserAndPostId(GetVoteByUserAndPostIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new GetVoteByUserAndPostIdResponse();

                var vote = db.Votes.FirstOrDefault(x => x.IdPost == request.PostId && x.IdUser == request.UserId);

                result.Good = vote != null && vote.God.HasValue ? vote.God.Value : false;
                result.Bad = vote != null && vote.Bad.HasValue ? vote.Bad.Value : false;

                return result;
            }
        }
    }
}
