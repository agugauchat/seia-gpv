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
                    Body = request.Body,
                    EffectDate = DateTime.Now
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
                            Body = x.Body,
                            EffectDate = x.EffectDate,
                            IdWriter = x.IdWriter
                        }).ToList();
                }
                else if(!string.IsNullOrEmpty(request.AspNetUserId))
                {
                    result = db.Posts.Where(x => x.Users.AspNetUserId == request.AspNetUserId && !x.NullDate.HasValue)
                        .Select(x => new Post
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Body = x.Body,
                            EffectDate = x.EffectDate,
                            IdWriter = x.IdWriter
                        }).ToList();
                }

                return new SearchPostsByUserIdResponse { Posts = result };
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
                            Body = x.Body,
                            EffectDate = x.EffectDate,
                            IdWriter = x.IdWriter
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
                        Body = x.Body,
                        EffectDate = x.EffectDate,
                        IdWriter = x.IdWriter
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
                    post.Body = request.Body;

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
