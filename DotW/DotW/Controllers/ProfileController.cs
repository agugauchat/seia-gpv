namespace DotW.Controllers
{
    using Contracts.UserContracts.Request;
    using Entities.UserEntities;
    using Microsoft.AspNet.Identity;
    using Models;
    using Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    [Authorize(Roles = "User")]
    public class ProfileController : BaseController
    {
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