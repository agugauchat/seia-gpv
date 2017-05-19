namespace Entities.SearchEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PostsSearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int IdWriter { get; set; }
        public string Body { get; set; }
        public DateTime EffectDate { get; set; }
        public DateTime? NullDate { get; set; }
        public int IdCategory { get; set; }
        public bool IsDraft { get; set; }
        public string Summary { get; set; }
        public string PrincipalImageName { get; set; }
        public bool DeletedByComplaintsOrVotes { get; set; }
        public string WriterUserName { get; set; }
    }
}
