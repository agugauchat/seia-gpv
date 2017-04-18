USE [DotW]
GO

DROP TABLE [dbo].[PostCategories]
GO

DELETE FROM [dbo].[Posts]
GO

ALTER TABLE [dbo].[Posts]
ADD IdCategory INT NOT NULL;
GO

ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Category] FOREIGN KEY([IdCategory])
REFERENCES [dbo].[Categories] ([Id])
GO