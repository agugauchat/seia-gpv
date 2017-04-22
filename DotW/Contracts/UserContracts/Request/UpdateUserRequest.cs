namespace Contracts.UserContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UpdateUserRequest
    {
        public string AspNetUserId { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
