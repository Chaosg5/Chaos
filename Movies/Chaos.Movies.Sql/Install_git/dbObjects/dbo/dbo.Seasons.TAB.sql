------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Seasons 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Seasons]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Seasons](
		[SeasonId] [int] IDENTITY(1,1) NOT NULL,
		[MovieId] [int] NOT NULL,
		[Number] [smallint] NOT NULL,
	 CONSTRAINT [PK_Seasons] PRIMARY KEY CLUSTERED 
	(
		[SeasonId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Seasons_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[Seasons]'))
BEGIN
	ALTER TABLE [dbo].[Seasons] CHECK CONSTRAINT [FK_Seasons_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Seasons_Movies1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Seasons]'))
BEGIN
	ALTER TABLE [dbo].[Seasons] CHECK CONSTRAINT [FK_Seasons_Movies1]
END 

GO

------------------------------------- 
