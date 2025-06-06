------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTitles 
-- name     : FK_MovieTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
