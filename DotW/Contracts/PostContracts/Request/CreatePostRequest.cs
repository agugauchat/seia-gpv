namespace Contracts.PostContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreatePostRequest
    {
        public string Title { get; set; }

        public int IdWriter { get; set; }

        public string Summary { get; set; }

        public string Body { get; set; }

        public int CategoryId { get; set; }

        public bool IsDraft { get; set; }
    }
}
