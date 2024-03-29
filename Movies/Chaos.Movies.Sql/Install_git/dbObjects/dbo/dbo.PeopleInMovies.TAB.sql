------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PeopleInMovies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PeopleInMovies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PeopleInMovies](
		[PersonInMovieId] [int] IDENTITY(1,1) NOT NULL,
		[PersonId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
	 CONSTRAINT [PK_PeopleInMovies] PRIMARY KEY CLUSTERED 
	(
		[PersonInMovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_PeopleInMovies] UNIQUE NONCLUSTERED 
	(
		[PersonId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PeopleInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[PeopleInMovies]'))
BEGIN
	ALTER TABLE [dbo].[PeopleInMovies] CHECK CONSTRAINT [FK_PeopleInMovies_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PeopleInMovies_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[PeopleInMovies]'))
BEGIN
	ALTER TABLE [dbo].[PeopleInMovies] CHECK CONSTRAINT [FK_PeopleInMovies_People]
END 

GO

------------------------------------- 
