namespace Entities.UserEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AspNetUserId { get; set; }
        public DateTime EffectDate { get; set; }
        public int IdState { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public bool ShowData { get; set; }
        public DateTime? NullDate { get; set; }
    }
}
