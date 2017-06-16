using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web.DotW.API.Infrastructure;
using Contracts.CommentaryContracts.Request;
using Web.DotW.API.Models;
using Services.CommentaryServices;
using Services.UserServices;
using Contracts.UserContracts.Request;
using Microsoft.AspNet.Identity;
using Services.PostServices;
using Contracts.PostContracts.Request;

namespace Web.DotW.API.Controllers
{
    public class CommentsController : BaseApiController
    {
        // POST: api/Comments
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        [Route("api/Comments/", Name = "PostCommentary")]
        public IHttpActionResult PostCommentary(CreateCommentaryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var commentaryService = new CommentaryService();
                    var postService = new PostService();
                    var userService = new UserService();
                    var currentUserId = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User.Id;

                    // Controla que exista la publicación asociada.
                    var post = postService.GetPostById(new GetPostByIdRequest() { Id = model.IdPost }).Post;
                    if (post == null)
                    {
                        return BadRequest("Invalid post");
                    }
                    if (post.NullDate.HasValue)
                    {
                        return BadRequest("Invalid post");
                    }

                    if (model.IdUpperComment.HasValue)
                    {
                        // Controla que exista el comentario padre.
                        var upperCommentary = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = model.IdUpperComment.Value }).Commentary;
                        if (upperCommentary == null)
                        {
                            return BadRequest("Invalid upper commentary");
                        }
                        if (upperCommentary.NullDate.HasValue)
                        {
                            return BadRequest("Invalid upper commentary");
                        }

                        if (upperCommentary.IdUpperComment.HasValue)
                        {
                            return BadRequest("You can't respond an answer");
                        }
                    }

                    var request = new CreateCommentaryRequest()
                    {
                        CommentaryText = model.TextComment,
                        IdPost = model.IdPost,
                        IdUpperComment = model.IdUpperComment,
                        IdUser = currentUserId
                    };

                    var result = commentaryService.CreateCommentary(request);

                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Comments/5
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        [Route("api/Comments/{id}", Name = "DeleteCommentary")]
        public IHttpActionResult DeleteCommentary(int id)
        {
            var userService = new UserService();
            var commentaryService = new CommentaryService();
            var commentary = commentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = id }).Commentary;
            var currentUserId = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User.Id;

            if (commentary == null)
            {
                return NotFound();
            }

            if (commentary.NullDate.HasValue)
            {
                return NotFound();
            }

            if (currentUserId != commentary.IdUser)
            {
                return Unauthorized();
            }

            try
            {
                var result = commentaryService.DeleteCommentary(new DeleteCommentaryRequest() { Id = id });

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}