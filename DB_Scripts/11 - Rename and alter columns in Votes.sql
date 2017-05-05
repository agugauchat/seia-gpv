USE [DotW]
GO

EXEC sp_RENAME 'Votes.God' , 'Good', 'COLUMN'
GO

ALTER TABLE [dbo].[Votes]
ALTER COLUMN [Good] [bit] NOT NULL;
GO

ALTER TABLE [dbo].[Votes]
ALTER COLUMN [Bad] [bit] NOT NULL;
GO