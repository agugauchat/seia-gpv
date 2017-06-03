using Contracts.BanContracts.Request;
using Contracts.CommentaryContracts.Request;
using Contracts.ComplaintContracts.Request;
using Services.BanServices;
using Services.CommentaryServices;
using Services.ComplaintServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotW.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BansController : BaseController
    {
        public ActionResult Index()
        {
            var banService = new BanService();

            ViewBag.Posts = banService.SearchBannedPosts(new SearchBannedPostsRequest()).Posts;
            ViewBag.Comments = banService.SearchBannedComments(new SearchBannedCommentsRequest()).Comments;
            ViewBag.Users = banService.SearchBannedUsers(new SearchBannedUsersRequest()).Users;

            return View();
        }

        public ActionResult EnablePost(int id)
        {
            var banService = new BanService();

            var result = banService.EnablePost(new EnablePostRequest { PostId = id });

            return RedirectToAction("Index");
        }

        public ActionResult EnableCommentary(int id)
        {
            var banService = new BanService();

            var result = banService.EnableCommentary(new EnableCommentaryRequest { CommentaryId = id });

            return RedirectToAction("Index");
        }

        public ActionResult EnableUser(int id)
        {
            var banService = new BanService();

            var result = banService.EnableUser(new EnableUserRequest { UserId = id });

            SendAccountEnabledToUser(result.UserName, result.Email);

            return RedirectToAction("Index");
        }

        public ActionResult CommentaryComplaints(int id)
        {
            var commentaryService = new CommentaryService();
            var complaintService = new ComplaintService();

            ViewBag.Comment = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest { Id = id }).Commentary;
            ViewBag.CommentaryComplaints = complaintService.SearchComplaintsByCommentaryId(new SearchComplaintsByCommentaryIdRequest { CommentaryId = id }).Complaints;

            return View();
        }

        #region Private Methods

        private void SendAccountEnabledToUser(string userName, string email)
        {
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("no-reply@devsoftheweb.com", "Devs of the Web"),
                        new System.Net.Mail.MailAddress(email));

            m.Subject = "Cuenta habilitada nuevamente.";

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/EmailTemplate/AccountEnabled.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{UserName}", userName);

            m.Body = body;

            m.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            smtp.Credentials = new System.Net.NetworkCredential("devsoftheweb@gmail.com", emailPassword);
            smtp.Send(m);
        }

        #endregion
    }
}