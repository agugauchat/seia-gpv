namespace Contracts.BanContracts
{
    using Contracts.BanContracts.Request;
    using Contracts.BanContracts.Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBanService
    {
        SearchBannedPostsResponse SearchBannedPosts(SearchBannedPostsRequest request);

        SearchBannedCommentsResponse SearchBannedComments(SearchBannedCommentsRequest request);

        SearchBannedUsersResponse SearchBannedUsers(SearchBannedUsersRequest request);

        EnablePostResponse EnablePost(EnablePostRequest request);

        EnableCommentaryResponse EnableCommentary(EnableCommentaryRequest request);

        EnableUserResponse EnableUser(EnableUserRequest request);
    }
}
