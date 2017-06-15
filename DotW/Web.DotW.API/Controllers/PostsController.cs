using Contracts.ComplaintContracts.Request;
using Contracts.PostContracts.Request;
using Contracts.UserContracts.Request;
using Entities.General;
using Entities.PostEntities;
using Microsoft.AspNet.Identity;
using Services.ComplaintServices;
using Services.PostServices;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Web.DotW.API.Helpers;
using Web.DotW.API.Models;

namespace Web.DotW.API.Controllers
{
    public class PostsController : BaseApiController
    {
        // GET: api/Posts
        [AllowAnonymous]
        public IHttpActionResult GetPosts()
        {
            var postService = new PostService();
            var posts = postService.SearchPosts(new SearchPostsRequest()).Posts;

            return Ok(posts);
        }

        // GET: api/Posts/5
        [AllowAnonymous]
        [ResponseType(typeof(Post))]
        public IHttpActionResult GetPost(int id)
        {
            var postService = new PostService();
            var post = postService.GetPostById(new GetPostByIdRequest() { Id = id }).Post;

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/Posts/5
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPost(EditPostModel model)
        {
            try
            {
                var userService = new UserService();
                var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

                if (userAccountSuspended)
                {
                    return BadRequest("User account suspended");
                }

                var postService = new PostService();
                var complaintService = new ComplaintService();
                string finalFileName = string.Empty;

                var postComplaints = complaintService.SearchComplaintsByPostId(new SearchComplaintsByPostIdRequest { PostId = model.Id }).Complaints;

                if (postComplaints.Any())
                {
                    return BadRequest("User account suspended");
                }
                else if (model.File != null && !model.DeleteImage)
                {
                    //if ((new AllowedExtensions()).ImageExtensions.Contains(Path.GetExtension(model.File.FileName).Remove(0, 1).ToLower()))
                    //{
                    //    var pathPostPrincipalImages = ConfigurationManager.AppSettings["PathPostPrincipalImages"];

                    //    var newImage = new WebImage(model.File.InputStream);
                    //    finalFileName = model.Id.ToString() + Path.GetExtension(model.File.FileName);
                    //    var directory = Server.MapPath(pathPostPrincipalImages);

                    //    // Se crea el directorio; si ya existe el directorio, la función no hace nada.
                    //    Directory.CreateDirectory(directory);
                    //    var finalpath = directory + finalFileName;

                    //    newImage.Save(finalpath);
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", "La extensión de la imagen no es válida, vuelva a cargarla.");
                    //}
                }

                if (ModelState.IsValid)
                {
                    var post = postService.GetPostById(new GetPostByIdRequest() { Id = model.Id }).Post;

                    if (post == null)
                    {
                        return NotFound();
                    }

                    var result = postService.UpdatePost(new UpdatePostRequest
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Summary = model.Summary,
                        Body = model.Body,
                        IdCategory = model.IdCategory,
                        IsDraft = model.IsDraft,
                        PrincipalImageName = string.IsNullOrEmpty(finalFileName) && !model.DeleteImage ? model.PrincipalImageName : finalFileName,
                        Tags = model.Tags
                    });

                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Posts
        //[Authorize(Roles = "User")]
        [ResponseType(typeof(Post))]
        public IHttpActionResult PostPost(CreatePostModel model)
        {
            var data = model.base64;
            string[] tokens = data.Split(',');
            var extension = tokens[0].Split('/')[1].Split(';')[0];
            var fileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), extension);
            var folder = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PathPostPrincipalImages"]);
            string path = Path.Combine(folder, fileName);

            var stream = ImageHelpers.Base64ToStream(tokens[1], path);

            var image = new Bitmap(stream);
            image.Save(path);

            //UploadAndCompressImage(path, stream, path.Split('.').Last());


            //try
            //{
            //    var userService = new UserService();
            //    var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

            //    if (userAccountSuspended)
            //    {
            //        return BadRequest("User account suspended");
            //    }

            //    var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

            //    if (user != null)
            //    {
            //        model.IdWriter = user.Id;
            //    }

            //    if (ModelState.IsValid)
            //    {

            //        var postService = new PostService();

            //        string finalFileName = string.Empty;

            //        // Verifica si el modelo no tiene imagen, o tiene imagen y la misma tiene una extensión permitida.
            //        if (model.File == null ||
            //            (model.File != null &&
            //            (new AllowedExtensions()).ImageExtensions.Contains(Path.GetExtension(model.File.FileName).Remove(0, 1).ToLower()))
            //           )
            //        {
            //            var request = new CreatePostRequest
            //            {
            //                IdWriter = model.IdWriter,
            //                Title = model.Title,
            //                Summary = model.Summary,
            //                Body = model.Body,
            //                CategoryId = model.IdCategory,
            //                IsDraft = model.IsDraft,
            //                Tags = model.Tags
            //            };

            //            var createResult = postService.CreatePost(request);

            //            if (model.File != null)
            //            {
            //                //var pathPostPrincipalImages = ConfigurationManager.AppSettings["PathPostPrincipalImages"];
            //                //var newImage = new WebImage(model.File.InputStream);
            //                //finalFileName = createResult.PostId.ToString() + Path.GetExtension(model.File.FileName);
            //                //var directory = Server.MapPath(pathPostPrincipalImages);

            //                //// Se crea el directorio; si ya existe el directorio, la función no hace nada.
            //                //Directory.CreateDirectory(directory);
            //                //var finalpath = directory + finalFileName;

            //                //newImage.Save(finalpath);

            //                //var updateResult = postService.UpdatePost(new UpdatePostRequest
            //                //{
            //                //    Id = createResult.PostId,
            //                //    Title = model.Title,
            //                //    Summary = model.Summary,
            //                //    Body = model.Body,
            //                //    IdCategory = model.IdCategory,
            //                //    IsDraft = model.IsDraft,
            //                //    PrincipalImageName = finalFileName,
            //                //    Tags = model.Tags
            //                //});
            //            }

            //            return Ok();
            //        }
            //    }
            //    return BadRequest(ModelState);
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}

            return Ok();
        }

        // DELETE: api/Posts/5
        [Authorize(Roles = "User")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeletePost(int id)
        {
            var userService = new UserService();
            var userAccountSuspended = userService.VerifyIfIsSuspendedAndUpdateUser(new VerifyIfIsSuspendedAndUpdateUserRequest { AspNetUserId = User.Identity.GetUserId() }).UserSuspended;

            if (userAccountSuspended)
            {
                return BadRequest("User account suspended");
            }

            var postService = new PostService();
            var post = postService.GetPostById(new GetPostByIdRequest() { Id = id }).Post;

            if (post == null)
            {
                return NotFound();
            }

            var result = postService.DeletePost(new DeletePostRequest { Id = id, IsComplaintOrVoteDifference = false });

            return Ok();
        }
    }
}
