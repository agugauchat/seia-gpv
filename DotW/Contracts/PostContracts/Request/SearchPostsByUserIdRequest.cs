using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.PostContracts.Request
{
    public class SearchPostsByUserIdRequest
    {
        public int? UserId { get; set; }

        public string AspNetUserId { get; set; }
    }
}
