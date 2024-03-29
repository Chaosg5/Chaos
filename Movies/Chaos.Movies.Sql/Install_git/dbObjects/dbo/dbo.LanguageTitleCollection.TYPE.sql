------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageTitleCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'LanguageTitleCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[LanguageTitleCollection] AS TABLE(
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Language] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageTitleCollection] 
grant execute on type :: [dbo].[LanguageTitleCollection] TO [rCMDB] 
GO

------------------------------------- 
