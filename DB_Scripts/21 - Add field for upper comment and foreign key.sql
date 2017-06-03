ALTER TABLE [DotW].[dbo].[Comments]
ADD IdUpperComment int;
GO

ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Comments] FOREIGN KEY([IdUpperComment])
REFERENCES [dbo].[Comments] ([Id])
GO

ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Comments]
GO