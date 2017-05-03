namespace Contracts.ComplaintContracts.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreatePostComplaintResponse
    {
        public int ComplaintId { get; set; }

        public int PostId { get; set; }

        public int PostComplaintsCount { get; set; }
    }
}
