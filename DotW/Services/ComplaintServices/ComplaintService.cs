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

                var response = new CreatePostComplaintResponse { ComplaintId = complaint.Id, PostId = complaint.IdPost.Value };

                response.PostComplaintsCount = db.Complaints.Count(x => x.IdPost == response.PostId);

                return response;
            }
        }

        public CreateCommentaryComplaintResponse CreateCommentaryComplaint(CreateCommentaryComplaintRequest request)
        {
            using (var db = new DotWEntities())
            {
                var complaint = new Complaints
                {
                    IdComment = request.CommentaryId,
                    IdUser = request.UserId,
                    Description = request.Commentary
                };

                db.Complaints.Add(complaint);
                db.SaveChanges();

                var response = new CreateCommentaryComplaintResponse { ComplaintId = complaint.Id, CommentaryId = complaint.IdComment.Value };

                response.CommentaryComplaintsCount = db.Complaints.Count(x => x.IdComment == response.CommentaryId);

                return response;
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

        public SearchComplaintsByPostIdResponse SearchComplaintsByPostId(SearchComplaintsByPostIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new List<Complaint>();

                result = db.Complaints.Where(x => x.IdPost == request.PostId).Select(x => new Complaint
                {
                    Id = x.Id,
                    Description = x.Description,
                    IdComment = x.IdComment,
                    IdPost = x.IdPost,
                    IdUser = x.IdUser
                }).ToList();

                return new SearchComplaintsByPostIdResponse { Complaints = result };
            }
        }

        public SearchComplaintsByCommentaryIdResponse SearchComplaintsByCommentaryId(SearchComplaintsByCommentaryIdRequest request)
        {
            using (var db = new DotWEntities())
            {
                var result = new List<Complaint>();

                result = db.Complaints.Where(x => x.IdComment == request.CommentaryId).Select(x => new Complaint
                {
                    Id = x.Id,
                    Description = x.Description,
                    IdComment = x.IdComment,
                    IdPost = x.IdPost,
                    IdUser = x.IdUser
                }).ToList();

                return new SearchComplaintsByCommentaryIdResponse { Complaints = result };
            }
        }
    }
}
