namespace Services.SearchServices
{
    using Contracts.SearchContracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.SearchContracts.Request;
    using Contracts.SearchContracts.Response;
    using Data;
    using System.Data.SqlClient;
    using Entities.SearchEntities;

    public class SearchService : ISearchService
    {
        public SearchInPostsResponse SearchInPosts(SearchInPostsRequest request)
        {
            using (var db = new DotWEntities())
            {
                var response = new SearchInPostsResponse { PostsSearchResult = new List<PostsSearchResult>() };

                if (!string.IsNullOrEmpty(request.Text))
                {
                    response.PostsSearchResult.AddRange(db.Database.SqlQuery<PostsSearchResult>("sp_PostsSearch @text", new SqlParameter("text", request.Text)).ToList());
                }

                return response;
            }
        }
    }
}
