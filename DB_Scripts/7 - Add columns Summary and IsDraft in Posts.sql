USE [DotW]
GO

ALTER TABLE [dbo].[Posts]
ADD [IsDraft] BIT NULL,
    [Summary] varchar(500);

GO