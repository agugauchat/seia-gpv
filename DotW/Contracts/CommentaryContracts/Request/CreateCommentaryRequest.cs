namespace Contracts.CommentaryContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateCommentaryRequest
    {
        public int IdPost { get; set; }

        public int IdUser { get; set; }

        public string CommentaryText { get; set; }

        public DateTime EffectDate { get; set; }

        public int? IdUpperComment { get; set; }
    }
}
