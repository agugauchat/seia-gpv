namespace Web.DotW.API.Controllers
{
    using Contracts.CommentaryContracts.Request;
    using Contracts.ComplaintContracts.Request;
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Contracts.VoteContracts.Request;
    using Entities.General;
    using Entities.PostEntities;
    using Microsoft.AspNet.Identity;
    using Services.CommentaryServices;
    using Services.ComplaintServices;
    using Services.PostServices;
    using Services.UserServices;
    using Services.VoteServices;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Web.DotW.API.Helpers;
    using Web.DotW.API.Models;

    public class PostsController : BaseApiController
    {
        // GET: api/Posts
        [AllowAnonymous]
        [Route("api/Posts/", Name = "GetPosts")]
        public IHttpActionResult GetPosts()
        {
            try
            {
                var postService = new PostService();
                var votesService = new VoteService();
                var commentaryService = new CommentaryService();
                var posts = postService.SearchPosts(new SearchPostsRequest()).Posts;

                var result = new List<GetPostModel>();

                foreach (var post in posts)
                {
                    var postResult = GenerateGetPostModel(post);

                    result.Add(postResult);
                }

                return Ok(result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET: api/Posts/5
        [AllowAnonymous]
        [ResponseType(typeof(Post))]
        [Route("api/Posts/{id}", Name = "GetPostById")]
        public IHttpActionResult GetPost(int id)
        {
            try
            {
                var postService = new PostService();
                var votesService = new VoteService();
                var commentaryService = new CommentaryService();
                var post = postService.GetPostById(new GetPostByIdRequest() { Id = id }).Post;

                if (post == null)
                {
                    return NotFound();
                }

                var result = GenerateGetPostModel(post);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [ResponseType(typeof(GetCommentsModel))]
        [Route("api/Posts/{id}/Comments", Name = "GetCommentsByPostId")]
        public IHttpActionResult GetCommentary(int id)
        {
            try
            {
                var commentaryService = new CommentaryService();
                var comments = commentaryService.SearchCommentsByIdPost(new SearchCommentsByIdPostRequest() { IdPost = id }).Comments;

                if (!comments.Any())
                {
                    return NotFound();
                }

                var result = GenerateGetCommentsModel(id);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT: api/Posts/5
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPost(EditPostModel model)
        {
            try
            {
                var userService = new UserService();
                var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

                if (userAccountSuspended)
                {
                    return BadRequest("User account suspended");
                }

                var postService = new PostService();
                var complaintService = new ComplaintService();
                string finalFileName = string.Empty;

                var postComplaints = complaintService.SearchComplaintsByPostId(new SearchComplaintsByPostIdRequest { PostId = model.Id }).Complaints;

                if (postComplaints.Any())
                {
                    return BadRequest("Post with complaints");
                }

                if (ModelState.IsValid)
                {
                    var post = postService.GetPostById(new GetPostByIdRequest() { Id = model.Id }).Post;

                    if (post == null)
                    {
                        return NotFound();
                    }

                    var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                    if (post.IdWriter != user.Id)
                    {
                        return BadRequest();
                    }

                    if (!string.IsNullOrEmpty(model.base64File) && !string.IsNullOrWhiteSpace(model.base64File) && !model.DeleteImage)
                    {
                        string[] tokens = model.base64File.Split(',');
                        string extension = tokens[0].Split('/')[1].Split(';')[0];

                        if (new AllowedExtensions().ImageExtensions.Contains(extension.ToLower()))
                        {
                            var folder = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PathPostPrincipalImages"]);
                            finalFileName = string.Format("{0}.{1}", model.Id.ToString(), extension);

                            string path = Path.Combine(folder, finalFileName);
                            var stream = ImageHelpers.Base64ToStream(tokens[1], path);
                            var image = new Bitmap(stream);

                            // Se guarda la imagen en el servidor.
                            image.Save(path);

                            finalFileName = "api/" + finalFileName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "La extensión de la imagen no es válida, vuelva a cargarla.");
                        }
                    }

                    var result = postService.UpdatePost(new UpdatePostRequest
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Summary = model.Summary,
                        Body = model.Body,
                        IdCategory = model.IdCategory,
                        IsDraft = model.IsDraft,
                        PrincipalImageName = string.IsNullOrEmpty(finalFileName) && !model.DeleteImage ? post.PrincipalImageName : finalFileName,
                        Tags = model.Tags
                    });

                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Posts
        [Authorize(Roles = "User")]
        [ResponseType(typeof(Post))]
        public IHttpActionResult PostPost(CreatePostModel model)
        {
            try
            {
                var userService = new UserService();
                var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

                if (userAccountSuspended)
                {
                    return BadRequest("User account suspended");
                }

                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    model.IdWriter = user.Id;
                }

                if (ModelState.IsValid)
                {
                    var postService = new PostService();

                    string finalFileName = string.Empty;
                    string extension = string.Empty;
                    string[] tokens = new string[1];

                    if (!string.IsNullOrEmpty(model.base64File) && !string.IsNullOrWhiteSpace(model.base64File))
                    {
                        tokens = model.base64File.Split(',');
                        extension = tokens[0].Split('/')[1].Split(';')[0];
                    }

                    // Verifica si el modelo no tiene imagen, o tiene imagen y la misma tiene una extensión permitida.
                    if ((string.IsNullOrEmpty(model.base64File) && string.IsNullOrWhiteSpace(model.base64File)) ||
                        (!string.IsNullOrEmpty(extension) &&
                        new AllowedExtensions().ImageExtensions.Contains(extension.ToLower()))
                       )
                    {
                        var request = new CreatePostRequest
                        {
                            IdWriter = model.IdWriter,
                            Title = model.Title,
                            Summary = model.Summary,
                            Body = model.Body,
                            CategoryId = model.IdCategory,
                            IsDraft = model.IsDraft,
                            Tags = model.Tags
                        };

                        var createResult = postService.CreatePost(request);

                        if (!string.IsNullOrEmpty(extension))
                        {
                            var folder = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PathPostPrincipalImages"]);
                            finalFileName = string.Format("{0}.{1}", createResult.PostId.ToString(), extension);

                            string path = Path.Combine(folder, finalFileName);
                            var stream = ImageHelpers.Base64ToStream(tokens[1], path);
                            var image = new Bitmap(stream);

                            // Se guarda la imagen en el servidor.
                            image.Save(path);

                            // Se actualiza el post con la imagen que se acaba de guardar.
                            var updateResult = postService.UpdatePost(new UpdatePostRequest
                            {
                                Id = createResult.PostId,
                                Title = model.Title,
                                Summary = model.Summary,
                                Body = model.Body,
                                IdCategory = model.IdCategory,
                                IsDraft = model.IsDraft,
                                PrincipalImageName = "api/" + finalFileName,
                                Tags = model.Tags
                            });
                        }

                        return Ok();
                    }
                    else
                    {
                        return BadRequest("La extensión de la imagen no es válida, vuelva a cargarla.");
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Posts/5
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeletePost(int id)
        {
            try
            {
                var userService = new UserService();
                var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

                if (userAccountSuspended)
                {
                    return BadRequest("User account suspended");
                }

                var postService = new PostService();
                var post = postService.GetPostById(new GetPostByIdRequest() { Id = id }).Post;

                if (post == null)
                {
                    return NotFound();
                }

                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (post.IdWriter != user.Id)
                {
                    return BadRequest();
                }

                var result = postService.DeletePost(new DeletePostRequest { Id = id, IsComplaintOrVoteDifference = false });

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private GetPostModel GenerateGetPostModel(Post post)
        {
            var votesService = new VoteService();

            var result = TheModelFactory.CreateGetPostModel(post);
            var postVotes = votesService.GetVotesCountByPostId(new GetVotesCountByPostIdRequest { PostId = post.Id });

            result.GoodVotes = postVotes.GoodVotes;
            result.BadVotes = postVotes.BadVotes;

            result.Comments = GenerateGetCommentsModel(post.Id);

            return result;
        }

        private GetCommentsModel GenerateGetCommentsModel(int postId)
        {
            var commentaryService = new CommentaryService();
            var comments = commentaryService.SearchCommentsByIdPost(new SearchCommentsByIdPostRequest() { IdPost = postId }).Comments;

            var commentsResult = new GetCommentsModel();
            commentsResult.Comments = new List<CommentaryModel>();

            foreach (var commentary in comments)
            {
                if (!commentary.IdUpperComment.HasValue)
                {
                    var commentaryToAdd = TheModelFactory.CreateCommentaryModel(commentary);
                    commentaryToAdd.Answers = new List<AnswerModel>();
                    foreach (var answer in comments)
                    {
                        if (answer.IdUpperComment.HasValue)
                        {
                            if (answer.IdUpperComment == commentary.Id)
                            {
                                var answerToAdd = TheModelFactory.CreateAnswerModel(answer);
                                commentaryToAdd.Answers.Add(answerToAdd);
                            }
                        }
                    }
                    commentsResult.Comments.Add(commentaryToAdd);
                }
            }

            return commentsResult;
        }

        #endregion
    }
}
