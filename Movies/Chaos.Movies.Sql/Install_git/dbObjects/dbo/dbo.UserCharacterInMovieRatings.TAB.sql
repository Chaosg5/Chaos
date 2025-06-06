------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserCharacterInMovieRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserCharacterInMovieRatings](
		[UserId] [int] NOT NULL,
		[CharacterInMovieId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK_UserCharacterInMovieRatings] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[CharacterInMovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserCharacterInMovieRatings_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] ADD  CONSTRAINT [DF_UserCharacterInMovieRatings_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharacterInMovieRatings_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [FK_UserCharacterInMovieRatings_CharactersInMovies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharacterInMovieRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [FK_UserCharacterInMovieRatings_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserCharacterInMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserCharacterInMovieRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserCharacterInMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [CK_UserCharacterInMovieRatings_Rating]
END 

GO

------------------------------------- 
