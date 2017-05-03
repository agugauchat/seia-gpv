namespace DotW.Controllers
{
    using Contracts.CategoryContracts.Request;
    using Contracts.CommentaryContracts.Request;
    using Contracts.ComplaintContracts.Request;
    using Contracts.PostContracts.Request;
    using Contracts.UserContracts.Request;
    using Entities.ComplaintEntities;
    using Entities.General;
    using Microsoft.AspNet.Identity;
    using Models;
    using Services.CategoryServices;
    using Services.CommentaryServices;
    using Services.ComplaintServices;
    using Services.PostServices;
    using Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;

    public class PostController : BaseController
    {
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            var postService = new PostService();

            var model = new IndexPostViewModel();

            model.Posts = postService.SearchPostsByUserId(new SearchPostsByUserIdRequest { AspNetUserId = User.Identity.GetUserId() }).Posts.OrderByDescending(x => x.EffectDate).ToList();

            return View(model);
        }

        public ActionResult List(int? idCategory, string tag)
        {
            var postService = new PostService();
            var categoryService = new CategoryService();

            var model = new ListPostViewModel();

            if (idCategory.HasValue)
            {
                model.Posts = postService.SearchPostsByCategoryId(new SearchPostsByCategoryIdRequest { IdCategory = idCategory.Value }).Posts.Where(x => !x.IsDraft).OrderByDescending(x => x.EffectDate).ToList();
            }
            else if (!string.IsNullOrEmpty(tag))
            {
                model.Posts = postService.SearchPostsByTag(new SearchPostsByTagRequest { Tag = tag }).Posts.Where(x => !x.IsDraft).OrderByDescending(x => x.EffectDate).ToList();
            }
            else
            {
                model.Posts = postService.SearchPosts(new SearchPostsRequest()).Posts.Where(x => !x.IsDraft).OrderByDescending(x => x.EffectDate).ToList();
            }

            ViewBag.Categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;

            return View(model);
        }

        [Authorize(Roles = "User")]
        public ActionResult Create()
        {
            var categoryService = new CategoryService();

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePostViewModel model)
        {
            var userService = new UserService();
            var postService = new PostService();
            var categoryService = new CategoryService();
            string finalFileName = string.Empty;

            //TODO > Más adelante hay que validar el estado del usuario antes de permitir publicar.

            var user = userService.GetUserByAccountId(new GetUserByAccountIdRequest { AccountId = User.Identity.GetUserId() }).User;

            if (user != null)
            {
                model.IdWriter = user.Id;
            }
            
            if (ModelState.IsValid)
            {
                // Verifica si el modelo no tiene imagen, o tiene imagen y la misma tiene una extensión permitida.
                if ( model.File == null ||
                    (model.File != null &&
                    (new AllowedExtensions()).ImageExtensions.Contains(Path.GetExtension(model.File.FileName).Remove(0, 1).ToLower()))
                   )
                {
                var request = new CreatePostRequest
                {
                    IdWriter = model.IdWriter,
                    Title = model.Title,
                    Summary = model.Summary,
                    Body = model.Body,
                    CategoryId = model.IdCategory,
                        IsDraft = model.IsDraft,
                        Tags = model.Tags
                };

                    var createResult = postService.CreatePost(request);

                    if (model.File != null)
                    {
                        var pathPostPrincipalImages = ConfigurationManager.AppSettings["PathPostPrincipalImages"];
                        var newImage = new WebImage(model.File.InputStream);
                        finalFileName = createResult.PostId.ToString() + Path.GetExtension(model.File.FileName);
                        var directory = Server.MapPath(pathPostPrincipalImages);

                        // Se crea el directorio; si ya existe el directorio, la función no hace nada.
                        Directory.CreateDirectory(directory);
                        var finalpath = directory + finalFileName;

                        newImage.Save(finalpath);

                        var updateResult = postService.UpdatePost(new UpdatePostRequest
                        {
                            Id = createResult.PostId,
                            Title = model.Title,
                            Summary = model.Summary,
                            Body = model.Body,
                            IdCategory = model.IdCategory,
                            IsDraft = model.IsDraft,
                            PrincipalImageName = finalFileName,
                            Tags = model.Tags
                        });
                    }

                return RedirectToAction("Index", "Post");
            }
                else
                {
                    ModelState.AddModelError("", "La extensión de la imagen no es válida, vuelva a cargarla.");
                }
            }

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [Authorize(Roles = "User")]
        public ActionResult Edit(int id)
        {
            var postService = new PostService();
            var categoryService = new CategoryService();

            var result = postService.GetPostById(new GetPostByIdRequest { Id = id }).Post;

            var model = new EditPostViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Summary = result.Summary,
                Body = result.Body,
                IdCategory = result.IdCategory,
                IsDraft = result.IsDraft,
                PrincipalImageName = result.PrincipalImageName,
                Tags = result.Tags
            };

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPostViewModel model)
        {
            var postService = new PostService();
            var categoryService = new CategoryService();
            string finalFileName = string.Empty;

            if (model.File != null)
            {
                if ((new AllowedExtensions()).ImageExtensions.Contains(Path.GetExtension(model.File.FileName).Remove(0, 1).ToLower()))
                {
                    var pathPostPrincipalImages = ConfigurationManager.AppSettings["PathPostPrincipalImages"];

                    var newImage = new WebImage(model.File.InputStream);
                    finalFileName = model.Id.ToString() + Path.GetExtension(model.File.FileName);
                    var directory = Server.MapPath(pathPostPrincipalImages);

                    // Se crea el directorio; si ya existe el directorio, la función no hace nada.
                    Directory.CreateDirectory(directory);
                    var finalpath = directory + finalFileName;

                    newImage.Save(finalpath);
                }
                else
                {
                    ModelState.AddModelError("", "La extensión de la imagen no es válida, vuelva a cargarla.");
                }
            }

            if (ModelState.IsValid)
            {
                var result = postService.UpdatePost(new UpdatePostRequest
                {
                    Id = model.Id,
                    Title = model.Title,
                    Summary = model.Summary,
                    Body = model.Body,
                    IdCategory = model.IdCategory,
                    IsDraft = model.IsDraft,
                    PrincipalImageName = string.IsNullOrEmpty(finalFileName) ? model.PrincipalImageName : finalFileName,
                    Tags = model.Tags
                });

                return RedirectToAction("Index", "Post");
            }

            var categories = categoryService.SearchCategories(new SearchCategoriesRequest()).Categories;
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [Authorize(Roles = "User")]
        public ActionResult Delete(int id)
        {
            var postService = new PostService();

            var result = postService.GetPostById(new GetPostByIdRequest { Id = id }).Post;

            var model = new DeletePostViewModel
            {
                Id = result.Id,
                Title = result.Title,
                Summary = result.Summary,
                CategoryTitle = result.CategoryTitle,
                IsDraft = result.IsDraft,
                Tags = result.Tags
            };

            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult Delete(DeletePostViewModel model)
        {
            var postService = new PostService();

            var result = postService.DeletePost(new DeletePostRequest { Id = model.Id });

            return RedirectToAction("Index", "Post");
        }

        [Authorize(Roles = "User")]
        public ActionResult UploadImagePartial()
        {
            // Se obtiene el path de imagenes de Posts.
            var pathPostImages = ConfigurationManager.AppSettings["PathPostImages"];

            // Se obtiene el nombre del usuario actual para acceder al directorio de imagenes de posts de ese usuario.
            var currentUserName = User.Identity.GetUserName();
            var directory = pathPostImages + currentUserName;

            // Se crea el directorio; si ya existe el directorio, la función no hace nada.
            Directory.CreateDirectory(Server.MapPath(directory));

            // Se mapea el path relativo para obtener el path físico (real).
            var appData = Server.MapPath(pathPostImages + currentUserName);

            var images = Directory.GetFiles(appData).Select(x => new ImageViewModel
            {
                Url = Url.Content(directory + "/" + Path.GetFileName(x))
            });
            return View(images);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                string ImageName = upload.FileName;

                // Se obtiene el path de imagenes de Posts.
                var pathPostImages = ConfigurationManager.AppSettings["PathPostImages"];

                // Se obtiene el nombre del usuario actual para acceder al directorio de imagenes de posts de ese usuario.
                var currentUserName = User.Identity.GetUserName();
                var directory = pathPostImages + currentUserName + "\\";

                // Se crea el directorio; si ya existe el directorio, la función no hace nada.
                directory = Server.MapPath(directory);
                Directory.CreateDirectory(directory);

                // Se obtiene el path final de la imagen.
                string path = System.IO.Path.Combine(directory, ImageName);

                // Se guarda la imagen.
                upload.SaveAs(path);

                var result = new
                {
                    Resultado = "imagen enviada correctamente."
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            var postService = new PostService();
            var commentaryService = new CommentaryService();

            var post = postService.GetPostById(new GetPostByIdRequest() { Id = id }).Post;
            ViewBag.Comments = commentaryService.SearchCommentsByIdPost(new SearchCommentsByIdPostRequest() { IdPost = id }).Comments.Where(x => !x.NullDate.HasValue).ToList();

            List<Complaint> userComplaints = new List<Complaint>();

            if (User.Identity.IsAuthenticated)
            {
                var complaintService = new ComplaintService();

                userComplaints = complaintService.SearchComplaintsByUserId(new SearchComplaintsByUserIdRequest { AspNetUserId = User.Identity.GetUserId() }).Complaints;
            }

            ViewBag.UserComplaints = userComplaints;

            return View(post);
        }
    }
}