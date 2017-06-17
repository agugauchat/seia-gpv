namespace Web.DotW.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class SearchModel
    {
        public List<GetPostsSearchResult> PostsSearchResult { get; set; }

        public List<GetCommentsSearchResult> CommentsSearchResult { get; set; }

    }

    public class GetPostsSearchResult
    {
        public int Id { get; set; }
        public string PostUrl { get; set; }
        public string Title { get; set; }
        public int IdWriter { get; set; }
        public string WriterUserName { get; set; }
        public string WriterUrl { get; set; }
        public DateTime Date { get; set; }
        public string Summary { get; set; }
    }

    public class GetCommentsSearchResult
    {
        public int Id { get; set; }
        public int IdPost { get; set; }
        public string PostUrl { get; set; }
        public string Commentary { get; set; }
        public int IdWriter { get; set; }
        public string WriterUserName { get; set; }
        public string WriterUrl { get; set; }
    }

}