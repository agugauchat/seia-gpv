using System;
using System.Collections.Generic;
using System.Linq;
namespace DotW.Controllers
{
    using Contracts.ComplaintContracts.Request;
    using Contracts.UserContracts.Request;
    using DotW.Models;
    using Microsoft.AspNet.Identity;
    using Services.ComplaintServices;
    using Services.UserServices;
    using System.Web;
    using System.Web.Mvc;

    public class ComplaintController : BaseController
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult PostComplaint(ComplaintPostViewModel model)
        {
            var complaintService = new ComplaintService();
            var userService = new UserService();

            try
            {
                var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

                if (user != null)
                {
                    var userComplaints = complaintService.SearchComplaintsByUserId(new SearchComplaintsByUserIdRequest { UserId = user.Id }).Complaints;

                    if (userComplaints.Any(x => x.IdPost == model.PostId))
                    {
                        return Json(new { success = false, Message = "Ya se ha registrado una denuncia para esta cuenta en esta publicación." }, JsonRequestBehavior.AllowGet);
                    }

                    var result = complaintService.CreatePostComplaint(new CreatePostComplaintRequest { PostId = model.PostId, UserId = user.Id, Commentary = model.Commentary });

                    return Json(new { success = true, Message = "Su denuncia ha sido registrada. Gracias por contribuir con nuestra comunidad :)" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, Message = "Ha ocurrido un error al procesar la solicitud." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, Message = "Ha ocurrido un error al procesar la solicitud." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}