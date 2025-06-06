------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IdCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'IdCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[IdCollection] AS TABLE(
		[Id] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IdCollection] 
grant execute on type :: [dbo].[IdCollection] TO [rCMDB] 
GO

------------------------------------- 
