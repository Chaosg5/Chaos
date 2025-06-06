------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PeopleInMovies 
-- name     : FK_PeopleInMovies_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PeopleInMovies_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[PeopleInMovies]'))
BEGIN
	ALTER TABLE [dbo].[PeopleInMovies]  WITH CHECK ADD CONSTRAINT [FK_PeopleInMovies_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
