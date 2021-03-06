------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ActorsAsCharacters 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActorsAsCharacters]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[ActorsAsCharacters](
		[PersonId] [int] NOT NULL,
		[CharacterInMovieId] [int] NOT NULL,
		[Info] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_ActorsAsCharacters] PRIMARY KEY CLUSTERED 
	(
		[PersonId] ASC,
		[CharacterInMovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActorsAsCharacters_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActorsAsCharacters]'))
BEGIN
	ALTER TABLE [dbo].[ActorsAsCharacters] CHECK CONSTRAINT [FK_ActorsAsCharacters_CharactersInMovies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActorsAsCharacters_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActorsAsCharacters]'))
BEGIN
	ALTER TABLE [dbo].[ActorsAsCharacters] CHECK CONSTRAINT [FK_ActorsAsCharacters_People]
END 

GO

------------------------------------- 
