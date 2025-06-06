------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageDescriptionCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'LanguageDescriptionCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[LanguageDescriptionCollection] AS TABLE(
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](500) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Language] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageDescriptionCollection] 
grant execute on type :: [dbo].[LanguageDescriptionCollection] TO [rCMDB] 
GO

------------------------------------- 
