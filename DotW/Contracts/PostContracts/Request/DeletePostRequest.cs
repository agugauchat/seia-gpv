namespace Contracts.PostContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DeletePostRequest
    {
        public int Id { get; set; }

        public bool IsComplaintOrVoteDifference { get; set; }
    }
}
