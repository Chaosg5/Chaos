------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : EpisodeExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[EpisodeExternalLookup](
		[EpisodeId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_EpisodeExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[EpisodeId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeExternalLookup_Episodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeExternalLookup] CHECK CONSTRAINT [FK_EpisodeExternalLookup_Episodes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeExternalLookup] CHECK CONSTRAINT [FK_EpisodeExternalLookup_ExternalSources]
END 

GO

------------------------------------- 
