namespace Contracts.PostContracts.Response
{
    using Entities.PostEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetPostByIdResponse
    {
        public Post Post { get; set; }
    }
}
