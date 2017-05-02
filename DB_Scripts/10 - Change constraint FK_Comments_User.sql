USE [DotW]
GO

ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_User];

ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_User] FOREIGN KEY([IdUser])
REFERENCES [dbo].[Users] ([Id])
GO


