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

                return Json(new { success = true, goodVotes = voteResult.PostGoodVotes, badVotes = voteResult.PostBadVotes }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}