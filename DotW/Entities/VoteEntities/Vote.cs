namespace Entities.VoteEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Vote
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool? Good { get; set; }

        public bool? Bad { get; set; }

        public int PostId { get; set; }
    }
}