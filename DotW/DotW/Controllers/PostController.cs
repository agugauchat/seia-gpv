namespace DotW.Controllers
{
    using Contracts.CategoryContracts.Request;
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Microsoft.AspNet.Identity;
    using Models;
    using Services.CategoryServices;
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

        public ActionResult List(int? idCategory)
        {
            var postService = new PostService();
            var categoryService = new CategoryService();

            var model = new ListPostViewModel();

            if (idCategory.HasValue)
            {
                model.Posts = postService.SearchPostsByCategoryId(new SearchPostsByCategoryIdRequest { IdCategory = idCategory.Value }).Posts.ToList();
            }
            else
            {
                model.Posts = postService.SearchPosts(new SearchPostsRequest()).Posts.ToList();
            }

            ViewBag.Categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;

            return View(model);
        }

        [Authorize]
        public ActionResult Create()
        {
            var categoryService = new CategoryService();

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePostViewModel model)
        {
            var userService = new UserService();
            var postService = new PostService();
            var categoryService = new CategoryService();

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
                    Body = model.Body,
                    CategoryId = model.IdCategory
                };

                var result = postService.CreatePost(request);

                return RedirectToAction("Index", "Post");
            }

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var postService = new PostService();
            var categoryService = new CategoryService();

            var result = postService.GetPostById(new GetPostByIdRequest { Id = id }).Post;

            var model = new EditPostViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Body = result.Body,
                IdCategory = result.IdCategory
            };

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPostViewModel model)
        {
            var postService = new PostService();
            var categoryService = new CategoryService();

            if (ModelState.IsValid)
            {
                var result = postService.UpdatePost(new UpdatePostRequest
                {
                    Id = model.Id,
                    Title = model.Title,
                    Body = model.Body,
                    IdCategory = model.IdCategory
                });

                return RedirectToAction("Index", "Post");
            }

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
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
                CategoryTitle = result.CategoryTitle
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