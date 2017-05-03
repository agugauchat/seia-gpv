using System;
using System.Collections.Generic;
using System.Linq;
namespace DotW.Controllers
{
    using Contracts.ComplaintContracts.Request;
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Contracts.UserContracts.Response;
    using DotW.Models;
    using Microsoft.AspNet.Identity;
    using Services.ComplaintServices;
    using Services.PostServices;
    using Services.UserServices;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;

    public class ComplaintController : BaseController
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult PostComplaint(ComplaintPostViewModel model)
        {
            var complaintService = new ComplaintService();
            var userService = new UserService();

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

                    var complaintResult = complaintService.CreatePostComplaint(new CreatePostComplaintRequest { PostId = model.PostId, UserId = user.Id, Commentary = model.Commentary });

                    if (complaintResult.PostComplaintsCount >= 3)
                    {
                        var postService = new PostService();

                        // Se da de baja la publicación por haber alcanzado/superado las 3 denuncias.
                        var deletePostResult = postService.DeletePost(new DeletePostRequest { Id = complaintResult.PostId });

                        var post = postService.GetPostById(new GetPostByIdRequest { Id = complaintResult.PostId }).Post;

                        SendEmailToWriter(post.IdWriter);
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

        private void SendEmailToWriter(int idWriter)
        {
            var userService = new UserService();

            var writerUser = userService.GetUserById(new GetUserByIdRequest { UserId = idWriter }).User;

            if (writerUser != null)
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("no-reply@devsoftheweb.com", "Devs of the Web"),
                        new System.Net.Mail.MailAddress(writerUser.Email));

                m.Subject = "Verificar email";
                m.Body = string.Format("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=UTF-8\"><h3>Hola {0}!</h3><p>Tu publicación fue dada de baja.</p><br/><p align=\"right\"><small>Devs of the Web Team. &#169;</small></p>", writerUser.Name);
                //m.Body = System.IO.File.ReadAllText(Server.MapPath("EmailTemplates/Customer.htm"));
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