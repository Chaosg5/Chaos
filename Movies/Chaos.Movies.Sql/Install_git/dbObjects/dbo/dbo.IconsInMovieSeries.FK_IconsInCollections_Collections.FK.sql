------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovieSeries 
-- name     : FK_IconsInCollections_Collections 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries]  WITH CHECK ADD CONSTRAINT [FK_IconsInCollections_Collections] FOREIGN KEY([MovieSeriesId])
	REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])

END 

GO

------------------------------------- 
