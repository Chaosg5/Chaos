------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : SongsInEpisodes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[SongsInEpisodes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[SongsInEpisodes](
		[SongId] [int] NOT NULL,
		[EpisodeId] [int] NOT NULL,
	 CONSTRAINT [PK_SongsInEpisodes] PRIMARY KEY CLUSTERED 
	(
		[SongId] ASC,
		[EpisodeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInEpisodes_Episodes]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInEpisodes]'))
BEGIN
	ALTER TABLE [music].[SongsInEpisodes] CHECK CONSTRAINT [FK_SongsInEpisodes_Episodes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInEpisodes_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInEpisodes]'))
BEGIN
	ALTER TABLE [music].[SongsInEpisodes] CHECK CONSTRAINT [FK_SongsInEpisodes_Songs]
END 

GO

------------------------------------- 
