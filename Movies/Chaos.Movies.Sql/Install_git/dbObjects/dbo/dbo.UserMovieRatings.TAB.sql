------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieRatings](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[RatingTypeId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK_UserMovieRatings] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC,
		[RatingTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_UserMovieRatings] UNIQUE NONCLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserMovieRatings_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] ADD  CONSTRAINT [DF_UserMovieRatings_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_RatingTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [CK_UserMovieRatings_Rating]
END 

GO

------------------------------------- 
