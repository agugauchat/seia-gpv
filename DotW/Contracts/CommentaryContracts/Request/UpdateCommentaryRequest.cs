namespace Contracts.CommentaryContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UpdateCommentaryRequest
    {
        public int Id { get; set; }

        public string CommentaryText { get; set; }
    }
}
