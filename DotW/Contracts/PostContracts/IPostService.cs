namespace Contracts.PostContracts
{
    using Request;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPostService
    {
        CreatePostResponse CreatePost(CreatePostRequest request);

        SearchPostsByUserIdResponse SearchPostsByUserId(SearchPostsByUserIdRequest request);

        SearchPostsByCategoryIdResponse SearchPostsByCategoryId(SearchPostsByCategoryIdRequest request);

        SearchPostsByTagResponse SearchPostsByTag(SearchPostsByTagRequest request);

        SearchPostsResponse SearchPosts(SearchPostsRequest request);

        SearchPostsForHomeRankingsResponse SearchPostsForHomeRankings(SearchPostsForHomeRankingsRequest request);

        GetPostByIdResponse GetPostById(GetPostByIdRequest request);

        UpdatePostResponse UpdatePost(UpdatePostRequest request);

        DeletePostResponse DeletePost(DeletePostRequest request);
    }
}
