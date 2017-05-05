namespace Contracts.VoteContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetVoteByUserAndPostIdRequest
    {
        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}
