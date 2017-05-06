USE [DotW]
GO

EXEC sp_RENAME 'Posts.DeletedByComplaints' , 'DeletedByComplaintsOrVotes', 'COLUMN'
GO

EXEC sp_RENAME 'Users.BlockedPublications' , 'BlockedPosts', 'COLUMN'
GO