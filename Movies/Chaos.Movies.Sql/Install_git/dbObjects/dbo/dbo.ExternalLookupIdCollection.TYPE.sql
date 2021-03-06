------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalLookupIdCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'ExternalLookupIdCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[ExternalLookupIdCollection] AS TABLE(
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[ExternalSourceId] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalLookupIdCollection] 
grant execute on type :: [dbo].[ExternalLookupIdCollection] TO [rCMDB] 
GO

------------------------------------- 
