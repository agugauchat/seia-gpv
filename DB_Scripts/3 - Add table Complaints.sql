USE [DotW]
GO

/****** Object:  Table [dbo].[Complaints]    Script Date: 17/4/2017 18:56:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Complaints](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPost] [int] NULL,
	[IdComment] [int] NULL,
	[IdUser] [int] NOT NULL,
	[Description] [varchar](500) NULL,
 CONSTRAINT [PK_Complaints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_Comment] FOREIGN KEY([IdComment])
REFERENCES [dbo].[Comments] ([Id])
GO

ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_Comment]
GO

ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_Post] FOREIGN KEY([IdPost])
REFERENCES [dbo].[Posts] ([Id])
GO

ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_Post]
GO

ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_User] FOREIGN KEY([IdUser])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_User]
GO


