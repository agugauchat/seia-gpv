namespace Web.DotW.API.Controllers
{
    using DotW.API.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Net.Http;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using System.Threading.Tasks;
    using DotW.API.Models;
    using System.Security.Claims;
    using Services.UserServices;
    using Contracts.UserContracts.Request;
    using Entities.UserEntities;

    [RoutePrefix("api/account")]
    public class AccountsController : BaseApiController
    {
        //[Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();
        }

        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterModel registeUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = registeUserModel.UserName,
                Email = registeUserModel.Email,
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, registeUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }
            else
            {
                var userService = new UserService();

                var request = new CreateUserRequest
                {
                    User = new User
                    {
                        Name = user.UserName,
                        AspNetUserId = user.Id,
                        Email = user.Email
                    }
                };

                var response = userService.CreateUser(request);

                AppUserManager.AddToRole(user.Id, "User");
            }

            // TODO > Agregar email de confirmación y quitar "userService.CreateUser" de esta acción.

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [Authorize]
        [Route("prueba")]
        public async Task<IHttpActionResult> Prueba(Prueba2 model)
        {
            var x = model;

            return Ok("Funcionó");
        }
    }
}