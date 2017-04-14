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

        SearchPostsResponse SearchPosts(SearchPostsRequest request);

        GetPostByIdResponse GetPostById(GetPostByIdRequest request);

        UpdatePostResponse UpdatePost(UpdatePostRequest request);

        DeletePostResponse DeletePost(DeletePostRequest request);
    }
}
