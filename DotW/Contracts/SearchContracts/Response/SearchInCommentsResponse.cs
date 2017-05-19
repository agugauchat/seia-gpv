namespace Contracts.SearchContracts.Response
{
    using Entities.SearchEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchInCommentsResponse
    {
        public List<CommentsSearchResult> CommentsSearchResult { get; set; }
    }
}
