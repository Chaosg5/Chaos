------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSourceMovieTypeAddress 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSourceMovieTypeAddress]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[ExternalSourceMovieTypeAddress](
		[ExternalSourceId] [int] NOT NULL,
		[MovieTypeId] [int] NOT NULL,
		[Address] [nvarchar](150) NOT NULL,
	 CONSTRAINT [PK_ExternalSourceMovieTypeAddress] PRIMARY KEY CLUSTERED 
	(
		[ExternalSourceId] ASC,
		[MovieTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExternalSourceMovieTypeAddress_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExternalSourceMovieTypeAddress]'))
BEGIN
	ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress] CHECK CONSTRAINT [FK_ExternalSourceMovieTypeAddress_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExternalSourceMovieTypeAddress_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExternalSourceMovieTypeAddress]'))
BEGIN
	ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress] CHECK CONSTRAINT [FK_ExternalSourceMovieTypeAddress_MovieTypes]
END 

GO

------------------------------------- 
