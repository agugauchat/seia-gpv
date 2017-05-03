namespace Contracts.ComplaintContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateCommentaryComplaintRequest
    {
        public int CommentaryId { get; set; }

        public int UserId { get; set; }

        public string Commentary { get; set; }
    }
}
