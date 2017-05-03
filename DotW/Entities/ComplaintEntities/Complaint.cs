namespace Entities.ComplaintEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Complaint
    {
        public int Id { get; set; }
        public int? IdPost { get; set; }
        public int? IdComment { get; set; }
        public int IdUser { get; set; }
        public string Description { get; set; }
    }
}
