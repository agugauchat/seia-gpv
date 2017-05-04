namespace Contracts.ComplaintContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchComplaintsByCommentaryIdRequest
    {
        public int CommentaryId { get; set; }
    }
}
