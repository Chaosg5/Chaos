------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserPersonInMovieRoleRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserPersonInMovieRoleRatings](
		[UserId] [int] NOT NULL,
		[PersonInMovieRoleId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
		[CreatedDate] [date] NOT NULL,
	 CONSTRAINT [PK_UserPersonInMovieRoleRatings] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[PersonInMovieRoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserPersonInMovieRoleRatings_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] ADD  CONSTRAINT [DF_UserPersonInMovieRoleRatings_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPersonInMovieRoleRatings_PersonInMovieRoles]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [FK_UserPersonInMovieRoleRatings_PersonInMovieRoles]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPersonInMovieRoleRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [FK_UserPersonInMovieRoleRatings_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserPersonInMovieRoleRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserPersonInMovieRoleRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserPersonInMovieRoleRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [CK_UserPersonInMovieRoleRatings_Rating]
END 

GO

------------------------------------- 
