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

namespace Web.DotW.API.Controllers
{
    public class CommentsController : BaseApiController
    {
        //// POST: api/Comments
        //[Authorize(Roles = "User")]
        //[ResponseType(typeof(Commentary))]
        //public IHttpActionResult PostCommentary(CreateCommentaryModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var CommentaryService = new CommentaryService();

        //            if (model.IdUpperCommentary.HasValue)
        //            {
        //                // Controla que exista la categoría padre.
        //                var upperCommentary = CommentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = model.IdUpperCommentary.Value }).Commentary;
        //                if (upperCommentary != null)
        //                {
        //                    return BadRequest("Invalid upper Commentary");
        //                }
        //            }

        //            var request = new CreateCommentaryRequest
        //            {
        //                Title = model.Title,
        //                Summary = model.Summary,
        //                Description = model.Description,
        //                IdUpperCommentary = model.IdUpperCommentary
        //            };

        //            var result = CommentaryService.CreateCommentary(request);

        //            return Ok();
        //        }
        //        catch (Exception)
        //        {
        //            return BadRequest();
        //        }
        //    }

        //    return BadRequest(ModelState);
        //}

        // DELETE: api/Comments/5
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
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