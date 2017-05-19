namespace Entities.SearchEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CommentsSearchResult
    {
        public int Id { get; set; }
        public int IdPost { get; set; }
        public string Commentary { get; set; }
        public int IdUser { get; set; }
        public string WriterUserName { get; set; }
    }
}
