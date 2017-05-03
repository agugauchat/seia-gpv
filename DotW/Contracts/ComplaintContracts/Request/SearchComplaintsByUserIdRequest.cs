namespace Contracts.ComplaintContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchComplaintsByUserIdRequest
    {
        public int? UserId { get; set; }

        public string AspNetUserId { get; set; }
    }
}
