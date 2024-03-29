------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : ActorsAsCharacters 
-- name     : FK_ActorsAsCharacters_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActorsAsCharacters_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActorsAsCharacters]'))
BEGIN
	ALTER TABLE [dbo].[ActorsAsCharacters]  WITH CHECK ADD CONSTRAINT [FK_ActorsAsCharacters_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
