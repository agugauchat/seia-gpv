namespace Contracts.VoteContracts.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetVotesCountByPostIdResponse
    {
        public int GoodVotes { get; set; }

        public int BadVotes { get; set; }
    }
}
