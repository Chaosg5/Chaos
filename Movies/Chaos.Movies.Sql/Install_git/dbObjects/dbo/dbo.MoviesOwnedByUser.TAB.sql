------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesOwnedByUser 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUser]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MoviesOwnedByUser](
		[MoviesOwnedByUserId] [int] IDENTITY(1,1) NOT NULL,
		[MovieId] [int] NOT NULL,
		[UserId] [int] NOT NULL,
		[DVD] [bit] NOT NULL,
		[BD] [bit] NOT NULL,
		[Digital] [bit] NOT NULL,
	 CONSTRAINT [PK_MoviesOwnedByUser] PRIMARY KEY CLUSTERED 
	(
		[MoviesOwnedByUserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_MoviesOwnedByUser] UNIQUE NONCLUSTERED 
	(
		[MovieId] ASC,
		[UserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesOwnedByUser_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesOwnedByUser] CHECK CONSTRAINT [FK_MoviesOwnedByUser_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesOwnedByUser_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesOwnedByUser] CHECK CONSTRAINT [FK_MoviesOwnedByUser_Users]
END 

GO

------------------------------------- 
