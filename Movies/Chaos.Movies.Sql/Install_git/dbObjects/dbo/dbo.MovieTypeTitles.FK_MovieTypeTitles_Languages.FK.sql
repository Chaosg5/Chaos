------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTypeTitles 
-- name     : FK_MovieTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
