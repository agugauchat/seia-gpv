using Contracts.BanContracts.Request;
using Contracts.CommentaryContracts.Request;
using Contracts.ComplaintContracts.Request;
using Services.BanServices;
using Services.CommentaryServices;
using Services.ComplaintServices;
using System;
using System.Collections.Generic;
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
    }
}