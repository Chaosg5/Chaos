------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovieSeries 
-- name     : FK_IconsInCollections_Icons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries]  WITH CHECK ADD CONSTRAINT [FK_IconsInCollections_Icons] FOREIGN KEY([IconId])
	REFERENCES [dbo].[Icons] ([IconId])

END 

GO

------------------------------------- 
