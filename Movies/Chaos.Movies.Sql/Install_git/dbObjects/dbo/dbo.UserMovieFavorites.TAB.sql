------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieFavorites 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieFavorites](
		[MovieId] [int] NOT NULL,
		[UserId] [int] NOT NULL,
		[FavoriteTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_UserMovieFavorites] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[UserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_Users]
END 

GO

------------------------------------- 
