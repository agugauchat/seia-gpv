namespace Contracts.CommentaryContracts
{
    using Request;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICommentaryService
    {
        CreateCommentaryResponse CreateCommentary(CreateCommentaryRequest request);

        SearchCommentsByIdPostResponse SearchCommentsByIdPost(SearchCommentsByIdPostRequest request);

        GetCommentaryByIdResponse GetCommentaryById(GetCommentaryByIdRequest request);

        UpdateCommentaryResponse UpdateCommentary(UpdateCommentaryRequest request);

        DeleteCommentaryResponse DeleteCommentary(DeleteCommentaryRequest request);
    }
}
