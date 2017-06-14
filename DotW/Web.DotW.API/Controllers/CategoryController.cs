namespace Web.DotW.API.Controllers
{
    using Contracts.CategoryContracts.Request;
    using DotW.API.Models;
    using Services.CategoryServices;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class CategoryController : BaseApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            var categoryService = new CategoryService();

            return Ok(categoryService.SearchCategories(new SearchCategoriesRequest()).Categories.ToList());
        }

        [AllowAnonymous]
        public IHttpActionResult Get(int id)
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

        [AllowAnonymous]
        public IHttpActionResult Put(int id)
        {
            return Ok();
        }

        [AllowAnonymous]
        public IHttpActionResult Post(int id)
        {
            return Ok();
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