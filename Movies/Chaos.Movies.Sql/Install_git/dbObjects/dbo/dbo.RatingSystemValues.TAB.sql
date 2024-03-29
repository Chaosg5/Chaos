------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemValues 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingSystemValues](
		[RatingSystemId] [int] NOT NULL,
		[RatingTypeId] [int] NOT NULL,
		[Weight] [tinyint] NOT NULL,
	 CONSTRAINT [PK_RatingSystemValues] PRIMARY KEY CLUSTERED 
	(
		[RatingSystemId] ASC,
		[RatingTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemValues_RatingSystems]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemValues] CHECK CONSTRAINT [FK_RatingSystemValues_RatingSystems]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemValues_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemValues] CHECK CONSTRAINT [FK_RatingSystemValues_RatingTypes]
END 

GO

------------------------------------- 
