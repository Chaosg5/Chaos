------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalRatingCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'ExternalRatingCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[ExternalRatingCollection] AS TABLE(
		[ExternalSourceId] [int] NOT NULL,
		[ExternalRating] [float] NOT NULL,
		[RatingCount] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[ExternalSourceId] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalRatingCollection] 
grant execute on type :: [dbo].[ExternalRatingCollection] TO [rCMDB] 
GO

------------------------------------- 
