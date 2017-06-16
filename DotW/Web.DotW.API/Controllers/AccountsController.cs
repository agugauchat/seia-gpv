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
    }
}