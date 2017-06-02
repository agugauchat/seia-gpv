using Contracts.PostContracts.Request;
using Contracts.VoteContracts.Request;
using Services.PostServices;
using Services.VoteServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotW.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var postService = new PostService();
            var votesService = new VoteService();

            var posts = postService.SearchPostsForHomeRankings(new SearchPostsForHomeRankingsRequest()).Posts;
            foreach (var post in posts)
            {
                // Cuenta los votos positivos de cada post para después ordenar la lista por ese atributo.
                post.GoodVotes = votesService.GetVotesCountByPostId(new GetVotesCountByPostIdRequest { PostId = post.Id }).GoodVotes;
            }

            ViewBag.RecentsPosts = posts.OrderByDescending(x => x.EffectDate).Take(5).ToList();
            ViewBag.MostVotedPosts = posts.OrderByDescending(x => x.GoodVotes).Take(5).ToList();

            return View();
        }


        public ActionResult TermsAndConditions()
        {
            return View();
        }
    }
}