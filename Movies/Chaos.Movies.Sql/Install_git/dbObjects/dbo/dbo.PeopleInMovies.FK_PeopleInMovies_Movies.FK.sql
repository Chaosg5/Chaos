------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PeopleInMovies 
-- name     : FK_PeopleInMovies_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PeopleInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[PeopleInMovies]'))
BEGIN
	ALTER TABLE [dbo].[PeopleInMovies]  WITH CHECK ADD CONSTRAINT [FK_PeopleInMovies_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
