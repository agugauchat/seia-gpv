namespace Contracts.VoteContracts.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SaveVoteResponse
    {
        public int VoteId { get; set; }

        public int PostId { get; set; }

        public int PostGoodVotes { get; set; }

        public int PostBadVotes { get; set; }
    }
}
