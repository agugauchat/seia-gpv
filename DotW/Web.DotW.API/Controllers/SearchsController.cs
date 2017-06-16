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
using Services.SearchServices;
using Contracts.SearchContracts.Request;

namespace Web.DotW.API.Controllers
{
    public class SearchsController : BaseApiController
    {
        // GET: api/Searchs
        [AllowAnonymous]
        [ResponseType(typeof(SearchModel))]
        [Route("api/Searchs/{id}", Name = "GetSearchs")]
        public IHttpActionResult GetSearchs(string id)
        {
            var searchService = new SearchService();

            var PostsList = searchService.SearchInPosts(new SearchInPostsRequest { Text = id }).PostsSearchResult;

            var CommentsList = searchService.SearchInComments(new SearchInCommentsRequest { Text = id }).CommentsSearchResult;

            // TODO Agu -> Armar las listas de objetos a devolver.
            return Ok();
        }
    }
}