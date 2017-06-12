namespace Web.DotW.API.Controllers
{
    using DotW.API.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Net.Http;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using System.Threading.Tasks;
    using DotW.API.Models;
    using System.Security.Claims;
    using Services.UserServices;
    using Contracts.UserContracts.Request;
    using Entities.UserEntities;
    using Services.CategoryServices;
    using Contracts.CategoryContracts.Request;

    public class CategoryController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> List()
        {
            var categoryService = new CategoryService();

            return Ok(categoryService.SearchCategories(new SearchCategoriesRequest()).Categories.ToList());
        }

        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            try
            {
                var categoryService = new CategoryService();
                var category = categoryService.GetCategoryById(new GetCategoryByIdRequest() { Id = id}).Category;
                if ((category != null) && (!category.NullDate.HasValue))
                {
                    return Ok(category);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(CreateCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var categoryService = new CategoryService();
                var request = new CreateCategoryRequest
                {
                    Title = model.Title,
                    Summary = model.Summary,
                    Description = model.Description,
                    IdUpperCategory = model.IdUpperCategory
                };

                var result = categoryService.CreateCategory(request);

                return Ok(ModelState);

                // TODO Agu -> Ver que hace esto e implementarlo correctamente.
                //Uri locationHeader = new Uri(Url.Link("Details", new { id = result.CategoryId }));
                //return Created(locationHeader, TheModelFactory.Create(categoryService));
            }
            return BadRequest(ModelState);
        }

        // TODO Agu -> Hacer edit y delete para terminar el controller.
    }
}