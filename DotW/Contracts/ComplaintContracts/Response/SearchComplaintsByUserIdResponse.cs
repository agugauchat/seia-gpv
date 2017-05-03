namespace Contracts.ComplaintContracts.Response
{
    using Entities.ComplaintEntities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchComplaintsByUserIdResponse
    {
        public List<Complaint> Complaints { get; set; }
    }
}
