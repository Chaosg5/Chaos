------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserCharacterFavorites 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserCharacterFavorites](
		[UserId] [int] NOT NULL,
		[CharacterId] [int] NOT NULL,
		[FavoriteTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_UserCharactersFavorites] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[CharacterId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_Characters]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_Users]
END 

GO

------------------------------------- 
