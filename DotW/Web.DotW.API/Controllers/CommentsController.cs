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

namespace Web.DotW.API.Controllers
{
    public class CommentsController : BaseApiController
    {
        //// POST: api/Comments
        //[Authorize(Roles = "Admin")]
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

        //// DELETE: api/Comments/5
        //[Authorize(Roles = "Admin")]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult DeleteCommentary(int id)
        //{
        //    var CommentaryService = new CommentaryService();
        //    var postService = new PostService();
        //    var Commentary = CommentaryService.GetCommentaryById(new GetCommentaryByIdRequest() { Id = id }).Commentary;

        //    if (Commentary == null)
        //    {
        //        return NotFound();
        //    }

        //    var lowerComments = CommentaryService.SearchCommentsByIdUpperCommentary(new SearchCommentsByIdUpperCommentaryRequest() { IdUpperCommentary = id }).Comments;
        //    var relatedPosts = postService.SearchPostsByCommentaryId(new SearchPostsByCommentaryIdRequest() { IdCommentary = id }).Posts;

        //    // Si existen categorías hijas o publicaciones asociadas, la categoría no se puede eliminar.
        //    if ((lowerComments.Any()) || (relatedPosts.Any()))
        //    {
        //        return BadRequest("Can't delete this Commentary, is in use.");
        //    }

        //    // La categoría existe y no fue eliminada, entonces la elimino.
        //    var result = CommentaryService.DeleteCommentary(new DeleteCommentaryRequest() { Id = id });

        //    return Ok();
        //}
    }
}