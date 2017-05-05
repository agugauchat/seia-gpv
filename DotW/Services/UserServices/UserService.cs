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
                    IdState = (int)UserAccountStates.Active,
                    ActivationDate = (DateTime?)null
                };

                db.Users.Add(user);

                db.SaveChanges();

                return new CreateUserResponse();
            }
        }

        public GetUserByIdResponse GetUserById(GetUserByIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = db.Users.FirstOrDefault(x => x.Id == request.UserId);

                var response = new GetUserByIdResponse();

                if (result != null)
                {
                    response.User = new User
                    {
                        Id = result.Id,
                        Name = result.Name,
                        AspNetUserId = result.AspNetUserId,
                        EffectDate = result.EffectDate,
                        IdState = result.IdState,
                        ActivationDate = result.ActivationDate,
                        Email = result.AspNetUsers.Email
                    };
                }

                return response;
            }

            throw new NotImplementedException();
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
                        ActivationDate = result.ActivationDate,
                        Email = result.AspNetUsers.Email
                    };
                }

                return response;
            }
        }

        public GetUserByUsernameResponse GetUserByUsername(GetUserByUsernameRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = db.Users.FirstOrDefault(x => x.Name == request.Username);

                var response = new GetUserByUsernameResponse();

                if (result != null)
                {
                    response.User = new User
                    {
                        Id = result.Id,
                        Name = result.Name,
                        AspNetUserId = result.AspNetUserId,
                        EffectDate = result.EffectDate,
                        IdState = result.IdState,
                        ActivationDate = result.ActivationDate,
                        Email = result.AspNetUsers.Email
                    };
                }

                return response;
            }
        }

        public UpdateUserResponse UpdateUser(UpdateUserRequest request)
        {
            using (var db = new DotWEntities())
            {
                if (!string.IsNullOrEmpty(request.AspNetUserId))
                {
                    var user = db.Users.FirstOrDefault(x => x.AspNetUserId == request.AspNetUserId);

                    if (user != null)
                    {
                        user.Name = request.Name;

                        db.SaveChanges();
                    }
                }
                else if (request.Id.HasValue)
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == request.Id);

                    if (user != null)
                    {
                        user.Name = request.Name;

                        db.SaveChanges();
                    }
                }

                return new UpdateUserResponse();
            }
        }

        public DeleteUserResponse DeleteUser(DeleteUserRequest request)
        {
            using (var db = new DotWEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.AspNetUserId == request.AspNetUserId);

                if (user != null)
                {
                    user.AspNetUserId = null;
                    user.NullDate = DateTime.Now;

                    db.SaveChanges();
                }

                return new DeleteUserResponse();
            }
        }
    }
}
