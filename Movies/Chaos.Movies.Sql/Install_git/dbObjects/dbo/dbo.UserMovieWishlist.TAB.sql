------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieWishlist 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieWishlist](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
	 CONSTRAINT [PK_UserMovieWishlist] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserMovieWishlist_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] ADD  CONSTRAINT [DF_UserMovieWishlist_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieWishlist_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [FK_UserMovieWishlist_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieWishlist_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [FK_UserMovieWishlist_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieWishlist_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieWishlist_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieWishlist_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [CK_UserMovieWishlist_Rating]
END 

GO

------------------------------------- 
