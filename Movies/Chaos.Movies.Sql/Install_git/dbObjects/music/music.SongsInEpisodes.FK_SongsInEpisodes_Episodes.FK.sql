------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInEpisodes 
-- name     : FK_SongsInEpisodes_Episodes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInEpisodes_Episodes]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInEpisodes]'))
BEGIN
	ALTER TABLE [music].[SongsInEpisodes]  WITH CHECK ADD CONSTRAINT [FK_SongsInEpisodes_Episodes] FOREIGN KEY([EpisodeId])
	REFERENCES [dbo].[Episodes] ([EpisodeId])

END 

GO

------------------------------------- 
