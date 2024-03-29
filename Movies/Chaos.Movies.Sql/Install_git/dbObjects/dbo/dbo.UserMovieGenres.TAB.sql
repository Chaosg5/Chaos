------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieGenres](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
	 CONSTRAINT [PK_UserMovieGenres] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserMovieGenres_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] ADD  CONSTRAINT [DF_UserMovieGenres_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieGenres_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieGenres_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieGenres_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [CK_UserMovieGenres_Rating]
END 

GO

------------------------------------- 
