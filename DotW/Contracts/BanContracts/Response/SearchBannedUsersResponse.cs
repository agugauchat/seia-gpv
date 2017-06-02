namespace Contracts.BanContracts.Response
{
    using Entities.UserEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchBannedUsersResponse
    {
        public List<User> Users { get; set; }
    }
}
