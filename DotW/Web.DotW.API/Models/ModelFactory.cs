namespace Web.DotW.API.Models
{
    using Entities.PostEntities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Routing;
    using Web.DotW.API.Infrastructure;
    using Entities.CommentaryEntities;

    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email
            };

        }

        public RoleReturnModel Create(IdentityRole appRole)
        {
            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }

        internal CommentaryModel CreateCommentaryModel(Commentary commentary)
        {
            return new CommentaryModel()
            {
                Id = commentary.Id,
                Date = commentary.EffectDate,
                IdWriter = commentary.IdUser,
                WriterUsername = commentary.WriterUserName,
                Text = commentary.CommentaryText,
                // TODO Agu -> Cambiar por la url del detalle de usuario
                //WriterUrl = _UrlHelper.Link("GetRoleById", new { id = commentary.IdUser })
            };

        }

        internal AnswerModel CreateAnswerModel(Commentary answer)
        {
            return new AnswerModel()
            {
                Id = answer.Id,
                Date = answer.EffectDate,
                IdWriter = answer.IdUser,
                WriterUsername = answer.WriterUserName,
                Text = answer.CommentaryText,
                // TODO Agu -> Cambiar por la url del detalle de usuario
                //WriterUrl = _UrlHelper.Link("GetRoleById", new { id = answer.IdUser })
            };
        }

        internal GetPostModel CreateGetPostModel(Post post)
        {
            return new GetPostModel
            {
                Id = post.Id,
                Title = post.Title,
                Summary = post.Summary,
                Body = post.Body,
                Tags = post.Tags,
                EffectDate = post.EffectDate,
                CategoryId = post.IdCategory,
                CategoryUrl = _UrlHelper.Link("GetCategoryById", new { id = post.IdCategory }),
                ImageUrl = post.PrincipalImageName.Contains("api/") ?
                            ConfigurationManager.AppSettings["UrlImagesAPI"] + post.PrincipalImageName.Split('/')[1]
                            :
                            ConfigurationManager.AppSettings["UrlImagesWeb"] + post.PrincipalImageName,
                WritterId = post.IdWriter,
                // TODO npellegrinet > Agregar perfil.
            };
        }
    }

    public class UserReturnModel
    {

        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }


    }

    public class RoleReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}