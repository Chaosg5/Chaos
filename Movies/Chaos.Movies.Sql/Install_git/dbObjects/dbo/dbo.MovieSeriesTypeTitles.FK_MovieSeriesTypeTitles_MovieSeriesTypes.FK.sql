------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTypeTitles 
-- name     : FK_MovieSeriesTypeTitles_MovieSeriesTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTypeTitles_MovieSeriesTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieSeriesTypeTitles_MovieSeriesTypes] FOREIGN KEY([MovieSeriesTypeId])
	REFERENCES [dbo].[MovieSeriesTypes] ([MovieSeriesTypeId])

END 

GO

------------------------------------- 
