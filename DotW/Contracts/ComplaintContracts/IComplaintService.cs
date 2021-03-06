﻿namespace Contracts.ComplaintContracts
{
    using Contracts.ComplaintContracts.Request;
    using Contracts.ComplaintContracts.Response;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IComplaintService
    {
        CreatePostComplaintResponse CreatePostComplaint(CreatePostComplaintRequest request);

        CreateCommentaryComplaintResponse CreateCommentaryComplaint(CreateCommentaryComplaintRequest request);

        SearchComplaintsByUserIdResponse SearchComplaintsByUserId(SearchComplaintsByUserIdRequest request);

        SearchComplaintsByPostIdResponse SearchComplaintsByPostId(SearchComplaintsByPostIdRequest request);

        SearchComplaintsByCommentaryIdResponse SearchComplaintsByCommentaryId(SearchComplaintsByCommentaryIdRequest request);
    }
}
