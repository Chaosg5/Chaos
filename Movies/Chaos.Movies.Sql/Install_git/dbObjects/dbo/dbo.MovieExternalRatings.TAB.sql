------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieExternalRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieExternalRatings](
		[MovieId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalRating] [float] NOT NULL,
		[RatingCount] [int] NOT NULL,
	 CONSTRAINT [PK_MovieExternalRating] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_MovieExternalRatings_ExternalRating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] ADD  CONSTRAINT [DF_MovieExternalRatings_ExternalRating]  DEFAULT ((0)) FOR [ExternalRating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalRatings_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [FK_MovieExternalRatings_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalRatings_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [FK_MovieExternalRatings_Movies]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_MovieExternalRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD  CONSTRAINT [CK_MovieExternalRatings_Rating] CHECK  (([ExternalRating]>=(0) AND [ExternalRating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_MovieExternalRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [CK_MovieExternalRatings_Rating]
END 

GO

------------------------------------- 
