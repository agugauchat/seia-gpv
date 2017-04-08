namespace DotW.Controllers
{
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    public class UserController : BaseController
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var model = UserManager.Users.ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Roles = new SelectList(GetRoleList(), "RoleValue", "RoleName" );

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, model.Rol);

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    AddErrors(result);
                }
            }

            ViewBag.Roles = new SelectList(GetRoleList(), "RoleValue", "RoleName");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string token)
        {
            var user = UserManager.FindById(token);

            if (user != null)
            {
                var model = new DeleteViewModel
                {
                    Email = user.Email,
                    Id = user.Id,
                    Roles = TranslateRoles(UserManager.GetRoles(user.Id).ToList())
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(ApplicationUser model)
        {
            var user = UserManager.FindById(model.Id);

            UserManager.Delete(user);

            return RedirectToAction("Index", "User");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private List<UserRoles> GetRoleList()
        {
            var roles = new List<UserRoles>
            {
                new UserRoles
                {
                    RoleName = "Administrador",
                    RoleValue = RoleTypes.Admin.ToString()
                },
                new UserRoles
                {
                    RoleName = "Usuario",
                    RoleValue = RoleTypes.User.ToString()
                }
            };

            return roles;
        }

        private List<string> TranslateRoles(IList<string> rolesList)
        {
            var roles = new List<string>();

            foreach (var role in rolesList)
            {
                if (role == RoleTypes.Admin.ToString())
                {
                    roles.Add("Administrador");
                }

                if (role == RoleTypes.User.ToString())
                {
                    roles.Add("Usuario");
                }
            }

            return roles;
        }
    }
}