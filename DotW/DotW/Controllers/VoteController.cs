namespace DotW.Controllers
{
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Contracts.VoteContracts;
    using Contracts.VoteContracts.Request;
    using Microsoft.AspNet.Identity;
    using Services.PostServices;
    using Services.UserServices;
    using Services.VoteServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Contracts.VoteContracts.Response;
    using Entities.PostEntities;
    using System.IO;
    using System.Configuration;

    public class VoteController : BaseController
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult SaveVote(int postId, bool goodVote, bool badVote)
        {
            var postService = new PostService();
            var userService = new UserService();
            var voteService = new VoteService();

            var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

            if (user != null)
            {
                var post = postService.GetPostById(new GetPostByIdRequest { Id = postId }).Post;

                // Se guarda el voto del usuario.
                var voteResult = voteService.SaveVote(new SaveVoteRequest { PostId = postId, UserId = user.Id, Good = goodVote, Bad = badVote });

                if ((voteResult.PostBadVotes - voteResult.PostGoodVotes) >= (int)VotesDifferenceToDelete.MaxDifferenceBetweenNegativeAndPositivesVotes)
                {
                    // Se da de baja la publicación por haber alcanzado/superado la diferencia límite entre votos
                    // negativos y positivos.
                    var deletePostResult = postService.DeletePost(new DeletePostRequest { Id = voteResult.PostId, IsComplaintOrVoteDifference = true });

                    // Se notifica la baja del post via correo electrónico al escritor.
                    SendPostDeletedByVotesEmailToWriter(post, voteResult);
                }

                return Json(new { success = true, goodVotes = voteResult.PostGoodVotes, badVotes = voteResult.PostBadVotes }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Private Methods

        private void SendPostDeletedByVotesEmailToWriter(Post post, SaveVoteResponse voteResult)
        {
            var userService = new UserService();

            var writerUser = userService.GetUserById(new GetUserByIdRequest { UserId = post.IdWriter }).User;

            if (writerUser != null)
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("no-reply@devsoftheweb.com", "Devs of the Web"),
                        new System.Net.Mail.MailAddress(writerUser.Email));

                m.Subject = "Publicación eliminada por votos.";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/EmailTemplate/PostDeletedByVotes.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{UserName}", writerUser.Name);
                body = body.Replace("{PostTitle}", post.Title);
                body = body.Replace("{GoodVotes}", voteResult.PostGoodVotes.ToString());
                body = body.Replace("{BadVotes}", voteResult.PostBadVotes.ToString());

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