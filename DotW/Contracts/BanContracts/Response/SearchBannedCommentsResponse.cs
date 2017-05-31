namespace Contracts.BanContracts.Response
{
    using Entities.CommentaryEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchBannedCommentsResponse
    {
        public List<Commentary> Comments;
    }
}
