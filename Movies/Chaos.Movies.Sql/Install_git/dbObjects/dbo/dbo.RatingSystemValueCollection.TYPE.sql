------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemValueCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'RatingSystemValueCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[RatingSystemValueCollection] AS TABLE(
		[RatingTypeId] [int] NOT NULL,
		[Weight] [tinyint] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[RatingTypeId] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemValueCollection] 
grant execute on type :: [dbo].[RatingSystemValueCollection] TO [rCMDB] 
GO

------------------------------------- 
