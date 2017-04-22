namespace Contracts.UserContracts
{
    using Contracts.UserContracts.Request;
    using Contracts.UserContracts.Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUserService
    {
        CreateUserResponse CreateUser(CreateUserRequest request);

        GetUserByAccountIdResponse GetUserByAccountId(GetUserByAccountIdRequest request);

        UpdateUserResponse UpdateUser(UpdateUserRequest request);

        DeleteUserResponse DeleteUser(DeleteUserRequest request);
    }
}
