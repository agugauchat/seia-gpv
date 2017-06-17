using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web.DotW.API.Infrastructure;
using Contracts.CommentaryContracts.Request;
using Web.DotW.API.Models;
using Services.CommentaryServices;
using Services.UserServices;
using Contracts.UserContracts.Request;
using Microsoft.AspNet.Identity;
using Services.PostServices;
using Contracts.PostContracts.Request;
using Services.ComplaintServices;
using Contracts.ComplaintContracts.Request;
using Contracts.ComplaintContracts;
using Entities.CommentaryEntities;
using Entities.ComplaintEntities;
using System.IO;
using System.Web;
using System.Configuration;

namespace Web.DotW.API.Controllers
{
    public class CommentsController : BaseApiController
    {
        // POST: api/Comments
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        [Route("api/Comments/", Name = "PostCommentary")]
        public IHttpActionResult PostCommentary(CreateCommentaryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var commentaryService = new CommentaryService();
                    var postService = new PostService();
                    var userService = new UserService();
                    var currentUserId = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User.Id;

                    // Controla que exista la publicación asociada.
                    var post = postService.GetPostById(new GetPostByIdRequest() { Id = model.IdPost }).Post;
                    if (post == null)
                    {
                        return BadRequest("Invalid post");
                    }
                    if (post.NullDate.HasValue)
                    {
                        return BadRequest("Invalid post");
                    }

                    if (model.IdUpperComment.HasValue)
                    {
                        // Controla que exista el comentario padre.
                        var upperCommentary = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = model.IdUpperComment.Value }).Commentary;
                        if (upperCommentary == null)
                        {
                            return BadRequest("Invalid upper commentary");
                        }
                        if (upperCommentary.NullDate.HasValue)
                        {
                            return BadRequest("Invalid upper commentary");
                        }

                        if (upperCommentary.IdUpperComment.HasValue)
                        {
                            return BadRequest("You can't respond an answer");
                        }
                    }

                    var request = new CreateCommentaryRequest()
                    {
                        CommentaryText = model.TextComment,
                        IdPost = model.IdPost,
                        IdUpperComment = model.IdUpperComment,
                        IdUser = currentUserId
                    };

                    var result = commentaryService.CreateCommentary(request);

                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Comments/5
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        [Route("api/Comments/{id}", Name = "DeleteCommentary")]
        public IHttpActionResult DeleteCommentary(int id)
        {
            var userService = new UserService();
            var commentaryService = new CommentaryService();
            var commentary = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = id }).Commentary;
            var currentUserId = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User.Id;

            if (commentary == null)
            {
                return NotFound();
            }

            if (commentary.NullDate.HasValue)
            {
                return NotFound();
            }

            if (currentUserId != commentary.IdUser)
            {
                return Unauthorized();
            }

            try
            {
                var result = commentaryService.DeleteCommentary(new DeleteCommentaryRequest() { Id = id });

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("api/comments/{id}/complaint", Name = "CommentaryComplaint")]
        public IHttpActionResult PostComplaint(int id, CommentaryComplaintModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                var userService = new UserService();
                var complaintService = new ComplaintService();
                var commentaryService = new CommentaryService();

                var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

                if (userAccountSuspended)
                {
                    return BadRequest("User account suspended");
                }

                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    var userComplaints = complaintService.SearchComplaintsByUserId(new SearchComplaintsByUserIdRequest { UserId = user.Id }).Complaints;

                    if (userComplaints.Any(x => x.IdComment == id))
                    {
                        return BadRequest("Complaint has been registered before");
                    }

                    var commentary = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest { Id = id }).Commentary;

                    if (commentary.IdUser == user.Id)
                    {
                        return BadRequest("You can not register a complaint to your own comment");
                    }

                    if (commentary.NullDate.HasValue)
                    {
                        return NotFound();
                    }

                    var complaintResult = complaintService.CreateCommentaryComplaint(new CreateCommentaryComplaintRequest { CommentaryId = id, UserId = user.Id, Commentary = model.Commentary });

                    if ((complaintResult.CommentaryComplaintsCount % (int)DividersToDeleteByComplaint.PostAndCommentaryDeletedDivider) == 0)
                    {
                        // Se da de baja el comentario.
                        var deletePostResult = commentaryService.DeleteCommentary(new DeleteCommentaryRequest { Id = complaintResult.CommentaryId, IsComplaintOrVoteDifference = true });

                        var complaints = complaintService.SearchComplaintsByCommentaryId(new SearchComplaintsByCommentaryIdRequest { CommentaryId = commentary.Id }).Complaints.OrderByDescending(x => x.Id).Take(3).ToList();

                        // Se notifica la baja del comentario via correo electrónico al escritor.
                        SendCommentaryDeletedEmailToWriter(commentary, complaints);

                        // Se verifica y de ser necesario, se suspende temporalmente la cuenta del usuario.
                        var verifyResult = userService.VerifyAndUpdateUserStateByComments(new VerifyAndUpdateUserStateByCommentsRequest { UserId = commentary.IdUser });

                        if (verifyResult.UserSuspended)
                        {
                            var reason = "La cantidad de comentarios dados de baja por denuncias ha alcanzando el número estipulado para suspender temporalmente su cuenta.";
                            SendAccountBlockedToWriter(commentary.IdUser, verifyResult.ActivationDate, reason);
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

        private void SendCommentaryDeletedEmailToWriter(Commentary commentary, IList<Complaint> complaints)
        {
            var postService = new PostService();
            var userService = new UserService();

            var writerUser = userService.GetUserById(new GetUserByIdRequest { UserId = commentary.IdUser }).User;
            var post = postService.GetPostById(new GetPostByIdRequest { Id = commentary.IdPost }).Post;

            if (writerUser != null)
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("no-reply@devsoftheweb.com", "Devs of the Web"),
                        new System.Net.Mail.MailAddress(writerUser.Email));

                m.Subject = "Comentario eliminado por denuncias.";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/EmailTemplate/CommentaryDeleted.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{UserName}", writerUser.Name);
                body = body.Replace("{Commentary}", commentary.CommentaryText);
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

        #endregion
    }
}