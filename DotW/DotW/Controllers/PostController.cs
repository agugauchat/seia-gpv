namespace DotW.Controllers
{
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Microsoft.AspNet.Identity;
    using Models;
    using Services.PostServices;
    using Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class PostController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            var postService = new PostService();

            var model = new IndexPostViewModel();

            model.Posts = postService.SearchPostsByUserId(new SearchPostsByUserIdRequest { AspNetUserId = User.Identity.GetUserId() }).Posts.ToList();

            return View(model);
        }

        public ActionResult List()
        {
            var postService = new PostService();

            var model = new ListPostViewModel();

            model.Posts = postService.SearchPosts(new SearchPostsRequest()).Posts.ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult Create()
        {
            //TODO > Agregar lista de categorias.

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePostViewModel model)
        {
            var userService = new UserService();
            var postService = new PostService();

            //TODO > Más adelante hay que validar el estado del usuario antes de permitir publicar.

            var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

            if (user != null)
            {
                model.IdWriter = user.Id;
            }
            
            if (ModelState.IsValid)
            {
                var request = new CreatePostRequest
                {
                    IdWriter = model.IdWriter,
                    Title = model.Title,
                    Body = model.Body
                };

                var result = postService.CreatePost(request);

                return RedirectToAction("Index", "Post");
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var postService = new PostService();

            var result = postService.GetPostById(new GetPostByIdRequest { Id = id }).Post;

            var model = new EditPostViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Body = result.Body,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditPostViewModel model)
        {
            var postService = new PostService();

            var result = postService.UpdatePost(new UpdatePostRequest
            {
                Id = model.Id,
                Title = model.Title,
                Body = model.Body
            });

            return RedirectToAction("Index", "Post");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var postService = new PostService();

            var result = postService.GetPostById(new GetPostByIdRequest { Id = id }).Post;

            var model = new DeletePostViewModel
            {
                Id = result.Id,
                Title = result.Title,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(DeletePostViewModel model)
        {
            var postService = new PostService();

            var result = postService.DeletePost(new DeletePostRequest { Id = model.Id });

            return RedirectToAction("Index", "Post");
        }
    }
}