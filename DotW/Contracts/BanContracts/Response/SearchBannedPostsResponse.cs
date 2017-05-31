namespace Contracts.BanContracts.Response
{
    using Entities.PostEntities;
    using System.Collections.Generic;

    public class SearchBannedPostsResponse
    {
        public List<Post> Posts { get; set; }
    }
}
