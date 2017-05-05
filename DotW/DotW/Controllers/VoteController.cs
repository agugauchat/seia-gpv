namespace DotW.Controllers
{
    using Contracts.UserContracts.Request;
    using Contracts.VoteContracts.Request;
    using Microsoft.AspNet.Identity;
    using Services.UserServices;
    using Services.VoteServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class VoteController : BaseController
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult SaveVote(int postId, bool? goodVote, bool? badVote)
        {
            var userService = new UserService();
            var voteService = new VoteService();

            var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

            if (user != null)
            {
                var result = voteService.SaveVote(new SaveVoteRequest { PostId = postId, UserId = user.Id, Good = goodVote, Bad = badVote });

                return Json(new { success = true, goodVotes = result.PostGoodVotes, badVotes = result.PostBadVotes }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}