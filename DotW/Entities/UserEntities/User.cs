namespace Entities.UserEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class User
    {
        public string Name { get; set; }
        public string AspNetUserId { get; set; }
        public DateTime EffectDate { get; set; }
        public int IdState { get; set; }
        public DateTime? SuspendedDate { get; set; }
    }
}
