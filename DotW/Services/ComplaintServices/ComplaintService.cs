namespace Services.ComplaintServices
{
    using Contracts.ComplaintContracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.ComplaintContracts.Request;
    using Contracts.ComplaintContracts.Response;
    using Data;

    public class ComplaintService : IComplaintService
    {
        public CreatePostComplaintResponse CreatePostComplaint(CreatePostComplaintRequest request)
        {
            using (var db = new DotWEntities())
            {
                var complaint = new Complaints
                {
                    IdPost = request.PostId,
                    IdUser = request.UserId,
                    Description = request.Commentary
                };

                db.Complaints.Add(complaint);
                db.SaveChanges();

                return new CreatePostComplaintResponse { ComplaintId = complaint.Id };
            }
        }
    }
}
