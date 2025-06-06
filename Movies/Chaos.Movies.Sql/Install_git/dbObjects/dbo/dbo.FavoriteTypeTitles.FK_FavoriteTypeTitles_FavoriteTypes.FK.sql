------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : FavoriteTypeTitles 
-- name     : FK_FavoriteTypeTitles_FavoriteTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FavoriteTypeTitles_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[FavoriteTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_FavoriteTypeTitles_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
	REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])

END 

GO

------------------------------------- 
