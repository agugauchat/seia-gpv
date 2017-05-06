namespace Contracts.UserContracts.Response
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class VerifyAndUpdateUserStateByCommentsResponse
    {
        public bool UserSuspended { get; set; }

        public DateTime ActivationDate { get; set; }
    }
}
