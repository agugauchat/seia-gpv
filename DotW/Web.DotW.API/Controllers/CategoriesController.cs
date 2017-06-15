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
using Entities.CategoryEntities;
using Web.DotW.API.Infrastructure;
using Services.CategoryServices;
using Contracts.CategoryContracts.Request;
using Web.DotW.API.Models;
using Services.PostServices;
using Contracts.PostContracts.Request;

namespace Web.DotW.API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        // GET: api/Categories
        [AllowAnonymous]
        [ResponseType(typeof(List<GetCategoryModel>))]
        public IHttpActionResult GetCategories()
        {
            var categoryService = new CategoryService();
            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;

            var result = new List<GetCategoryModel>();

            foreach (var category in categories)
            {
                var partialResult = new GetCategoryModel()
                {
                    Id = category.Id,
                    Title = category.Title,
                    Summary = category.Summary,
                    Description = category.Description,
                };

                if (category.IdUpperCategory.HasValue)
                {
                    var upperCategory = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = category.IdUpperCategory.Value }).Category;

                    if (upperCategory != null)
                    {
                        partialResult.UpperCategory = new CategoryModel()
                        {
                            Id = upperCategory.Id,
                            Title = upperCategory.Title,
                            Summary = upperCategory.Summary,
                            Description = upperCategory.Description,
                            IdUpperCategory = upperCategory.IdUpperCategory
                        };
                    }
                }

                result.Add(partialResult);
            }

            return Ok(result);
        }

        // GET: api/Categories/5
        [AllowAnonymous]
        [ResponseType(typeof(GetCategoryModel))]
        public IHttpActionResult GetCategory(int id)
        {
            var categoryService = new CategoryService();
            var category = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = id }).Category;

            if (category == null)
            {
                return NotFound();
            }

            var result = new GetCategoryModel()
            {
                Id = category.Id,
                Title = category.Title,
                Summary = category.Summary,
                Description = category.Description,
            };

            if (category.IdUpperCategory.HasValue)
            {
                var upperCategory = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = category.IdUpperCategory.Value }).Category;

                if (upperCategory != null)
                {
                    result.UpperCategory = new CategoryModel()
                    {
                        Id = upperCategory.Id,
                        Title = upperCategory.Title,
                        Summary = upperCategory.Summary,
                        Description = upperCategory.Description,
                        IdUpperCategory = upperCategory.IdUpperCategory
                    };
                }
            }

            return Ok(result);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PutCategory(EditCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoryService = new CategoryService();
                    var category = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = model.Id }).Category;

                    if (category == null)
                    {
                        return NotFound();
                    }

                    if (model.IdUpperCategory.HasValue)
                    {
                        // Controla que exista la categoría padre.
                        var upperCategory = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = model.IdUpperCategory.Value }).Category;
                        if ((upperCategory == null) || (model.Id == model.IdUpperCategory))
                        {
                            return BadRequest("Invalid upper category");
                        }
                    }

                    var request = new UpdateCategoryRequest
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Summary = model.Summary,
                        Description = model.Description,
                        IdUpperCategory = model.IdUpperCategory
                    };

                    var result = categoryService.UpdateCategory(request);

                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Categories
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostCategory(CreateCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoryService = new CategoryService();

                    if (model.IdUpperCategory.HasValue)
                    {
                        // Controla que exista la categoría padre.
                        var upperCategory = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = model.IdUpperCategory.Value }).Category;
                        if (upperCategory != null)
                        {
                            return BadRequest("Invalid upper category");
                        }
                    }

                    var request = new CreateCategoryRequest
                    {
                        Title = model.Title,
                        Summary = model.Summary,
                        Description = model.Description,
                        IdUpperCategory = model.IdUpperCategory
                    };

                    var result = categoryService.CreateCategory(request);

                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Categories/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteCategory(int id)
        {
            var categoryService = new CategoryService();
            var postService = new PostService();
            var category = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = id }).Category;

            if (category == null)
            {
                return NotFound();
            }

            var lowerCategories = categoryService.SearchCategoriesByIdUpperCategory(new SearchCategoriesByIdUpperCategoryRequest() { IdUpperCategory = id }).Categories;
            var relatedPosts = postService.SearchPostsByCategoryId(new SearchPostsByCategoryIdRequest() { IdCategory = id }).Posts;

            // Si existen categorías hijas o publicaciones asociadas, la categoría no se puede eliminar.
            if ((lowerCategories.Any()) || (relatedPosts.Any()))
            {
                return BadRequest("Can't delete this category, is in use.");
            }

            // La categoría existe y no fue eliminada, entonces la elimino.
            var result = categoryService.DeleteCategory(new DeleteCategoryRequest() { Id = id });

            return Ok();
        }
    }
}