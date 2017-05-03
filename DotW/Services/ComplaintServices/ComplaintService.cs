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
    using Entities.ComplaintEntities;

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

        public SearchComplaintsByUserIdResponse SearchComplaintsByUserId(SearchComplaintsByUserIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new List<Complaint>();

                if (request.UserId.HasValue)
                {
                    result = db.Complaints.Where(x => x.IdUser == request.UserId).Select(x => new Complaint
                    {
                        Id = x.Id,
                        Description = x.Description,
                        IdComment = x.IdComment,
                        IdPost = x.IdPost,
                        IdUser = x.IdUser
                    }).ToList();
                }
                else if (!string.IsNullOrEmpty(request.AspNetUserId))
                {
                    result = db.Complaints.Where(x => x.Users.AspNetUserId == request.AspNetUserId).Select(x => new Complaint
                    {
                        Id = x.Id,
                        Description = x.Description,
                        IdComment = x.IdComment,
                        IdPost = x.IdPost,
                        IdUser = x.IdUser
                    }).ToList();
                }

                return new SearchComplaintsByUserIdResponse { Complaints = result };
            }
        }
    }
}
