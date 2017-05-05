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
                        Good = request.Good,
                        Bad = request.Bad
                    };

                    db.Votes.Add(vote);
                    db.SaveChanges();
                }
                else
                {
                    vote = previousVote;

                    vote.Good = request.Good;
                    vote.Bad = request.Bad;

                    db.SaveChanges();
                }

                var response = new SaveVoteResponse
                {
                    VoteId = vote.Id,
                    PostId = vote.IdPost,
                    PostGoodVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.Good),
                    PostBadVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.Bad)
                };

                return response;
            }
        }

        public GetVotesCountByPostIdResponse GetVotesCountByPostId(GetVotesCountByPostIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new GetVotesCountByPostIdResponse();

                result.GoodVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.Good);
                result.BadVotes = db.Votes.Where(x => x.IdPost == request.PostId).Count(x => x.Bad);

                return result;
            }
        }

        public GetVoteByUserAndPostIdResponse GetVoteByUserAndPostId(GetVoteByUserAndPostIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new GetVoteByUserAndPostIdResponse();

                var vote = db.Votes.FirstOrDefault(x => x.IdPost == request.PostId && x.IdUser == request.UserId);

                result.Good = vote != null ? vote.Good : false;
                result.Bad = vote != null ? vote.Bad : false;

                return result;
            }
        }
    }
}
