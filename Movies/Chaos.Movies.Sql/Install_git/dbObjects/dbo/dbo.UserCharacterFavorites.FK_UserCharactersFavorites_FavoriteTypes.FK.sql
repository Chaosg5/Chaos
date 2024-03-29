------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserCharacterFavorites 
-- name     : FK_UserCharactersFavorites_FavoriteTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserCharactersFavorites_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
	REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])

END 

GO

------------------------------------- 
