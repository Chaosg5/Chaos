------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeries 
-- name     : FK_Collections_CollectionTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Collections_CollectionTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeries]  WITH CHECK ADD CONSTRAINT [FK_Collections_CollectionTypes] FOREIGN KEY([MovieSeriesTypeId])
	REFERENCES [dbo].[MovieSeriesTypes] ([MovieSeriesTypeId])

END 

GO

------------------------------------- 
