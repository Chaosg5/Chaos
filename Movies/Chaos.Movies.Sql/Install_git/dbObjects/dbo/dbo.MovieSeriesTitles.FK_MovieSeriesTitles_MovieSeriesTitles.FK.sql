------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTitles 
-- name     : FK_MovieSeriesTitles_MovieSeriesTitles 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTitles_MovieSeriesTitles]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieSeriesTitles_MovieSeriesTitles] FOREIGN KEY([MovieSeriesId], [Language])
	REFERENCES [dbo].[MovieSeriesTitles] ([MovieSeriesId], [Language])

END 

GO

------------------------------------- 
