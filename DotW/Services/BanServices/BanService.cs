namespace Services.BanServices
{
    using System;
    using Contracts.BanContracts;
    using Contracts.BanContracts.Request;
    using Contracts.BanContracts.Response;
    using Data;
    using System.Linq;
    using Entities.PostEntities;
    using System.Collections.Generic;
    using Entities.CommentaryEntities;
    using Entities.UserEntities;
    using Contracts.UserContracts;

    public class BanService : IBanService
    {
        public SearchBannedPostsResponse SearchBannedPosts(SearchBannedPostsRequest request)
        {
            using (var db = new DotWEntities())
            {
                var response = new SearchBannedPostsResponse { Posts = new List<Post>() };

                response.Posts.AddRange(db.Posts.Where(x => x.NullDate.HasValue && x.DeletedByComplaintsOrVotes)
                        .Select(x => new Post
                        {
                            Id = x.Id,
                            Title = x.Title,
                            EffectDate = x.EffectDate,
                            NullDate = x.NullDate,
                            IdWriter = x.IdWriter,
                            WriterUserName = x.Users.Name,
                            IdCategory = x.IdCategory,
                            CategoryTitle = x.Categories.Title,
                        }).ToList());

                return response;
            }
        }

        public SearchBannedCommentsResponse SearchBannedComments(SearchBannedCommentsRequest request)
        {
            using (var db = new DotWEntities())
            {
                var response = new SearchBannedCommentsResponse { Comments = new List<Commentary>() };

                response.Comments.AddRange(db.Comments.Where(x => x.NullDate.HasValue && x.DeletedByComplaints)
                        .Select(x => new Commentary
                        {
                            Id = x.Id,
                            CommentaryText = x.Commentary,
                            IdPost = x.IdPost,
                            IdUser = x.IdUser,
                            WriterUserName = x.Users.Name,
                            EffectDate = x.EffectDate,
                            NullDate = x.NullDate
                        }).ToList());

                return response;
            }
        }

        public SearchBannedUsersResponse SearchBannedUsers(SearchBannedUsersRequest request)
        {
            using (var db = new DotWEntities())
            {
                var response = new SearchBannedUsersResponse { Users = new List<User>() };

                response.Users.AddRange(db.Users
                    .Where(x => x.NullDate.HasValue && x.ActivationDate.HasValue
                                && (x.UserStates.State == UserAccountStates.Suspended.ToString()
                                    || x.UserStates.State == UserAccountStates.Blocked.ToString()))
                    .Select(x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        AspNetUserId = x.AspNetUserId,
                        EffectDate = x.EffectDate,
                        NullDate = x.NullDate,
                        IdState = x.IdState,
                        ActivationDate = x.ActivationDate,
                        Email = x.AspNetUsers.Email,
                        Phone = x.Phone,
                        Description = x.Description,
                        FullName = x.FullName,
                        ShowData = x.ShowData,
                    }).ToList());

                return response;
            }
        }

        public EnablePostResponse EnablePost(EnablePostRequest request)
        {
            using (var db = new DotWEntities())
            {
                var post = db.Posts.FirstOrDefault(x => x.Id == request.PostId);

                if (post != null)
                {
                    post.NullDate = null;
                    post.DeletedByComplaintsOrVotes = false;

                    var user = db.Users.FirstOrDefault(x => x.Id == post.IdWriter);

                    user.BlockedPosts -= 1;

                    db.SaveChanges();
                }

                return new EnablePostResponse();
            }
        }

        public EnableCommentaryResponse EnableCommentary(EnableCommentaryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var comment = db.Comments.FirstOrDefault(x => x.Id == request.CommentaryId);

                if (comment != null)
                {
                    comment.NullDate = null;
                    comment.DeletedByComplaints = false;

                    var user = db.Users.FirstOrDefault(x => x.Id == comment.IdUser);

                    user.BlockedComments -= 1;

                    db.SaveChanges();
                }

                return new EnableCommentaryResponse();
            }
        }

        public EnableUserResponse EnableUser(EnableUserRequest request)
        {
            using (var db = new DotWEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == request.UserId);

                if (user != null)
                {
                    user.NullDate = null;
                    user.IdState = (int)UserAccountStates.Active;
                    user.ActivationDate = null;

                    db.SaveChanges();
                }

                return new EnableUserResponse();
            }
        }
    }
}
