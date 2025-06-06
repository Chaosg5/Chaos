------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesWatchedByUser 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MoviesWatchedByUser](
		[MoviesWatchedByUserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[UserId] [int] NOT NULL,
		[WatchedDate] [date] NOT NULL,
		[DateUncertain] [bit] NOT NULL,
		[WatchTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_MoviesWatchedByUser] PRIMARY KEY CLUSTERED 
	(
		[MoviesWatchedByUserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_MoviesWatchedByUser_DateUncertain]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] ADD  CONSTRAINT [DF_MoviesWatchedByUser_DateUncertain]  DEFAULT ((0)) FOR [DateUncertain]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_Users]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_WatchTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_WatchTypes]
END 

GO

------------------------------------- 
