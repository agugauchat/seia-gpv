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
using Contracts.SearchContracts.Request;
using Services.PostServices;
using Contracts.PostContracts.Request;

namespace Web.DotW.API.Controllers
{
    public class UsersController : BaseApiController
    {
        // GET: api/Users/{text}
        [Authorize]
        [ResponseType(typeof(UserModel))]
        [Route("api/Users/{id}", Name = "GetUser")]
        public IHttpActionResult GetUser(int id)
        {
            var userService = new UserService();
            var postService = new PostService();
            var user = userService.GetUserById(new GetUserByIdRequest() { UserId = id }).User;

            if (user == null)
            {
                return NotFound();
            }

            var result = TheModelFactory.CreateUserModel(user);

            var posts = postService.SearchPostsByUserId(new SearchPostsByUserIdRequest { UserId = id }).Posts.OrderByDescending(x => x.EffectDate).ToList();
            
            result.Post = new List<PostsForUserDetail>();

            foreach (var post in posts)
            {
                var postResult = TheModelFactory.CreatePostsForUserDetail(post);

                result.Post.Add(postResult);
            }

            return Ok(result);
        }
    }
}