namespace Contracts.SearchContracts
{
    using Contracts.SearchContracts.Request;
    using Contracts.SearchContracts.Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISearchService
    {
        SearchInPostsResponse SearchInPosts(SearchInPostsRequest request);
    }
}
