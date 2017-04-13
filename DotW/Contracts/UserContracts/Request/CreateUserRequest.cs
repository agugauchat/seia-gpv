namespace Contracts.UserContracts.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Entities.UserEntities;

    public class CreateUserRequest
    {
        public User User { get; set; }
    }
}
