------------------------------------------------------------ 
------------------------------------------------------------ 
--- EyeDoc SQL script- CMDB.music                       
------------------------------------------------------------ 
--- Scheme    : music 
--- Name    : CMDB 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Role ----------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Schema --------------------------------- 
------------------------------------------------------------ 
-- music.schema.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : music 
-- Type     : schema 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'music')
BEGIN
	EXEC sys.sp_executesql N'CREATE SCHEMA [music]'

END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ PRE Create tables ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create tables --------------------------------- 
------------------------------------------------------------ 
-- music.Albums.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : Albums 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[Albums]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[Albums](
		[AlbumId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](150) NOT NULL,
	 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
	(
		[AlbumId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- music.AlbumsInGenres.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : AlbumsInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[AlbumsInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[AlbumsInGenres](
		[AlbumId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_AlbumsInGenres] PRIMARY KEY CLUSTERED 
	(
		[AlbumId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_AlbumsInGenres_Albums]') AND parent_object_id = OBJECT_ID(N'[music].[AlbumsInGenres]'))
BEGIN
	ALTER TABLE [music].[AlbumsInGenres] CHECK CONSTRAINT [FK_AlbumsInGenres_Albums]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_AlbumsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[AlbumsInGenres]'))
BEGIN
	ALTER TABLE [music].[AlbumsInGenres] CHECK CONSTRAINT [FK_AlbumsInGenres_Genres]
END 

GO

------------------------------------- 
-- music.Artists.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : Artists 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[Artists]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[Artists](
		[ArtistId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](150) NOT NULL,
	 CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED 
	(
		[ArtistId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- music.ArtistsInGenres.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : ArtistsInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[ArtistsInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[ArtistsInGenres](
		[ArtistId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_ArtistsInGenres] PRIMARY KEY CLUSTERED 
	(
		[ArtistId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Artists]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres] CHECK CONSTRAINT [FK_ArtistsInGenres_Artists]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres] CHECK CONSTRAINT [FK_ArtistsInGenres_Genres]
END 

GO

------------------------------------- 
-- music.Genres.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : Genres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[Genres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[Genres](
		[GenreId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
	(
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- music.GenreTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : GenreTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[GenreTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[GenreTitles](
		[GenreId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_GenreTitles] PRIMARY KEY CLUSTERED 
	(
		[GenreId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_GenreTitles_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[GenreTitles]'))
BEGIN
	ALTER TABLE [music].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_GenreTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[music].[GenreTitles]'))
BEGIN
	ALTER TABLE [music].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Languages]
END 

GO

------------------------------------- 
-- music.Songs.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : Songs 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[Songs]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[Songs](
		[SongId] [int] IDENTITY(1,1) NOT NULL,
		[ArtistId] [int] NOT NULL,
		[AlbumId] [int] NOT NULL,
		[Name] [nvarchar](150) NOT NULL,
	 CONSTRAINT [PK_Songs] PRIMARY KEY CLUSTERED 
	(
		[SongId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_Songs_Albums]') AND parent_object_id = OBJECT_ID(N'[music].[Songs]'))
BEGIN
	ALTER TABLE [music].[Songs] CHECK CONSTRAINT [FK_Songs_Albums]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_Songs_Artists]') AND parent_object_id = OBJECT_ID(N'[music].[Songs]'))
BEGIN
	ALTER TABLE [music].[Songs] CHECK CONSTRAINT [FK_Songs_Artists]
END 

GO

------------------------------------- 
-- music.SongsInEpisodes.TAB.sql 
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
-- music.SongsInGenres.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : SongsInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[SongsInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[SongsInGenres](
		[SongId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_SongsInGenres] PRIMARY KEY CLUSTERED 
	(
		[SongId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInGenres]'))
BEGIN
	ALTER TABLE [music].[SongsInGenres] CHECK CONSTRAINT [FK_SongsInGenres_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInGenres_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInGenres]'))
BEGIN
	ALTER TABLE [music].[SongsInGenres] CHECK CONSTRAINT [FK_SongsInGenres_Songs]
END 

GO

------------------------------------- 
-- music.SongsInMovies.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : SongsInMovies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[SongsInMovies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[SongsInMovies](
		[SongId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
	 CONSTRAINT [PK_SongsInMovies] PRIMARY KEY CLUSTERED 
	(
		[SongId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies] CHECK CONSTRAINT [FK_SongsInMovies_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies] CHECK CONSTRAINT [FK_SongsInMovies_Songs]
END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------ Update tables --------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create TYPE ----------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Set dafault data ------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Set config data ------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create CONSTRAINT ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update CONSTRAINT ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create CHECK CONSTRAINT ----------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update CHECK CONSTRAINT ----------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create REFERENCES ----------------------------- 
------------------------------------------------------------ 
-- music.AlbumsInGenres.FK_AlbumsInGenres_Albums.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : AlbumsInGenres 
-- name     : FK_AlbumsInGenres_Albums 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_AlbumsInGenres_Albums]') AND parent_object_id = OBJECT_ID(N'[music].[AlbumsInGenres]'))
BEGIN
	ALTER TABLE [music].[AlbumsInGenres]  WITH CHECK ADD CONSTRAINT [FK_AlbumsInGenres_Albums] FOREIGN KEY([AlbumId])
	REFERENCES [music].[Albums] ([AlbumId])

END 

GO

------------------------------------- 
-- music.AlbumsInGenres.FK_AlbumsInGenres_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : AlbumsInGenres 
-- name     : FK_AlbumsInGenres_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_AlbumsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[AlbumsInGenres]'))
BEGIN
	ALTER TABLE [music].[AlbumsInGenres]  WITH CHECK ADD CONSTRAINT [FK_AlbumsInGenres_Genres] FOREIGN KEY([GenreId])
	REFERENCES [music].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- music.ArtistsInGenres.FK_ArtistsInGenres_Artists.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : ArtistsInGenres 
-- name     : FK_ArtistsInGenres_Artists 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Artists]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres]  WITH CHECK ADD CONSTRAINT [FK_ArtistsInGenres_Artists] FOREIGN KEY([ArtistId])
	REFERENCES [music].[Artists] ([ArtistId])

END 

GO

------------------------------------- 
-- music.ArtistsInGenres.FK_ArtistsInGenres_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : ArtistsInGenres 
-- name     : FK_ArtistsInGenres_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres]  WITH CHECK ADD CONSTRAINT [FK_ArtistsInGenres_Genres] FOREIGN KEY([GenreId])
	REFERENCES [music].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- music.GenreTitles.FK_GenreTitles_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : GenreTitles 
-- name     : FK_GenreTitles_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_GenreTitles_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[GenreTitles]'))
BEGIN
	ALTER TABLE [music].[GenreTitles]  WITH CHECK ADD CONSTRAINT [FK_GenreTitles_Genres] FOREIGN KEY([GenreId])
	REFERENCES [music].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- music.GenreTitles.FK_GenreTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : GenreTitles 
-- name     : FK_GenreTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_GenreTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[music].[GenreTitles]'))
BEGIN
	ALTER TABLE [music].[GenreTitles]  WITH CHECK ADD CONSTRAINT [FK_GenreTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- music.Songs.FK_Songs_Albums.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : Songs 
-- name     : FK_Songs_Albums 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_Songs_Albums]') AND parent_object_id = OBJECT_ID(N'[music].[Songs]'))
BEGIN
	ALTER TABLE [music].[Songs]  WITH CHECK ADD CONSTRAINT [FK_Songs_Albums] FOREIGN KEY([AlbumId])
	REFERENCES [music].[Albums] ([AlbumId])

END 

GO

------------------------------------- 
-- music.Songs.FK_Songs_Artists.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : Songs 
-- name     : FK_Songs_Artists 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_Songs_Artists]') AND parent_object_id = OBJECT_ID(N'[music].[Songs]'))
BEGIN
	ALTER TABLE [music].[Songs]  WITH CHECK ADD CONSTRAINT [FK_Songs_Artists] FOREIGN KEY([ArtistId])
	REFERENCES [music].[Artists] ([ArtistId])

END 

GO

------------------------------------- 
-- music.SongsInEpisodes.FK_SongsInEpisodes_Episodes.FK.sql 
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
-- music.SongsInEpisodes.FK_SongsInEpisodes_Songs.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInEpisodes 
-- name     : FK_SongsInEpisodes_Songs 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInEpisodes_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInEpisodes]'))
BEGIN
	ALTER TABLE [music].[SongsInEpisodes]  WITH CHECK ADD CONSTRAINT [FK_SongsInEpisodes_Songs] FOREIGN KEY([SongId])
	REFERENCES [music].[Songs] ([SongId])

END 

GO

------------------------------------- 
-- music.SongsInGenres.FK_SongsInGenres_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInGenres 
-- name     : FK_SongsInGenres_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInGenres]'))
BEGIN
	ALTER TABLE [music].[SongsInGenres]  WITH CHECK ADD CONSTRAINT [FK_SongsInGenres_Genres] FOREIGN KEY([GenreId])
	REFERENCES [music].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- music.SongsInGenres.FK_SongsInGenres_Songs.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInGenres 
-- name     : FK_SongsInGenres_Songs 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInGenres_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInGenres]'))
BEGIN
	ALTER TABLE [music].[SongsInGenres]  WITH CHECK ADD CONSTRAINT [FK_SongsInGenres_Songs] FOREIGN KEY([SongId])
	REFERENCES [music].[Songs] ([SongId])

END 

GO

------------------------------------- 
-- music.SongsInMovies.FK_SongsInMovies_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInMovies 
-- name     : FK_SongsInMovies_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies]  WITH CHECK ADD CONSTRAINT [FK_SongsInMovies_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- music.SongsInMovies.FK_SongsInMovies_Songs.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInMovies 
-- name     : FK_SongsInMovies_Songs 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies]  WITH CHECK ADD CONSTRAINT [FK_SongsInMovies_Songs] FOREIGN KEY([SongId])
	REFERENCES [music].[Songs] ([SongId])

END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------ Update REFERENCES ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update INDEX ---------------------------------- 
------------------------------------------------------------ 
------------ Update Unique INDEX --------------------------- 
------------ Update Non-Unique INDEX ----------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create userdefind functions ------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Views----------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create CONSTRAINT UNIQUE NONCLUSTERED --------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update CONSTRAINT UNIQUE NONCLUSTERED --------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Stored procedures----------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Remove Objects -------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Rights ---------------------------------------- 
------------------------------------------------------------ 
