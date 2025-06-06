------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieReviews 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieReviews](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
		[Title] [nvarchar](140) NOT NULL,
		[Review] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_UserMovieReviews] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews] CHECK CONSTRAINT [FK_UserMovieReviews_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews] CHECK CONSTRAINT [FK_UserMovieReviews_Users]
END 

GO

------------------------------------- 
