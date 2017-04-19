namespace DotW.Controllers
{
    using Contracts.CategoryContracts.Request;
    using Contracts.PostContracts.Request;
    using Microsoft.AspNet.Identity;
    using Models;
    using Services.CategoryServices;
    using Services.PostServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class CategoryController : BaseController
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var categoryService = new CategoryService();
            var model = new IndexCategoryViewModel();

            model.Categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories.ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryViewModel model)
        {
            var categoryService = new CategoryService();

            if (ModelState.IsValid)
            {
                var request = new CreateCategoryRequest
                {
                    Title = model.Title,
                    Summary = model.Summary,
                    Description = model.Description,
                    IdUpperCategory = model.IdUpperCategory
                };

                var result = categoryService.CreateCategory(request);

                return RedirectToAction("Index", "Category");
            }

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var categoryService = new CategoryService();

            var result = categoryService.GetCategoryById(new GetCategoryByIdRequest { Id = id }).Category;

            var model = new EditCategoryViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Summary = result.Summary,
                Description = result.Description,
                IdUpperCategory = result.IdUpperCategory
            };

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel model)
        {
            var categoryService = new CategoryService();

            if (ModelState.IsValid)
            {
                var result = categoryService.UpdateCategory(new UpdateCategoryRequest
                {
                    Id = model.Id,
                    Title = model.Title,
                    Summary = model.Summary,
                    Description = model.Description,
                    IdUpperCategory = model.IdUpperCategory
                });

                return RedirectToAction("Index", "Category");
            }

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var categoryService = new CategoryService();

            var result = categoryService.GetCategoryById(new GetCategoryByIdRequest { Id = id }).Category;

            var model = new DeleteCategoryViewModel
            {
                Id = result.Id,
                Title = result.Title,
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(DeleteCategoryViewModel model)
        {
            var categoryService = new CategoryService();
            var postService = new PostService();

            var post = postService.SearchPostsByCategoryId(new SearchPostsByCategoryIdRequest { IdCategory = model.Id }).Posts;
            var childCategories = categoryService.SearchCategoriesByIdUpperCategory(new SearchCategoriesByIdUpperCategoryRequest { IdUpperCategory = model.Id }).Categories;
            if ((!childCategories.Any()) && (!post.Any()))
            {
                var result = categoryService.DeleteCategory(new DeleteCategoryRequest { Id = model.Id });
            }
            else
            {
                if (post.Any())
                {
                    ModelState.AddModelError(string.Empty, "No se puede eliminar la categoría. Posee publicaciones asignadas.");
                }
                if (childCategories.Any())
                {
                    ModelState.AddModelError(string.Empty, "No se puede eliminar la categoría. Posee categorías hijas.");
                }
                return View(model);
            }

            return RedirectToAction("Index", "Category");
        }

        public ActionResult Details(int id)
        {
            var categoryService = new CategoryService();
            var category = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = id }).Category;

            if (category != null)
            {
                var model = new DetailsCategoryViewModel
                {
                    Title = category.Title,
                    Description = category.Description,
                    Summary = category.Summary,
                    IdUpperCategory = category.IdUpperCategory,
                    TitleUpperCategory = category.IdUpperCategory.HasValue ? categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = category.IdUpperCategory.Value }).Category.Title : string.Empty
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Category");
            }
        }


        public ActionResult List()
        {
            var categoryService = new CategoryService();

            var model = new ListCategoriesViewModel();

            model.Categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories.ToList();

            ViewBag.Message = "Listado de categorías disponibles";


            return View(model);
        }
    }
}