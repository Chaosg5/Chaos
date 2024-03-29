------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguagesTitlesCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'LanguagesTitlesCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[LanguagesTitlesCollection] AS TABLE(
		[InLanguage] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[InLanguage] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguagesTitlesCollection] 
grant execute on type :: [dbo].[LanguagesTitlesCollection] TO [rCMDB] 
GO

------------------------------------- 
