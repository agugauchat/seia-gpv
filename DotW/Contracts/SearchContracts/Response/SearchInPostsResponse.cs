namespace Contracts.SearchContracts.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Entities.SearchEntities;

    public class SearchInPostsResponse
    {
        public List<PostsSearchResult> PostsSearchResult { get; set; }
    }
}
