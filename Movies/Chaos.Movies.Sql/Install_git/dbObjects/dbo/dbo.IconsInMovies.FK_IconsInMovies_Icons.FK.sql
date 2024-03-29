------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovies 
-- name     : FK_IconsInMovies_Icons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies]  WITH CHECK ADD CONSTRAINT [FK_IconsInMovies_Icons] FOREIGN KEY([IconId])
	REFERENCES [dbo].[Icons] ([IconId])

END 

GO

------------------------------------- 
