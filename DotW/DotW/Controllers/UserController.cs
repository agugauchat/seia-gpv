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

    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            var model = UserManager.Users.ToList().OrderBy(x => x.UserName);

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "User");

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View(model);
        }

        public ActionResult Edit(string token)
        {
            var user = UserManager.FindById(token);

            if (user != null)
            {
                var model = new EditViewModel
                {
                    Token = token,
                    UserName = user.UserName,
                    Email = user.Email,
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        [HttpPost]
        public ActionResult Edit(EditViewModel model)
        {
            var user = UserManager.FindById(model.Token);

            if (user != null)
            {
                // Verificar si los atributos Mail y Username se encuentran disponibles
                var auxUser = UserManager.FindByEmail(model.Email);
                if (auxUser != null && auxUser.Id != model.Token)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario con ese email.");
                    return View(model);
                }
                auxUser = UserManager.FindByName(model.UserName);
                if (auxUser != null && auxUser.Id != model.Token)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario con ese nombre.");
                    return View(model);
                }

                // Actualizar el usuario con los atributos del modelo
                user.Email = model.Email;
                if (user.UserName != "admin")
                {
                    user.UserName = model.UserName;
                }    

                // Persistir los cambios en la BD
                UserManager.Update(user);

                return RedirectToAction("Index", "User");
            }

            return View(model);
        }

        public ActionResult Details(string token)
        {
            var user = UserManager.FindById(token);

            if (user != null)
            {
                var model = new DetailsViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = TranslateRoles(UserManager.GetRoles(user.Id).ToList())
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string token)
        {
            var user = UserManager.FindById(token);

            if (user != null)
            {
                var model = new DeleteViewModel
                {
                    UserName = user.UserName,
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
        public ActionResult Delete(ApplicationUser model)
        {
            var user = UserManager.FindById(model.Id);

            if (user.UserName != "admin")
            {
                UserManager.Delete(user);
            }

            return RedirectToAction("Index", "User");
        }

        #region Private Methods

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

        #endregion
    }
}