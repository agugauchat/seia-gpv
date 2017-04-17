namespace Services.CategoryServices
{
    using Contracts.CategoryContracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.CategoryContracts.Request;
    using Contracts.CategoryContracts.Response;
    using Data;
    using Entities.CategoryEntities;

    public class CategoryService : ICategoryService
    {
        public CreateCategoryResponse CreateCategory(CreateCategoryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var category = new Categories
                {
                    Title = request.Title,
                    Description = request.Description,
                    Summary = request.Summary,
                    IdUpperCategory = request.IdUpperCategory
                };

                db.Categories.Add(category);

                db.SaveChanges();

                return new CreateCategoryResponse { CategoryId = category.Id };
            }
        }

        public SearchCategoriesByIdUpperCategoryResponse SearchCategoriesByIdUpperCategory(SearchCategoriesByIdUpperCategoryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = db.Categories.Where(x => !x.NullDate.HasValue && x.IdUpperCategory == request.IdUpperCategory)
                        .Select(x => new Category
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            Summary = x.Summary,
                            IdUpperCategory = x.IdUpperCategory
                        }).ToList();

                return new SearchCategoriesByIdUpperCategoryResponse { Categories = result };
            }
        }

        public SearchCategoriesResponse SearchCategories(SearchCategoriesRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = db.Categories.Where(x => !x.NullDate.HasValue)
                        .Select(x => new Category
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            Summary = x.Summary,
                            IdUpperCategory = x.IdUpperCategory
                        }).ToList();

                return new SearchCategoriesResponse { Categories = result };
            }
        }

        public GetCategoryByIdResponse GetCategoryById(GetCategoryByIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var response = new GetCategoryByIdResponse();

                response.Category = db.Categories.Select(x =>
                    new Category
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Summary = x.Summary,
                        IdUpperCategory = x.IdUpperCategory,
                        NullDate = x.NullDate
                }).FirstOrDefault(x => x.Id == request.Id && (!x.NullDate.HasValue));

                return response;
            }
        }

        public UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var Category = db.Categories.FirstOrDefault(x => x.Id == request.Id);

                if (Category != null)
                {
                    Category.Title = request.Title;
                    Category.Description = request.Description;
                    Category.Summary = request.Summary;
                    Category.IdUpperCategory = request.IdUpperCategory;

                    db.SaveChanges();
                }

                return new UpdateCategoryResponse();
            }
        }

        public DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest request)
        {
            using (var db = new DotWEntities())
            {
                var Category = db.Categories.FirstOrDefault(x => x.Id == request.Id);

                if (Category != null)
                {
                    Category.NullDate = DateTime.Now;

                    db.SaveChanges();
                }

                return new DeleteCategoryResponse();
            }
        }
    }
}
