namespace Contracts.VoteContracts
{
    using Contracts.VoteContracts.Request;
    using Contracts.VoteContracts.Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IVoteService
    {
        SaveVoteResponse SaveVote(SaveVoteRequest request);

        GetVotesCountByPostIdResponse GetVotesCountByPostId(GetVotesCountByPostIdRequest request);

        GetVoteByUserAndPostIdResponse GetVoteByUserAndPostId(GetVoteByUserAndPostIdRequest request);
    }
}
