namespace Contracts.PostContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UpdatePostRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Body { get; set; }

        public int IdCategory { get; set; }

        public bool IsDraft { get; set; }

        public string PrincipalImageName { get; set; }

        public List<string> Tags { get; set; }
    }
}
