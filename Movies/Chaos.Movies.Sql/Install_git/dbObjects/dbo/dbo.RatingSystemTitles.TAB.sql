------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingSystemTitles](
		[RatingSystemId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](500) NULL,
	 CONSTRAINT [PK_RatingSystemTitles] PRIMARY KEY CLUSTERED 
	(
		[RatingSystemId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemTitles] CHECK CONSTRAINT [FK_RatingSystemTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemTitles_RatingSystems]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemTitles] CHECK CONSTRAINT [FK_RatingSystemTitles_RatingSystems]
END 

GO

------------------------------------- 
