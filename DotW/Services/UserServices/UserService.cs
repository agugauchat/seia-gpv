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

        public VerifyAndUpdateUserStateByPostsResponse VerifyAndUpdateUserStateByPosts(VerifyAndUpdateUserStateByPostsRequest request)
        {
            using (var db = new DotWEntities())
            {
                Users user;

                if (request.UserId.HasValue)
                {
                    user = db.Users.FirstOrDefault(x => x.Id == request.UserId.Value);
                }
                else
                {
                    user = db.Users.FirstOrDefault(x => x.AspNetUserId == request.AspNetUserId);
                }

                var response = new VerifyAndUpdateUserStateByPostsResponse();

                if (user != null)
                {
                    // Se verifica si la cantidad de posts bloqueados es mayor a 0 porque la operación
                    // 0 % PostBlockedDivider da como resultado 0 y se podría bloquear un usuario cuando en realidad no corresponde.
                    if (user.BlockedPublications > 0)
                    {
                        decimal divisionMod = user.BlockedPublications % (int)DividersToBlockUser.PostBlockedDivider;

                        if (divisionMod == 0)
                        {
                            // Se debe suspender al usuario.
                            user.IdState = db.UserStates.Where(x => x.State == UserAccountStates.Suspended.ToString()).Select(x => x.Id).FirstOrDefault();

                            DateTime previousActivationDate;

                            if (user.ActivationDate.HasValue)
                            {
                                previousActivationDate = user.ActivationDate.Value;
                            }
                            else
                            {
                                previousActivationDate = DateTime.Now;
                            }

                            user.ActivationDate = previousActivationDate.AddDays((int)SuspendDays.SuspendedByPosts);

                            db.SaveChanges();

                            response.UserSuspended = true;
                            response.ActivationDate = user.ActivationDate.Value;
                        }
                    }
                }

                return response;
            }
        }

        public VerifyAndUpdateUserStateByCommentsResponse VerifyAndUpdateUserStateByComments(VerifyAndUpdateUserStateByCommentsRequest request)
        {
            using (var db = new DotWEntities())
            {
                Users user;

                if (request.UserId.HasValue)
                {
                    user = db.Users.FirstOrDefault(x => x.Id == request.UserId.Value);
                }
                else
                {
                    user = db.Users.FirstOrDefault(x => x.AspNetUserId == request.AspNetUserId);
                }

                var response = new VerifyAndUpdateUserStateByCommentsResponse();

                if (user != null)
                {
                    // Se verifica si la cantidad de comentarios bloqueados es mayor a 0 porque la operación
                    // 0 % CommentaryBlockedDivider da como resultado 0 y se podría bloquear un usuario cuando en realidad no corresponde.
                    if (user.BlockedComments > 0)
                    {
                        decimal divisionMod = user.BlockedComments % (int)DividersToBlockUser.CommentaryBlockedDivider;

                        if (divisionMod == 0)
                        {
                            // Se debe suspender al usuario.
                            user.IdState = db.UserStates.Where(x => x.State == UserAccountStates.Suspended.ToString()).Select(x => x.Id).FirstOrDefault();

                            DateTime previousActivationDate;

                            if (user.ActivationDate.HasValue)
                            {
                                previousActivationDate = user.ActivationDate.Value;
                            }
                            else
                            {
                                previousActivationDate = DateTime.Now;
                            }

                            user.ActivationDate = previousActivationDate.AddDays((int)SuspendDays.SuspendedByComments);

                            db.SaveChanges();

                            response.UserSuspended = true;
                            response.ActivationDate = user.ActivationDate.Value;
                        }
                    }
                }

                return response;
            }
        }
    }
}
