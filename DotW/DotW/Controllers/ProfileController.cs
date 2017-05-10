namespace DotW.Controllers
{
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Entities.UserEntities;
    using Microsoft.AspNet.Identity;
    using Models;
    using Services.PostServices;
    using Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    public class ProfileController : BaseController
    {
        [Authorize]
        public ActionResult Details(int id)
        {
            var userService = new UserService();
            var postService = new PostService();
            var user = userService.GetUserById(new GetUserByIdRequest() { UserId = id }).User;

            if (user != null)
            {
                var posts = postService.SearchPostsByUserId(new SearchPostsByUserIdRequest() { UserId = id }).Posts;

                ViewBag.Posts = posts;

                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        public ActionResult Edit()
        {
            var userService = new UserService();
            var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User;

            if (user != null)
            {
                var model = new EditProfileViewModel
                {
                    Phone = user.Phone,
                    FullName = user.FullName,
                    Description = user.Description,
                    ShowData = user.ShowData
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult Edit(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userService = new UserService();
                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest() { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    var request = new UpdateProfileRequest
                    {
                        Id = user.Id,
                        Description = model.Description,
                        Phone = model.Phone,
                        FullName = model.FullName,
                        ShowData = model.ShowData
                    };

                    var response = userService.UpdateProfile(request);

                    return RedirectToAction("Index", "Manage");
                }
            }

            return View(model);
        }
    }
}