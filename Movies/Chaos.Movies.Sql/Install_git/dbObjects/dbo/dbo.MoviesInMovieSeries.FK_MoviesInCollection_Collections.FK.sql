------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesInMovieSeries 
-- name     : FK_MoviesInCollection_Collections 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInCollection_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInMovieSeries]  WITH CHECK ADD CONSTRAINT [FK_MoviesInCollection_Collections] FOREIGN KEY([MovieSeriesId])
	REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])

END 

GO

------------------------------------- 
