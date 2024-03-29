------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : EpisodeExternalLookup 
-- name     : FK_EpisodeExternalLookup_Episodes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeExternalLookup_Episodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_EpisodeExternalLookup_Episodes] FOREIGN KEY([EpisodeId])
	REFERENCES [dbo].[Episodes] ([EpisodeId])

END 

GO

------------------------------------- 
