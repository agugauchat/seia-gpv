namespace Services.CommentaryServices
{
    using Contracts.CommentaryContracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.CommentaryContracts.Request;
    using Contracts.CommentaryContracts.Response;
    using Data;
    using Entities.CommentaryEntities;

    public class CommentaryService : ICommentaryService
    {
        public CreateCommentaryResponse CreateCommentary(CreateCommentaryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var commentary = new Comments
                {
                    Commentary = request.CommentaryText,
                    EffectDate = DateTime.Now,
                    IdPost = request.IdPost,
                    IdUser = request.IdUser
                };

                db.Comments.Add(commentary);

                db.SaveChanges();

                return new CreateCommentaryResponse { CommentaryId = commentary.Id };
            }
        }


        public SearchCommentsByIdPostResponse SearchCommentsByIdPost(SearchCommentsByIdPostRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = db.Comments.Where(x => !x.NullDate.HasValue && x.IdPost == request.IdPost)
                        .Select(x => new Commentary
                        {
                            Id = x.Id,
                            CommentaryText = x.Commentary,
                            IdPost = x.IdPost,
                            IdUser = x.IdUser,
                            WriterUserName = x.Users.Name,
                            EffectDate = x.EffectDate
                        }).ToList();

                return new SearchCommentsByIdPostResponse { Comments = result };
            }
        }

        public GetCommentaryByIdResponse GetCommentaryById(GetCommentaryByIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var response = new GetCommentaryByIdResponse();

                response.Commentary = db.Comments.Select(x =>
                    new Commentary
                    {
                        Id = x.Id,
                        CommentaryText = x.Commentary,
                        IdPost = x.IdPost,
                        IdUser = x.IdUser,
                        WriterUserName = x.Users.Name,
                        EffectDate = x.EffectDate,
                    }).FirstOrDefault(x => x.Id == request.Id);

                return response;
            }
        }

        public UpdateCommentaryResponse UpdateCommentary(UpdateCommentaryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var commentary = db.Comments.FirstOrDefault(x => x.Id == request.Id);

                if (commentary != null)
                {
                    commentary.Commentary = request.CommentaryText;

                    db.SaveChanges();
                }

                return new UpdateCommentaryResponse();
            }
        }

        public DeleteCommentaryResponse DeleteCommentary(DeleteCommentaryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var commentary = db.Comments.FirstOrDefault(x => x.Id == request.Id);

                if (commentary != null)
                {
                    commentary.NullDate = DateTime.Now;

                    db.SaveChanges();
                }

                return new DeleteCommentaryResponse();
            }
        }
    }
}