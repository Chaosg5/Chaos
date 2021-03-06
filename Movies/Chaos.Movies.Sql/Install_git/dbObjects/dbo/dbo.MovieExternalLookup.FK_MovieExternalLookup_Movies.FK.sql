------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieExternalLookup 
-- name     : FK_MovieExternalLookup_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_MovieExternalLookup_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
