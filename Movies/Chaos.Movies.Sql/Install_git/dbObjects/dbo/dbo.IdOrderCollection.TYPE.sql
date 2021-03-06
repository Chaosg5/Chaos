------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IdOrderCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'IdOrderCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[IdOrderCollection] AS TABLE(
		[Id] [int] NOT NULL,
		[Order] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IdOrderCollection] 
grant execute on type :: [dbo].[IdOrderCollection] TO [rCMDB] 
GO

------------------------------------- 
