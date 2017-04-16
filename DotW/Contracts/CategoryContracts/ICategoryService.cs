namespace Contracts.CategoryContracts
{
    using Request;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        CreateCategoryResponse CreateCategory(CreateCategoryRequest request);

        SearchCategoriesByIdUpperCategoryResponse SearchCategoriesByIdUpperCategory(SearchCategoriesByIdUpperCategoryRequest request);

        SearchCategoriesResponse SearchCategories(SearchCategoriesRequest request);

        GetCategoryByIdResponse GetCategoryById(GetCategoryByIdRequest request);

        UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request);

        DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest request);
    }
}
