------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieExternalLookup 
-- name     : FK_MovieExternalLookup_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_MovieExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
