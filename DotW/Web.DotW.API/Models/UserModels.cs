namespace Web.DotW.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Description { get; set; }

        public List<PostsForUserDetail> Post { get; set; }
    }

    public class PostsForUserDetail
    {
        public int Id { get; set; }

        public string PostUrl { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }
    }
}