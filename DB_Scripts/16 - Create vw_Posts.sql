USE [DotW]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Posts] WITH SCHEMABINDING AS
SELECT P.[Id]
      ,P.[Title]
      ,P.[IdWriter]
      ,P.[Body]
      ,P.[EffectDate]
      ,P.[NullDate]
      ,P.[IdCategory]
      ,P.[IsDraft]
      ,P.[Summary]
      ,P.[PrincipalImageName]
      ,P.[DeletedByComplaintsOrVotes]
	  ,U.Name AS WriterUserName
FROM dbo.Posts P
INNER JOIN dbo.Users U ON P.IdWriter = U.Id
WHERE P.[NullDate] IS NULL AND P.[IsDraft] = 0
GO

SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO

CREATE UNIQUE CLUSTERED INDEX [uci_PostId] ON [dbo].[vw_Posts]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE FULLTEXT INDEX ON [dbo].[vw_Posts](
[Body] LANGUAGE 'Spanish', 
[Summary] LANGUAGE 'Spanish', 
[Title] LANGUAGE 'Spanish',
[WriterUserName] LANGUAGE 'Spanish')
KEY INDEX [uci_PostId]ON ([PostsCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)

GO


