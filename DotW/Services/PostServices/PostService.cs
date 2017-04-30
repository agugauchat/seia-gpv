namespace Services.PostServices
{
    using Contracts.PostContracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.PostContracts.Request;
    using Contracts.PostContracts.Response;
    using Data;
    using Entities.PostEntities;

    public class PostService : IPostService
    {
        public CreatePostResponse CreatePost(CreatePostRequest request)
        {
            using (var db = new DotWEntities())
            {
                var post = new Posts
                {
                    IdWriter = request.IdWriter,
                    Title = request.Title,
                    Summary = request.Summary,
                    Body = request.Body,
                    EffectDate = DateTime.Now,
                    IdCategory = request.CategoryId,
                    IsDraft = request.IsDraft,
                    PrincipalImageName = request.PrincipalImageName
                };

                db.Posts.Add(post);

                db.SaveChanges();

                return new CreatePostResponse { PostId = post.Id };
            }
        }

        public SearchPostsByUserIdResponse SearchPostsByUserId(SearchPostsByUserIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new List<Post>();

                if (request.UserId.HasValue)
                {
                    result = db.Posts.Where(x => x.Users.Id == request.UserId && !x.NullDate.HasValue)
                        .Select(x => new Post
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Summary = x.Summary,
                            Body = x.Body,
                            EffectDate = x.EffectDate,
                            IdWriter = x.IdWriter,
                            WriterUserName = x.Users.Name,
                            IdCategory = x.IdCategory,
                            CategoryTitle = x.Categories.Title,
                            IsDraft = x.IsDraft,
                            PrincipalImageName = x.PrincipalImageName
                        }).ToList();
                }
                else if(!string.IsNullOrEmpty(request.AspNetUserId))
                {
                    result = db.Posts.Where(x => x.Users.AspNetUserId == request.AspNetUserId && !x.NullDate.HasValue)
                        .Select(x => new Post
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Summary = x.Summary,
                            Body = x.Body,
                            EffectDate = x.EffectDate,
                            IdWriter = x.IdWriter,
                            WriterUserName = x.Users.Name,
                            IdCategory = x.IdCategory,
                            CategoryTitle = x.Categories.Title,
                            IsDraft = x.IsDraft,
                            PrincipalImageName = x.PrincipalImageName
                        }).ToList();
                }

                return new SearchPostsByUserIdResponse { Posts = result };
            }
        }

        public SearchPostsByCategoryIdResponse SearchPostsByCategoryId(SearchPostsByCategoryIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new List<Post>();
                result = db.Posts.Where(x => x.IdCategory == request.IdCategory
                                             && !x.NullDate.HasValue)
                    .Select(x => new Post
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Summary = x.Summary,
                        Body = x.Body,
                        EffectDate = x.EffectDate,
                        IdWriter = x.IdWriter,
                        WriterUserName = x.Users.Name,
                        IdCategory = x.IdCategory,
                        CategoryTitle = x.Categories.Title,
                        IsDraft = x.IsDraft,
                        PrincipalImageName = x.PrincipalImageName
                    }).ToList();

                return new SearchPostsByCategoryIdResponse { Posts = result };
            }
        }

        public SearchPostsResponse SearchPosts(SearchPostsRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = db.Posts.Where(x => !x.NullDate.HasValue)
                        .Select(x => new Post
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Summary = x.Summary,
                            Body = x.Body,
                            EffectDate = x.EffectDate,
                            IdWriter = x.IdWriter,
                            WriterUserName = x.Users.Name,
                            IdCategory = x.IdCategory,
                            CategoryTitle = x.Categories.Title,
                            IsDraft = x.IsDraft,
                            PrincipalImageName = x.PrincipalImageName
                        }).ToList();

                return new SearchPostsResponse { Posts = result };
            }
        }

        public GetPostByIdResponse GetPostById(GetPostByIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var response = new GetPostByIdResponse();

                response.Post = db.Posts.Select(x =>
                    new Post
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Summary = x.Summary,
                        Body = x.Body,
                        EffectDate = x.EffectDate,
                        IdWriter = x.IdWriter,
                        WriterUserName = x.Users.Name,
                        IdCategory = x.IdCategory,
                        CategoryTitle = x.Categories.Title,
                        IsDraft = x.IsDraft,
                        PrincipalImageName = x.PrincipalImageName
                    }).FirstOrDefault(x => x.Id == request.Id);

                return response;
            }
        }

        public UpdatePostResponse UpdatePost(UpdatePostRequest request)
        {
            using (var db = new DotWEntities())
            {
                var post = db.Posts.FirstOrDefault(x => x.Id == request.Id);

                if (post != null)
                {
                    post.Title = request.Title;
                    post.Summary = request.Summary;
                    post.Body = request.Body;
                    post.IdCategory = request.IdCategory;
                    post.IsDraft = request.IsDraft;
                    post.PrincipalImageName = request.PrincipalImageName;

                    db.SaveChanges();
                }

                return new UpdatePostResponse();
            }
        }

        public DeletePostResponse DeletePost(DeletePostRequest request)
        {
            using (var db = new DotWEntities())
            {
                var post = db.Posts.FirstOrDefault(x => x.Id == request.Id);

                if (post != null)
                {
                    post.NullDate = DateTime.Now;

                    db.SaveChanges();
                }

                return new DeletePostResponse();
            }
        }
    }
}
