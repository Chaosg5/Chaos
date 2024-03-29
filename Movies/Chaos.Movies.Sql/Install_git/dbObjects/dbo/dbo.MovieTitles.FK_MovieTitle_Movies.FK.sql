------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTitles 
-- name     : FK_MovieTitle_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitle_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieTitle_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
