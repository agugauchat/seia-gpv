namespace Contracts.CategoryContracts.Response
{
    using Entities.CategoryEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchCategoriesResponse
    {
        public List<Category> Categories { get; set; }
    }
}
