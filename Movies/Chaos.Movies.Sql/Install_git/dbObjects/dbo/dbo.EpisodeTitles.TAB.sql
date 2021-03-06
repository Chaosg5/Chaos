------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : EpisodeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[EpisodeTitles](
		[EpisodeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_EpisodeTitles] PRIMARY KEY CLUSTERED 
	(
		[EpisodeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeTitles_Episodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeTitles] CHECK CONSTRAINT [FK_EpisodeTitles_Episodes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeTitles] CHECK CONSTRAINT [FK_EpisodeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeTitles_Languages1]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeTitles] CHECK CONSTRAINT [FK_EpisodeTitles_Languages1]
END 

GO

------------------------------------- 
