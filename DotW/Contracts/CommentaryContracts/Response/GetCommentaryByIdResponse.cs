namespace Contracts.CommentaryContracts.Response
{
    using Entities.CommentaryEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetCommentaryByIdResponse
    {
        public Commentary Commentary { get; set; }
    }
}
