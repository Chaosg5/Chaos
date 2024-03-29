------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSources 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSources]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[ExternalSources](
		[ExternalSourceId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
		[BaseAddress] [nvarchar](250) NULL,
		[PeopleAddress] [nvarchar](150) NULL,
		[CharacterAddress] [nvarchar](150) NULL,
		[GenreAddress] [nvarchar](150) NULL,
		[EpisodeAddress] [nvarchar](150) NULL,
	 CONSTRAINT [PK_ExternalSource] PRIMARY KEY CLUSTERED 
	(
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
