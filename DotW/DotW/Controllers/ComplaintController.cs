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

                    if (complaintResult.PostComplaintsCount >= (int)AllowedComplaints.MaxPostAndCommentaryComplaints)
                    {
                        // Se da de baja la publicación por haber alcanzado/superado las 3 denuncias.
                        var deletePostResult = postService.DeletePost(new DeletePostRequest { Id = complaintResult.PostId, IsComplaintOrVoteDifference = true });

                        var complaints = complaintService.SearchComplaintsByPostId(new SearchComplaintsByPostIdRequest { PostId = post.Id }).Complaints;

                        // Se notifica la baja del post via correo electrónico al escritor.
                        SendPostDeletedEmailToWriter(post, complaints);
                    }

                    return Json(new { success = true, Message = "Su denuncia ha sido registrada. Gracias por contribuir con nuestra comunidad :)" }, JsonRequestBehavior.AllowGet);
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

                    if (complaintResult.CommentaryComplaintsCount >= (int)AllowedComplaints.MaxPostAndCommentaryComplaints)
                    {                       
                        // Se da de baja el comentario por haber alcanzado/superado las 3 denuncias.
                        var deletePostResult = commentaryService.DeleteCommentary(new DeleteCommentaryRequest { Id = complaintResult.CommentaryId, IsComplaintOrVoteDifference = true });

                        var complaints = complaintService.SearchComplaintsByCommentaryId(new SearchComplaintsByCommentaryIdRequest { CommentaryId = commentary.Id }).Complaints;

                        // Se notifica la baja del comentario via correo electrónico al escritor.
                        SendCommentaryDeletedEmailToWriter(commentary, complaints);
                    }

                    return Json(new { success = true, Message = "Su denuncia ha sido registrada. Gracias por contribuir con nuestra comunidad :)" }, JsonRequestBehavior.AllowGet);
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
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/EmailTemplate/PostDeleted.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{UserName}", writerUser.Name);
                body = body.Replace("{PostTitle}", post.Title);
                body = body.Replace("{FirstComplaint}", complaints[0].Description);
                body = body.Replace("{SecondComplaint}", complaints[1].Description);
                body = body.Replace("{ThirdComplaint}", complaints[2].Description);

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
                body = body.Replace("{FirstComplaint}", complaints[0].Description);
                body = body.Replace("{SecondComplaint}", complaints[1].Description);
                body = body.Replace("{ThirdComplaint}", complaints[2].Description);

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