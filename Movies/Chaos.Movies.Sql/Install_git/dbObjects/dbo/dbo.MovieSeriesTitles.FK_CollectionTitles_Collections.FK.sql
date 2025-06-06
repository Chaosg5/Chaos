------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTitles 
-- name     : FK_CollectionTitles_Collections 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionTitles_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD CONSTRAINT [FK_CollectionTitles_Collections] FOREIGN KEY([MovieSeriesId])
	REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])

END 

GO

------------------------------------- 
