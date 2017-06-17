namespace Web.DotW.API.Controllers
{
    using Contracts.CategoryContracts.Request;
    using Contracts.CommentaryContracts.Request;
    using Contracts.ComplaintContracts;
    using Contracts.ComplaintContracts.Request;
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Contracts.VoteContracts.Request;
    using Entities.ComplaintEntities;
    using Entities.General;
    using Entities.PostEntities;
    using Microsoft.AspNet.Identity;
    using Services.CategoryServices;
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
        [Route("api/posts/", Name = "GetPosts")]
        public IHttpActionResult GetPosts()
        {
            try
            {
                var postService = new PostService();
                var votesService = new VoteService();
                var commentaryService = new CommentaryService();
                var posts = postService.SearchPosts(new SearchPostsRequest()).Posts;
                var result = new List<GetPostModel>();

                if (posts.Any())
                {
                    foreach (var post in posts)
                    {
                        var postResult = GenerateGetPostModel(post);

                        result.Add(postResult);
                    }

                    return Ok(result);
                }

                return NotFound();
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET: api/Posts/5
        [AllowAnonymous]
        [ResponseType(typeof(Post))]
        [Route("api/posts/{id}", Name = "GetPostById")]
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

                if (post.NullDate.HasValue || post.IsDraft)
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
        [Route("api/tags/{tag}/posts", Name = "GetPostsByTag")]
        public IHttpActionResult GetPostsByTag(string tag)
        {
            try
            {
                var postService = new PostService();

                if (!string.IsNullOrEmpty(tag))
                {
                    var posts = postService.SearchPostsByTag(new SearchPostsByTagRequest { Tag = tag }).Posts.Where(x => !x.IsDraft).OrderByDescending(x => x.EffectDate).ToList();
                    var result = new List<GetPostModel>();

                    if (posts.Any())
                    {
                        foreach (var post in posts)
                        {
                            var postResult = GenerateGetPostModel(post);

                            result.Add(postResult);
                        }

                        return Ok(result);
                    }

                    return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/categories/{id}/posts", Name = "GetPostsByCategory")]
        public IHttpActionResult GetPostsByTag(int id)
        {
            try
            {
                var postService = new PostService();

                var posts = postService.SearchPostsByCategoryId(new SearchPostsByCategoryIdRequest { IdCategory = id }).Posts.OrderByDescending(x => x.EffectDate).ToList();
                var result = new List<GetPostModel>();

                if (posts.Any())
                {
                    foreach (var post in posts)
                    {
                        var postResult = GenerateGetPostModel(post);

                        result.Add(postResult);
                    }

                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [ResponseType(typeof(GetCommentsModel))]
        [Route("api/posts/{id}/comments", Name = "GetCommentsByPostId")]
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
        [Route("api/posts/", Name = "PutPost")]
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
        [Route("api/posts/", Name = "PostPost")]
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
        [Route("api/posts/{id}", Name = "DeletePost")]
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

                if (post.NullDate.HasValue)
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

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("api/posts/{id}/vote", Name = "VotePost")]
        public IHttpActionResult VotePost(int id, VotePostModel Model)
        {
            try
            {
                if (Model.Bad && Model.Good)
                {
                    return BadRequest("Your vote include good and bad vote at the same time.");
                }

                var userService = new UserService();
                var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

                if (userAccountSuspended)
                {
                    return BadRequest("User account suspended");
                }

                var postService = new PostService();
                var voteService = new VoteService();

                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    var post = postService.GetPostById(new GetPostByIdRequest { Id = id }).Post;

                    if (post.NullDate.HasValue || post.IsDraft)
                    {
                        return NotFound();
                    }

                    // Se guarda el voto del usuario.
                    var voteResult = voteService.SaveVote(new SaveVoteRequest { PostId = id, UserId = user.Id, Good = Model.Good, Bad = Model.Bad });

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("api/posts/{id}/complaint", Name = "PostComplaint")]
        public IHttpActionResult PostComplaint(int id, PostComplaintModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                var userService = new UserService();
                var complaintService = new ComplaintService();
                var postService = new PostService();

                var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

                if (userAccountSuspended)
                {
                    return BadRequest("User account suspended");
                }

                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    var userComplaints = complaintService.SearchComplaintsByUserId(new SearchComplaintsByUserIdRequest { UserId = user.Id }).Complaints;

                    if (userComplaints.Any(x => x.IdPost == id))
                    {
                        return BadRequest("Complaint has been registered before");
                    }

                    var post = postService.GetPostById(new GetPostByIdRequest { Id = id }).Post;

                    if (post.IdWriter == user.Id)
                    {
                        return BadRequest("You can not register a complaint to your own post");
                    }

                    if (post.NullDate.HasValue || post.IsDraft)
                    {
                        return NotFound();
                    }

                    var complaintResult = complaintService.CreatePostComplaint(new CreatePostComplaintRequest { PostId = id, UserId = user.Id, Commentary = model.Commentary });

                    if ((complaintResult.PostComplaintsCount % (int)DividersToDeleteByComplaint.PostAndCommentaryDeletedDivider) == 0)
                    {
                        // Se da de baja la publicación.
                        var deletePostResult = postService.DeletePost(new DeletePostRequest { Id = complaintResult.PostId, IsComplaintOrVoteDifference = true });

                        var complaints = complaintService.SearchComplaintsByPostId(new SearchComplaintsByPostIdRequest { PostId = post.Id }).Complaints.OrderByDescending(x => x.Id).Take(3).ToList();

                        // Se notifica la baja del post via correo electrónico al escritor.
                        SendPostDeletedEmailToWriter(post, complaints);

                        // Se verifica y de ser necesario, se suspende temporalmente la cuenta del usuario.
                        var verifyResult = userService.VerifyAndUpdateUserStateByPosts(new VerifyAndUpdateUserStateByPostsRequest { UserId = post.IdWriter });

                        if (verifyResult.UserSuspended)
                        {
                            var reason = "La cantidad de publicaciones dadas de baja por denuncias ha alcanzando el número estipulado para suspender temporalmente su cuenta.";
                            SendAccountBlockedToWriter(post.IdWriter, verifyResult.ActivationDate, reason);
                        };
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

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
            var categoryService = new CategoryService();

            var category = categoryService.GetCategoryById(new GetCategoryByIdRequest { Id = post.IdCategory }).Category;

            var result = TheModelFactory.CreateGetPostModel(post, category);
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

        private void SendPostDeletedEmailToWriter(Post post, IList<Complaint> complaints)
        {
            var userService = new UserService();

            var writerUser = userService.GetUserById(new GetUserByIdRequest { UserId = post.IdWriter }).User;

            if (writerUser != null)
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("no-reply@devsoftheweb.com", "Devs of the Web"),
                        new System.Net.Mail.MailAddress(writerUser.Email));

                m.Subject = "Publicación eliminada por denuncias.";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/EmailTemplate/PostDeletedByComplaints.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{UserName}", writerUser.Name);
                body = body.Replace("{PostTitle}", post.Title);
                body = body.Replace("{FirstComplaint}", string.IsNullOrEmpty(complaints[0].Description) ? "Denuncia sin comentario." : complaints[0].Description);
                body = body.Replace("{SecondComplaint}", string.IsNullOrEmpty(complaints[1].Description) ? "Denuncia sin comentario." : complaints[1].Description);
                body = body.Replace("{ThirdComplaint}", string.IsNullOrEmpty(complaints[2].Description) ? "Denuncia sin comentario." : complaints[2].Description);

                m.Body = body;

                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
                smtp.Credentials = new System.Net.NetworkCredential("devsoftheweb@gmail.com", emailPassword);
                smtp.Send(m);
            }
        }

        private void SendAccountBlockedToWriter(int idWriter, DateTime activationDate, string reason)
        {
            var userService = new UserService();

            var writerUser = userService.GetUserById(new GetUserByIdRequest { UserId = idWriter }).User;

            if (writerUser != null)
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("no-reply@devsoftheweb.com", "Devs of the Web"),
                        new System.Net.Mail.MailAddress(writerUser.Email));

                m.Subject = "Cuenta suspendida.";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/EmailTemplate/AccountBlocked.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{UserName}", writerUser.Name);
                body = body.Replace("{Reason}", reason);
                body = body.Replace("{ActivationDate}", activationDate.ToString("dd/MM/yyyy"));

                m.Body = body;

                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
                smtp.Credentials = new System.Net.NetworkCredential("devsoftheweb@gmail.com", emailPassword);
                smtp.Send(m);
            }
        }

        #endregion
    }
}
