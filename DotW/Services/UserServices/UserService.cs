namespace Services.UserServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.UserContracts.Request;
    using Contracts.UserContracts.Response;
    using Data;
    using Contracts.UserContracts;
    using Entities.UserEntities;

    public class UserService : IUserService
    {
        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            using (var db = new DotWEntities())
            {
                var user = new Users
                {
                    Name = request.User.Name,
                    AspNetUserId = request.User.AspNetUserId,
                    EffectDate = DateTime.Now,
                    IdState = 1,
                    SuspendedDate = (DateTime?)null
                };

                db.Users.Add(user);

                db.SaveChanges();

                return new CreateUserResponse();
            }
        }

        public GetUserByAccountIdResponse GetUserByAccountId(GetUserByAccountIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = db.Users.FirstOrDefault(x => x.AspNetUserId == request.AccountId);

                var response = new GetUserByAccountIdResponse();

                if (result != null)
                {
                    response.User = new User
                    {
                        Id = result.Id,
                        Name = result.Name,
                        AspNetUserId = result.AspNetUserId,
                        EffectDate = result.EffectDate,
                        IdState = result.IdState,
                        SuspendedDate = result.SuspendedDate
                    };
                }

                return response;
            }
        }
    }
}
