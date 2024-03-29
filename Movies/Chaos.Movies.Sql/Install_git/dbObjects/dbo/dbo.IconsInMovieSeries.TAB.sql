------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInMovieSeries 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInMovieSeries](
		[IconId] [int] NOT NULL,
		[MovieSeriesId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInCollections] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[MovieSeriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries] CHECK CONSTRAINT [FK_IconsInCollections_Collections]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries] CHECK CONSTRAINT [FK_IconsInCollections_Icons]
END 

GO

------------------------------------- 
