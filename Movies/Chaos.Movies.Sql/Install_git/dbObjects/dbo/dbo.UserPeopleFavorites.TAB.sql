------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserPeopleFavorites 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserPeopleFavorites](
		[UserId] [int] NOT NULL,
		[PersonId] [int] NOT NULL,
		[FavoriteTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_UserPeopleFavorites] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[PersonId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_People]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_Users]
END 

GO

------------------------------------- 
