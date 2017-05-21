namespace DotW.Controllers
{
    using Contracts.CommentaryContracts.Request;
    using Contracts.UserContracts.Request;
    using Microsoft.AspNet.Identity;
    using Models;
    using Services.CommentaryServices;
    using Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class CommentaryController : BaseController
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult AddComment(string text, int post)
        {
            if ((text.Length > 0) && (text.Length < 251))
            {
                var commentaryService = new CommentaryService();
                var userService = new UserService();
                var commentaryId = 0;

                if (ModelState.IsValid)
                {
                    var request = new CreateCommentaryRequest
                    {
                        CommentaryText = text,
                        IdPost = post,
                        IdUser = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User.Id
                    };

                    var result = commentaryService.CreateCommentary(request);
                    commentaryId = result.CommentaryId;
                }

                var imgPath = Url.Content("~/Content/Images/GenericUser.png");
                var userName = User.Identity.GetUserName();
                var date = DateTime.Now.ToString("dd/MM/yyyy");
                var htmlComment = "<div class=\"media\" id=\"" + commentaryId + "\"><a class=\"pull-left\" href=\"#\"  onclick=\"return false; \"><img class=\"media-object\" src=\"" + imgPath + "\" height=\"64\" width=\"64\"></a><div class=\"media-body\"><h4 class=\"media-heading\">"+ userName +"<small> "+date+ "</small><span>&nbsp;&nbsp;&nbsp;</span><small><a class=\"deleteCommentary text-danger\" href=\"#\" id=\"" + commentaryId + "\">Eliminar comentario</a></small></h4>" + text + "</div></div>";

                return Json(new { success = htmlComment });
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult DeleteComment(int idCommentary)
        {
            var commentaryService = new CommentaryService();

            var commentary = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = idCommentary }).Commentary;

            if ((commentary != null) && (commentary.WriterUserName == User.Identity.Name))
            {
                var result = commentaryService.DeleteCommentary(new DeleteCommentaryRequest() { Id = idCommentary });

                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}