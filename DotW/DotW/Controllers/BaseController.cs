using Contracts.UserContracts.Request;
using DotW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Services.UserServices;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace DotW.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;

        public BaseController()
        {
        }

        public BaseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = User.Identity.GetUserId();
            List<string> roles = new List<string>();

            // Se pasarán estos elementos a todas las vistas de los controladores que hereden de BaseController.
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUser = UserManager.FindById(userId);
                roles.AddRange(UserManager.GetRoles(userId));

                if (!User.IsInRole(RoleTypes.Admin.ToString()))
                {
                    var userService = new UserService();

                    TempData["UserAccountSuspended"] = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;
                }
            }

            ViewBag.RolesOfCurrentUser = roles;

            base.OnActionExecuting(filterContext);
        }
    }
}