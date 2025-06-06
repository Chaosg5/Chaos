------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : WatchTypeTitles 
-- name     : FK_WatchTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WatchTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[WatchTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_WatchTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
