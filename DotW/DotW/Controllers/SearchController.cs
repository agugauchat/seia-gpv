﻿using Contracts.SearchContracts.Request;
using Services.SearchServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotW.Controllers
{
    public class SearchController : BaseController
    {
        [HttpPost]
        public ActionResult Index(string text)
        {
            var searchService = new SearchService();

            ViewBag.PostsList = searchService.SearchInPosts(new SearchInPostsRequest { Text = text }).PostsSearchResult;

            ViewBag.CommentsList = searchService.SearchInComments(new SearchInCommentsRequest { Text = text }).CommentsSearchResult;

            return View();
        }
    }
}