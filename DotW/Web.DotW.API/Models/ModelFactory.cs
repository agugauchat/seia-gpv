﻿namespace Web.DotW.API.Models
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
    using Entities.CategoryEntities;
    using Entities.SearchEntities;
    using Entities.UserEntities;

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

        internal UserModel CreateUserModel(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                UserName = user.Name,
                Email = (user.ShowData) ? user.Email : null,
                FullName = (user.ShowData) ? user.FullName : null,
                Phone = (user.ShowData) ? user.Phone : null,
                Description = (user.ShowData) ? user.Description : null
            };
        }

        internal PostsForUserDetail CreatePostsForUserDetail(Post post)
        {
            return new PostsForUserDetail()
            {
                Id = post.Id,
                Date = post.EffectDate,
                Summary = post.Summary,
                Title = post.Title,
                PostUrl = _UrlHelper.Link("GetPostById", new { id = post.Id })
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
                WriterUrl = _UrlHelper.Link("GetUser", new { id = commentary.IdUser })
            };

        }

        internal GetCommentsSearchResult CreateGetCommentsSearchResult(CommentsSearchResult commentary)
        {
            return new GetCommentsSearchResult()
            {
                Id = commentary.Id,
                IdPost = commentary.IdPost,
                Commentary = commentary.Commentary,
                IdWriter = commentary.IdUser,
                WriterUserName = commentary.WriterUserName,
                PostUrl = _UrlHelper.Link("GetPostById", new { id = commentary.IdPost }),
                WriterUrl = _UrlHelper.Link("GetUser", new { id = commentary.IdUser })
            };
        }

        internal GetPostsSearchResult CreateGetPostsSearchResult(PostsSearchResult post)
        {
            return new GetPostsSearchResult()
            {
                Id = post.Id,
                Date = post.EffectDate,
                IdWriter = post.IdWriter,
                WriterUserName = post.WriterUserName,
                Summary = post.Summary,
                Title = post.Title,
                PostUrl = _UrlHelper.Link("GetPostById", new { id = post.Id }),
                WriterUrl = _UrlHelper.Link("GetUser", new { id = post.IdWriter })
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
                WriterUrl = _UrlHelper.Link("GetUser", new { id = answer.IdUser })
            };
        }

        internal GetPostModel CreateGetPostModel(Post post, Category category)
        {
            var result = new GetPostModel
            {
                Id = post.Id,
                Title = post.Title,
                Summary = post.Summary,
                Body = post.Body,
                Tags = post.Tags,
                EffectDate = post.EffectDate,
                Category = new CategoryModel
                {
                    Id = category.Id,
                    Title = category.Title,
                    Summary = category.Summary,
                    Description = category.Description,
                    IdUpperCategory = category.IdUpperCategory
                },
                CategoryUrl = _UrlHelper.Link("GetCategoryById", new { id = post.IdCategory }),
                WritterId = post.IdWriter,
                WriterUrl = _UrlHelper.Link("GetUser", new { id = post.IdWriter })
            };

            if (!string.IsNullOrEmpty(post.PrincipalImageName) && post.PrincipalImageName.Contains("api/"))
            {
                result.ImageUrl = ConfigurationManager.AppSettings["UrlImagesAPI"] + post.PrincipalImageName.Split('/')[1];
            }
            else if (!string.IsNullOrEmpty(post.PrincipalImageName) && !post.PrincipalImageName.Contains("api/"))
            {
                result.ImageUrl = ConfigurationManager.AppSettings["UrlImagesWeb"] + post.PrincipalImageName;
            }

            return result;
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