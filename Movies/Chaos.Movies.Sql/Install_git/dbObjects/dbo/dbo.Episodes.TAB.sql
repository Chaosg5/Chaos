------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Episodes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Episodes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Episodes](
		[EpisodeId] [int] IDENTITY(1,1) NOT NULL,
		[SeasonId] [int] NOT NULL,
		[Number] [smallint] NOT NULL,
	 CONSTRAINT [PK_Episodes] PRIMARY KEY CLUSTERED 
	(
		[EpisodeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Episodes_Seasons]') AND parent_object_id = OBJECT_ID(N'[dbo].[Episodes]'))
BEGIN
	ALTER TABLE [dbo].[Episodes] CHECK CONSTRAINT [FK_Episodes_Seasons]
END 

GO

------------------------------------- 
