------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieHashtags 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieHashtags](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[Hashtag] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_UserMovieHashtags] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC,
		[Hashtag] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Hashtags]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Hashtags]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Users]
END 

GO

------------------------------------- 
