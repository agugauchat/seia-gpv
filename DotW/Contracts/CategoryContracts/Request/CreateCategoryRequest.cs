namespace Contracts.CategoryContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateCategoryRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int? IdUpperCategory { get; set; }
    }
}
