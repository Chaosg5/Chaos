------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieExternalRatings 
-- name     : FK_MovieExternalRatings_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalRatings_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD CONSTRAINT [FK_MovieExternalRatings_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
