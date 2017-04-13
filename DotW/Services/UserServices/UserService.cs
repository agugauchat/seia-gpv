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
    }
}
