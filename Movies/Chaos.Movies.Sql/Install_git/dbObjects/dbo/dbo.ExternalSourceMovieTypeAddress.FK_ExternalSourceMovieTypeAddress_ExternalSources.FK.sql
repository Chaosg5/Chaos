------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : ExternalSourceMovieTypeAddress 
-- name     : FK_ExternalSourceMovieTypeAddress_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExternalSourceMovieTypeAddress_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExternalSourceMovieTypeAddress]'))
BEGIN
	ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress]  WITH CHECK ADD CONSTRAINT [FK_ExternalSourceMovieTypeAddress_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
