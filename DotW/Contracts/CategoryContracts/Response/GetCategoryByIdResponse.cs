﻿namespace Contracts.CategoryContracts.Response
{
    using Entities.CategoryEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetCategoryByIdResponse
    {
        public Category Category { get; set; }
    }
}
