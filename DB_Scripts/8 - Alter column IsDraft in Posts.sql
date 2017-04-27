USE [DotW]
GO

UPDATE [dbo].[Posts] SET IsDraft = 0
GO

ALTER TABLE [dbo].[Posts]
ALTER COLUMN [IsDraft] BIT NOT NULL;

GO