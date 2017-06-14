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
        public IHttpActionResult GetCategories()
        {
            var categoryService = new CategoryService();
            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;

            // TODO Agu -> Preguntar como devolverlo.
            return Ok(categories);
        }

        // GET: api/Categories/5
        [AllowAnonymous]
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            var categoryService = new CategoryService();
            var category = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = id }).Category;

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
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
        [ResponseType(typeof(Category))]
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