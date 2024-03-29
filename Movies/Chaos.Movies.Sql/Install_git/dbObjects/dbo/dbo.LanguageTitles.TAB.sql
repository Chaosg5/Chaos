------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LanguageTitles](
		[Language] [varchar](8) NOT NULL,
		[InLanguage] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_LanguageTitles] PRIMARY KEY CLUSTERED 
	(
		[Language] ASC,
		[InLanguage] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LanguageTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[LanguageTitles]'))
BEGIN
	ALTER TABLE [dbo].[LanguageTitles] CHECK CONSTRAINT [FK_LanguageTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LanguageTitles_LanguagesIn]') AND parent_object_id = OBJECT_ID(N'[dbo].[LanguageTitles]'))
BEGIN
	ALTER TABLE [dbo].[LanguageTitles] CHECK CONSTRAINT [FK_LanguageTitles_LanguagesIn]
END 

GO

------------------------------------- 
