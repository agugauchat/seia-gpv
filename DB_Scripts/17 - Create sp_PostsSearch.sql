USE [DotW]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_PostsSearch] 
	@text varchar(20)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT * FROM vw_Posts WHERE FREETEXT(*, @text)
END
GO


