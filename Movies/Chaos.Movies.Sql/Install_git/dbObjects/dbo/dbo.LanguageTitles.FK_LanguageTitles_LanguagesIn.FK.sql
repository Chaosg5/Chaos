------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : LanguageTitles 
-- name     : FK_LanguageTitles_LanguagesIn 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LanguageTitles_LanguagesIn]') AND parent_object_id = OBJECT_ID(N'[dbo].[LanguageTitles]'))
BEGIN
	ALTER TABLE [dbo].[LanguageTitles]  WITH CHECK ADD CONSTRAINT [FK_LanguageTitles_LanguagesIn] FOREIGN KEY([InLanguage])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
