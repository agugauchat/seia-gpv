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
        public JsonResult AddComment(string text, int post, int? idUpperComment)
        {
            if ((text.Length > 0) && (text.Length < 251))
            {
                var commentaryService = new CommentaryService();
                var userService = new UserService();
                var commentaryId = 0;
                var idUser = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User.Id;

                if (ModelState.IsValid)
                {
                    var request = new CreateCommentaryRequest
                    {
                        CommentaryText = text,
                        IdPost = post,
                        IdUser = idUser,
                        IdUpperComment = (idUpperComment == 0) ? null : idUpperComment
                    };

                    var result = commentaryService.CreateCommentary(request);
                    commentaryId = result.CommentaryId;
                }

                var htmlComment = "";
                var userName = User.Identity.GetUserName();
                var date = DateTime.Now.ToString("dd/MM/yyyy");

                if (idUpperComment == 0)
                {
                    //Es un comentario nuevo.
                    htmlComment = "<article class=\"row\" id=\"" + commentaryId + "\"><div class=\"col-md-10 col-sm-10\"><div class=\"panel panel-default arrow left\"><div class=\"panel-body\"><header class=\"text-left\"><div class=\"comment-user\"><i class=\"fa fa-user\"></i> <a href=\"/Profile/Details/" + idUser + "\">" + userName + "</a>   <time class=\"comment-date\"><i class=\"fa fa-clock-o\"></i> " + date + "</time></div></header><div class=\"comment-post\"><p style=\"margin-bottom: 5px;\">" + text + "<span>&nbsp;</span><small><a class=\"deleteCommentary text-danger\" href=\"#\" id=\"" + commentaryId + "\">Eliminar comentario</a></small></p></div><p class=\"text-right\"><a id=\"replyCommentary\" href=\"#\" class=\"btn btn-default btn-sm\" replyTo=\"" + userName + "\" commentary-id=\"" + commentaryId + "\"><i class=\"fa fa-reply\"></i> Responder</a></p></div></div></div></article><div id=\"newComment" + commentaryId + "\" class=\"media\"></div>";
                }
                else
                {
                    //Es una respuesta.
                    var upperCommentWriterUserName = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = idUpperComment.Value}).Commentary.WriterUserName;
                    htmlComment = "<article class=\"row\" id=\"" + commentaryId + "\">	<div class=\"col-md-9 col-sm-9 col-md-offset-1 col-sm-offset-0 hidden-xs\"><div class=\"panel panel-default arrow left\"><div class=\"panel-heading right\">En respuesta a <i>" + upperCommentWriterUserName + "</i></div><div class=\"panel-body\"><header class=\"text-left\"><div class=\"comment-user\"><i class=\"fa fa-user\"></i> <a href=\"/Profile/Details/" + idUser + "\">" + userName + "</a>   <time class=\"comment-date\"><i class=\"fa fa-clock-o\"></i> " + date + "</time></div></header><div class=\"comment-post\"><p>" + text + "<span>&nbsp;</span><small><a class=\"deleteCommentary text-danger\" href=\"#\" id=\"" + commentaryId + "\">Eliminar comentario</a></small></p></div></div></div></div></article>";
                }

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