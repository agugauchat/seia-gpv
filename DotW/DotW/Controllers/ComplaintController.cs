namespace DotW.Controllers
{
    using Contracts.CommentaryContracts.Request;
    using Contracts.ComplaintContracts;
    using Contracts.ComplaintContracts.Request;
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using DotW.Models;
    using Entities.CommentaryEntities;
    using Entities.ComplaintEntities;
    using Entities.PostEntities;
    using Microsoft.AspNet.Identity;
    using Services.CommentaryServices;
    using Services.ComplaintServices;
    using Services.PostServices;
    using Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    public class ComplaintController : BaseController
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult PostComplaint(PostComplaintViewModel model)
        {
            var complaintService = new ComplaintService();
            var userService = new UserService();
            var postService = new PostService();

            try
            {
                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    var userComplaints = complaintService.SearchComplaintsByUserId(new SearchComplaintsByUserIdRequest { UserId = user.Id }).Complaints;

                    if (userComplaints.Any(x => x.IdPost == model.PostId))
                    {
                        return Json(new { success = false, Message = "Ya se ha registrado una denuncia para esta cuenta en esta publicación." }, JsonRequestBehavior.AllowGet);
                    }

                    var post = postService.GetPostById(new GetPostByIdRequest { Id = model.PostId }).Post;

                    if (post.IdWriter == user.Id)
                    {
                        return Json(new { success = false, Message = "No puede denunciar su propia publicación." }, JsonRequestBehavior.AllowGet);
                    }

                    var complaintResult = complaintService.CreatePostComplaint(new CreatePostComplaintRequest { PostId = model.PostId, UserId = user.Id, Commentary = model.Commentary });

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

                    return Json(new { success = true, Message = "<div style='text-align:justify;'>Su denuncia ha sido registrada y será colocada junto a la de otros usuarios. Si se alcanza el límite establecido en nuestros <a href='/Home/TermsAndConditions' target='_blank'>Términos y Condiciones</a>, la publicación será dada de baja.<br><br>Gracias por contribuir con nuestra comunidad :)</div>" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, Message = "Ha ocurrido un error al procesar la solicitud." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, Message = "Ha ocurrido un error al procesar la solicitud." }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult CommentaryComplaint(CommentaryComplaintViewModel model)
        {
            var commentaryService = new CommentaryService();
            var complaintService = new ComplaintService();
            var userService = new UserService();

            try
            {
                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    var userComplaints = complaintService.SearchComplaintsByUserId(new SearchComplaintsByUserIdRequest { UserId = user.Id }).Complaints;

                    if (userComplaints.Any(x => x.IdComment == model.CommentaryId))
                    {
                        return Json(new { success = false, Message = "Ya se ha registrado una denuncia para esta cuenta en este comentario." }, JsonRequestBehavior.AllowGet);
                    }

                    var commentary = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest { Id = model.CommentaryId }).Commentary;

                    if (commentary.IdUser == user.Id)
                    {
                        return Json(new { success = false, Message = "No puede denunciar su propio comentario." }, JsonRequestBehavior.AllowGet);
                    }

                    var complaintResult = complaintService.CreateCommentaryComplaint(new CreateCommentaryComplaintRequest { CommentaryId = model.CommentaryId, UserId = user.Id, Commentary = model.Commentary });

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

                    return Json(new { success = true, Message = "<div style='text-align:justify;'>Su denuncia ha sido registrada y será colocada junto a la de otros usuarios. Si se alcanza el límite establecido en nuestros <a href='/Home/TermsAndConditions' target='_blank'>Términos y Condiciones</a>, el comentario será dado de baja.<br><br>Gracias por contribuir con nuestra comunidad :)</div>" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, Message = "Ha ocurrido un error al procesar la solicitud." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, Message = "Ha ocurrido un error al procesar la solicitud." }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Private Methods

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
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/EmailTemplate/PostDeletedByComplaints.html")))
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
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/EmailTemplate/CommentaryDeleted.html")))
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
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/EmailTemplate/AccountBlocked.html")))
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