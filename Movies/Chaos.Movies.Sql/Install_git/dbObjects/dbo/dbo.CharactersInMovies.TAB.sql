------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharactersInMovies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharactersInMovies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[CharactersInMovies](
		[CharacterInMovieId] [int] IDENTITY(1,1) NOT NULL,
		[CharacterId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
	 CONSTRAINT [PK_CharactersInMovies] PRIMARY KEY CLUSTERED 
	(
		[CharacterInMovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_CharactersInMovies] UNIQUE NONCLUSTERED 
	(
		[CharacterId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharactersInMovies_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharactersInMovies]'))
BEGIN
	ALTER TABLE [dbo].[CharactersInMovies] CHECK CONSTRAINT [FK_CharactersInMovies_Characters]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharactersInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharactersInMovies]'))
BEGIN
	ALTER TABLE [dbo].[CharactersInMovies] CHECK CONSTRAINT [FK_CharactersInMovies_Movies]
END 

GO

------------------------------------- 
