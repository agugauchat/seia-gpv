namespace Contracts.ComplaintContracts.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateCommentaryComplaintResponse
    {
        public int ComplaintId { get; set; }

        public int CommentaryId { get; set; }

        public int CommentaryComplaintsCount { get; set; }
    }
}
