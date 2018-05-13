------------------------------------------------------------ 
------------------------------------------------------------ 
--- EyeDoc SQL script- CMDB                                
--- 2018-05-11 09:32                                            
------------------------------------------------------------ 
--- Name    : CMDB 
--- Tag     : 
--- Version : 
--- Datum   : 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create database ------------------------------- 
------------------------------------------------------------ 
USE master                                                   
GO                                                           
IF not EXISTS (SELECT name FROM dbo.sysdatabases WHERE name = N'CMDB')
begin                                                        
	CREATE DATABASE CMDB collate Finnish_Swedish_CI_AS          
	print 'database CMDB is created'                            
end                                                          
GO                                                           
------------ Backup Database ------------------------------- 
------------------------------------------------------------ 
/*                                                           
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'CMDB') 
begin                                                        
	-- Edit path                                              
   DECLARE @path VARCHAR(256) SET @path = 'C:\dbBackup\'     
   --                                                        
   DECLARE @dbname VARCHAR(50)                               
   SET @dbname = 'CMDB'                                
   SET @path = @path + @dbname +'_' + CONVERT(VARCHAR(20),GETDATE(),112) + '.BAK'  
	BACKUP DATABASE @dbname TO DISK = @path                   
end                                                          
GO                                                           
*/                                                           
------------------------------------------------------------ 
------------------------------------------------------------ 
USE CMDB                                                       
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create roles ---------------------------------- 
------------------------------------------------------------ 
if (user_id('rCMDB') is null)                                  
begin                                                        
	exec sp_addrole 'rCMDB'                                     
	print 'role rCMDB is created'                               
end                                                          
GO                                                           
------------------------------------------------------------ 
/*                                                           
CREATE LOGIN DDM_web WITH PASSWORD = 'ange lösenord'         
create user DDM_web from login DDM_web                       
EXEC sp_addrolemember 'rCMDB', 'DDM_web'                       
*/                                                           
------------------------------------------------------------ 
------------------------------------------------------------ 
-- dbo.CMDB.sql 
------------------------------------------------------------ 
------------------------------------------------------------ 
--- EyeDoc SQL script- CMDB.dbo                       
------------------------------------------------------------ 
--- Scheme    : dbo 
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
-- dbo.Acquisitions.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Acquisitions 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Acquisitions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Acquisitions](
		[AcquisitionId] [int] IDENTITY(1,1) NOT NULL,
		[MoviesOwnedByUserId] [int] NOT NULL,
		[AcquiredAt] [date] NULL,
		[DateUncertain] [bit] NOT NULL,
		[VendorId] [int] NULL,
		[Price] [float] NOT NULL,
	 CONSTRAINT [PK_Acquisitions] PRIMARY KEY CLUSTERED 
	(
		[AcquisitionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Acquisitions_DateUncertain]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[Acquisitions] ADD  CONSTRAINT [DF_Acquisitions_DateUncertain]  DEFAULT ((0)) FOR [DateUncertain]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Acquisitions_MoviesOwnedByUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[Acquisitions]'))
BEGIN
	ALTER TABLE [dbo].[Acquisitions] CHECK CONSTRAINT [FK_Acquisitions_MoviesOwnedByUser]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Acquisitions_Vendors]') AND parent_object_id = OBJECT_ID(N'[dbo].[Acquisitions]'))
BEGIN
	ALTER TABLE [dbo].[Acquisitions] CHECK CONSTRAINT [FK_Acquisitions_Vendors]
END 

GO

------------------------------------- 
-- dbo.ActorsAsCharacters.TAB.sql 
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
-- dbo.BoxOffice.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : BoxOffice 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BoxOffice]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[BoxOffice](
		[MovieId] [int] NOT NULL,
		[BoxOfficeTypeId] [int] NOT NULL,
		[USD] [bigint] NOT NULL,
	 CONSTRAINT [PK_BoxOffice] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[BoxOfficeTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOffice_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOffice]'))
BEGIN
	ALTER TABLE [dbo].[BoxOffice] CHECK CONSTRAINT [FK_BoxOffice_BoxOfficeTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOffice_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOffice]'))
BEGIN
	ALTER TABLE [dbo].[BoxOffice] CHECK CONSTRAINT [FK_BoxOffice_Movies]
END 

GO

------------------------------------- 
-- dbo.BoxOfficeTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : BoxOfficeTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[BoxOfficeTypes](
		[BoxOfficeTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_BoxOfficeTypes] PRIMARY KEY CLUSTERED 
	(
		[BoxOfficeTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.BoxOfficeTypeTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : BoxOfficeTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[BoxOfficeTypeTitles](
		[BoxOfficeTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_BoxOfficeTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[BoxOfficeTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOfficeTypeTitles_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[BoxOfficeTypeTitles] CHECK CONSTRAINT [FK_BoxOfficeTypeTitles_BoxOfficeTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOfficeTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[BoxOfficeTypeTitles] CHECK CONSTRAINT [FK_BoxOfficeTypeTitles_Languages]
END 

GO

------------------------------------- 
-- dbo.CharacterExternalLookup.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharacterExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[CharacterExternalLookup](
		[CharacterId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_CharacterExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[CharacterId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharacterExternalLookup_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[CharacterExternalLookup] CHECK CONSTRAINT [FK_CharacterExternalLookup_Characters]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharacterExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[CharacterExternalLookup] CHECK CONSTRAINT [FK_CharacterExternalLookup_ExternalSources]
END 

GO

------------------------------------- 
-- dbo.Characters.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Characters 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Characters]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Characters](
		[CharacterId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_Characters] PRIMARY KEY CLUSTERED 
	(
		[CharacterId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.CharactersInMovies.TAB.sql 
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
-- dbo.Departments.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Departments 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Departments](
		[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
	(
		[DepartmentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.DepartmentTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : DepartmentTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[DepartmentTitles](
		[DepartmentId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_DepartmentTitles] PRIMARY KEY CLUSTERED 
	(
		[DepartmentId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Departments]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Languages1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Languages1]
END 

GO

------------------------------------- 
-- dbo.EpisodeExternalLookup.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : EpisodeExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[EpisodeExternalLookup](
		[EpisodeId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_EpisodeExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[EpisodeId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeExternalLookup_Episodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeExternalLookup] CHECK CONSTRAINT [FK_EpisodeExternalLookup_Episodes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeExternalLookup] CHECK CONSTRAINT [FK_EpisodeExternalLookup_ExternalSources]
END 

GO

------------------------------------- 
-- dbo.Episodes.TAB.sql 
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
-- dbo.EpisodeTitles.TAB.sql 
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
-- dbo.Errors.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Errors 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Errors]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Errors](
		[ErrorId] [int] IDENTITY(1,1) NOT NULL,
		[UserId] [int] NOT NULL,
		[Time] [datetime2](7) NOT NULL,
		[Type] [nvarchar](255) NOT NULL,
		[Source] [nvarchar](255) NOT NULL,
		[TargetSite] [nvarchar](255) NOT NULL,
		[Message] [nvarchar](255) NOT NULL,
		[StackTrace] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_Exceptions] PRIMARY KEY CLUSTERED 
	(
		[ErrorId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Exceptions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Errors]'))
BEGIN
	ALTER TABLE [dbo].[Errors] CHECK CONSTRAINT [FK_Exceptions_Users]
END 

GO

------------------------------------- 
-- dbo.ExternalSourceMovieTypeAddress.TAB.sql 
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
-- dbo.ExternalSources.TAB.sql 
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
-- dbo.FavoriteTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : FavoriteTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[FavoriteTypes](
		[FavoriteTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_FavoriteTypes] PRIMARY KEY CLUSTERED 
	(
		[FavoriteTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.FavoriteTypeTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : FavoriteTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[FavoriteTypeTitles](
		[FavoriteTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_FavoriteTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[FavoriteTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FavoriteTypeTitles_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[FavoriteTypeTitles] CHECK CONSTRAINT [FK_FavoriteTypeTitles_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FavoriteTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[FavoriteTypeTitles] CHECK CONSTRAINT [FK_FavoriteTypeTitles_Languages]
END 

GO

------------------------------------- 
-- dbo.GenreExternalLookup.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GenreExternalLookup](
		[GenreId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_GenreExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[GenreId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[GenreExternalLookup] CHECK CONSTRAINT [FK_GenreExternalLookup_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreExternalLookup_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[GenreExternalLookup] CHECK CONSTRAINT [FK_GenreExternalLookup_Genres]
END 

GO

------------------------------------- 
-- dbo.Genres.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Genres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Genres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Genres](
		[GenreId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
	(
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.GenreTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GenreTitles](
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
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreTitles_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreTitles]'))
BEGIN
	ALTER TABLE [dbo].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreTitles]'))
BEGIN
	ALTER TABLE [dbo].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Languages]
END 

GO

------------------------------------- 
-- dbo.Hashtags.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Hashtags 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Hashtags]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Hashtags](
		[Hashtag] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Hashtags] PRIMARY KEY CLUSTERED 
	(
		[Hashtag] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.Icons.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Icons 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Icons]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Icons](
		[IconId] [int] IDENTITY(1,1) NOT NULL,
		[IconTypeId] [int] NOT NULL,
		[IconUrl] [nvarchar](500) NULL,
		[DataSize] [int] NULL,
		[Data] [varbinary](max) NULL,
	 CONSTRAINT [PK_Icons] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Icons_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Icons]'))
BEGIN
	ALTER TABLE [dbo].[Icons] CHECK CONSTRAINT [FK_Icons_IconTypes]
END 

GO

------------------------------------- 
-- dbo.IconsInCharacters.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInCharacters 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInCharacters](
		[IconId] [int] NOT NULL,
		[CharacterId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInCharacters] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[CharacterId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCharacters_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]'))
BEGIN
	ALTER TABLE [dbo].[IconsInCharacters] CHECK CONSTRAINT [FK_IconsInCharacters_Characters]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCharacters_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]'))
BEGIN
	ALTER TABLE [dbo].[IconsInCharacters] CHECK CONSTRAINT [FK_IconsInCharacters_Icons]
END 

GO

------------------------------------- 
-- dbo.IconsInMovies.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInMovies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInMovies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInMovies](
		[IconId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInMovies] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies] CHECK CONSTRAINT [FK_IconsInMovies_Icons]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies] CHECK CONSTRAINT [FK_IconsInMovies_Movies]
END 

GO

------------------------------------- 
-- dbo.IconsInMovieSeries.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInMovieSeries 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInMovieSeries](
		[IconId] [int] NOT NULL,
		[MovieSeriesId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInCollections] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[MovieSeriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries] CHECK CONSTRAINT [FK_IconsInCollections_Collections]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries] CHECK CONSTRAINT [FK_IconsInCollections_Icons]
END 

GO

------------------------------------- 
-- dbo.IconsInPeople.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInPeople 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInPeople]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInPeople](
		[IconId] [int] NOT NULL,
		[PersonId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInPeople] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[PersonId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInPeople_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInPeople]'))
BEGIN
	ALTER TABLE [dbo].[IconsInPeople] CHECK CONSTRAINT [FK_IconsInPeople_Icons]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInPeople_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInPeople]'))
BEGIN
	ALTER TABLE [dbo].[IconsInPeople] CHECK CONSTRAINT [FK_IconsInPeople_People]
END 

GO

------------------------------------- 
-- dbo.IconTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconTypes](
		[IconTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_IconTypes] PRIMARY KEY CLUSTERED 
	(
		[IconTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.IconTypeTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconTypeTitles](
		[IconTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_IconTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[IconTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconTypeTitles_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[IconTypeTitles] CHECK CONSTRAINT [FK_IconTypeTitles_IconTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[IconTypeTitles] CHECK CONSTRAINT [FK_IconTypeTitles_Languages]
END 

GO

------------------------------------- 
-- dbo.Languages.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Languages 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Languages]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Languages](
		[Language] [varchar](8) NOT NULL,
	 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
	(
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.LanguageTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LanguageTitles](
		[Language] [varchar](8) NOT NULL,
		[InLanguage] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_LanguageTitles] PRIMARY KEY CLUSTERED 
	(
		[Language] ASC,
		[InLanguage] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LanguageTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[LanguageTitles]'))
BEGIN
	ALTER TABLE [dbo].[LanguageTitles] CHECK CONSTRAINT [FK_LanguageTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LanguageTitles_LanguagesIn]') AND parent_object_id = OBJECT_ID(N'[dbo].[LanguageTitles]'))
BEGIN
	ALTER TABLE [dbo].[LanguageTitles] CHECK CONSTRAINT [FK_LanguageTitles_LanguagesIn]
END 

GO

------------------------------------- 
-- dbo.MovieExternalLookup.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieExternalLookup](
		[MovieId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_MovieExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup] CHECK CONSTRAINT [FK_MovieExternalLookup_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup] CHECK CONSTRAINT [FK_MovieExternalLookup_Movies]
END 

GO

------------------------------------- 
-- dbo.MovieExternalRatings.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieExternalRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieExternalRatings](
		[MovieId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalRating] [float] NOT NULL,
		[RatingCount] [int] NOT NULL,
	 CONSTRAINT [PK_MovieExternalRating] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_MovieExternalRatings_ExternalRating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] ADD  CONSTRAINT [DF_MovieExternalRatings_ExternalRating]  DEFAULT ((0)) FOR [ExternalRating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalRatings_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [FK_MovieExternalRatings_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalRatings_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [FK_MovieExternalRatings_Movies]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_MovieExternalRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD  CONSTRAINT [CK_MovieExternalRatings_Rating] CHECK  (([ExternalRating]>=(0) AND [ExternalRating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_MovieExternalRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [CK_MovieExternalRatings_Rating]
END 

GO

------------------------------------- 
-- dbo.Movies.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Movies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Movies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Movies](
		[MovieId] [int] IDENTITY(1,1) NOT NULL,
		[MovieTypeId] [int] NOT NULL,
		[Year] [date] NOT NULL,
		[EndYear] [date] NULL,
		[RunTime] [int] NOT NULL,
	 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Movies_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Movies]'))
BEGIN
	ALTER TABLE [dbo].[Movies] CHECK CONSTRAINT [FK_Movies_MovieTypes]
END 

GO

------------------------------------- 
-- dbo.MovieSeries.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeries 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeries]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieSeries](
		[MovieSeriesId] [int] IDENTITY(1,1) NOT NULL,
		[MovieSeriesTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_Collections] PRIMARY KEY CLUSTERED 
	(
		[MovieSeriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Collections_CollectionTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeries] CHECK CONSTRAINT [FK_Collections_CollectionTypes]
END 

GO

------------------------------------- 
-- dbo.MovieSeriesTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieSeriesTitles](
		[MovieSeriesId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_CollectionTitles] PRIMARY KEY CLUSTERED 
	(
		[MovieSeriesId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionTitles_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_CollectionTitles_Collections]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_MovieSeriesTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTitles_MovieSeriesTitles]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_MovieSeriesTitles_MovieSeriesTitles]
END 

GO

------------------------------------- 
-- dbo.MovieSeriesTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieSeriesTypes](
		[MovieSeriesTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_CollectionTypes] PRIMARY KEY CLUSTERED 
	(
		[MovieSeriesTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.MovieSeriesTypeTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieSeriesTypeTitles](
		[MovieSeriesTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_MovieSeriesTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[MovieSeriesTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTypeTitles] CHECK CONSTRAINT [FK_MovieSeriesTypeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTypeTitles_MovieSeriesTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTypeTitles] CHECK CONSTRAINT [FK_MovieSeriesTypeTitles_MovieSeriesTypes]
END 

GO

------------------------------------- 
-- dbo.MoviesInGenres.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MoviesInGenres](
		[MovieId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_MoviesInGenres] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInGenres] CHECK CONSTRAINT [FK_MoviesInGenres_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInGenres_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInGenres] CHECK CONSTRAINT [FK_MoviesInGenres_Movies]
END 

GO

------------------------------------- 
-- dbo.MoviesInMovieSeries.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesInMovieSeries 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MoviesInMovieSeries](
		[MovieId] [int] NOT NULL,
		[MovieSeriesId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_MoviesInCollection] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[MovieSeriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInCollection_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInMovieSeries] CHECK CONSTRAINT [FK_MoviesInCollection_Collections]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInCollection_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInMovieSeries] CHECK CONSTRAINT [FK_MoviesInCollection_Movies]
END 

GO

------------------------------------- 
-- dbo.MoviesOwnedByUser.TAB.sql 
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
-- dbo.MoviesWatchedByUser.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesWatchedByUser 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MoviesWatchedByUser](
		[MoviesWatchedByUserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[UserId] [int] NOT NULL,
		[WatchedDate] [date] NOT NULL,
		[DateUncertain] [bit] NOT NULL,
		[WatchTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_MoviesWatchedByUser] PRIMARY KEY CLUSTERED 
	(
		[MoviesWatchedByUserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_MoviesWatchedByUser_DateUncertain]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] ADD  CONSTRAINT [DF_MoviesWatchedByUser_DateUncertain]  DEFAULT ((0)) FOR [DateUncertain]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_Users]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_WatchTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_WatchTypes]
END 

GO

------------------------------------- 
-- dbo.MovieTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieTitles](
		[MovieId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_MovieTitle] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitle_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles] CHECK CONSTRAINT [FK_MovieTitle_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles] CHECK CONSTRAINT [FK_MovieTitles_Languages]
END 

GO

------------------------------------- 
-- dbo.MovieTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieTypes](
		[MovieTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_MovieTypes] PRIMARY KEY CLUSTERED 
	(
		[MovieTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.MovieTypeTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieTypeTitles](
		[MovieTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_MovieTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[MovieTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTypeTitles] CHECK CONSTRAINT [FK_MovieTypeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTypeTitles_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTypeTitles] CHECK CONSTRAINT [FK_MovieTypeTitles_MovieTypes]
END 

GO

------------------------------------- 
-- dbo.People.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : People 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[People]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[People](
		[PersonId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
	(
		[PersonId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.PeopleInMovies.TAB.sql 
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
-- dbo.PersonExternalLookup.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PersonExternalLookup](
		[PersonId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_PersonExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[PersonId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[PersonExternalLookup] CHECK CONSTRAINT [FK_PersonExternalLookup_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonExternalLookup_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[PersonExternalLookup] CHECK CONSTRAINT [FK_PersonExternalLookup_People]
END 

GO

------------------------------------- 
-- dbo.PersonInMovieRoles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonInMovieRoles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PersonInMovieRoles](
		[PersonInMovieRoleId] [int] IDENTITY(1,1) NOT NULL,
		[PersonInMovieId] [int] NOT NULL,
		[RoleInDepartmentId] [int] NOT NULL,
	 CONSTRAINT [PK_PersonInMovieRoles] PRIMARY KEY CLUSTERED 
	(
		[PersonInMovieRoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_PersonInMovieRoles] UNIQUE NONCLUSTERED 
	(
		[PersonInMovieId] ASC,
		[RoleInDepartmentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonInMovieRoles_PeopleInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]'))
BEGIN
	ALTER TABLE [dbo].[PersonInMovieRoles] CHECK CONSTRAINT [FK_PersonInMovieRoles_PeopleInMovies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonInMovieRoles_RolesInDepartments]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]'))
BEGIN
	ALTER TABLE [dbo].[PersonInMovieRoles] CHECK CONSTRAINT [FK_PersonInMovieRoles_RolesInDepartments]
END 

GO

------------------------------------- 
-- dbo.RatingSystems.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystems 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystems]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingSystems](
		[RatingSystemId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_RatingSystems] PRIMARY KEY CLUSTERED 
	(
		[RatingSystemId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.RatingSystemTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingSystemTitles](
		[RatingSystemId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](500) NULL,
	 CONSTRAINT [PK_RatingSystemTitles] PRIMARY KEY CLUSTERED 
	(
		[RatingSystemId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemTitles] CHECK CONSTRAINT [FK_RatingSystemTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemTitles_RatingSystems]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemTitles] CHECK CONSTRAINT [FK_RatingSystemTitles_RatingSystems]
END 

GO

------------------------------------- 
-- dbo.RatingSystemValues.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemValues 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingSystemValues](
		[RatingSystemId] [int] NOT NULL,
		[RatingTypeId] [int] NOT NULL,
		[Weight] [tinyint] NOT NULL,
	 CONSTRAINT [PK_RatingSystemValues] PRIMARY KEY CLUSTERED 
	(
		[RatingSystemId] ASC,
		[RatingTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemValues_RatingSystems]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemValues] CHECK CONSTRAINT [FK_RatingSystemValues_RatingSystems]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemValues_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemValues] CHECK CONSTRAINT [FK_RatingSystemValues_RatingTypes]
END 

GO

------------------------------------- 
-- dbo.RatingTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingTypes](
		[RatingTypeId] [int] IDENTITY(1,1) NOT NULL,
		[ParentRatingTypeId] [int] NULL,
	 CONSTRAINT [PK_RatingTypes] PRIMARY KEY CLUSTERED 
	(
		[RatingTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypes_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypes]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypes] CHECK CONSTRAINT [FK_RatingTypes_RatingTypes]
END 

GO

------------------------------------- 
-- dbo.RatingTypeTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingTypeTitles](
		[RatingTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](500) NULL,
	 CONSTRAINT [PK_RatingTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[RatingTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypeTitles] CHECK CONSTRAINT [FK_RatingTypeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypeTitles_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypeTitles] CHECK CONSTRAINT [FK_RatingTypeTitles_RatingTypes]
END 

GO

------------------------------------- 
-- dbo.Roles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Roles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Roles](
		[RoleId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
	(
		[RoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.RolesInDepartments.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RolesInDepartments 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RolesInDepartments](
		[RoleInDepartmentId] [int] IDENTITY(1,1) NOT NULL,
		[RoleId] [int] NOT NULL,
		[DepartmentId] [int] NOT NULL,
	 CONSTRAINT [PK_RolesInDepartments] PRIMARY KEY CLUSTERED 
	(
		[RoleInDepartmentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_RolesInDepartments] UNIQUE NONCLUSTERED 
	(
		[DepartmentId] ASC,
		[RoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments] CHECK CONSTRAINT [FK_RolesInDepartments_Departments]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments] CHECK CONSTRAINT [FK_RolesInDepartments_Roles]
END 

GO

------------------------------------- 
-- dbo.RoleTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RoleTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RoleTitles](
		[RoleId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_RoleTitles] PRIMARY KEY CLUSTERED 
	(
		[RoleId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Roles]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Roles1]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Roles1]
END 

GO

------------------------------------- 
-- dbo.Seasons.TAB.sql 
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
-- dbo.UserCharacterFavorites.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserCharacterFavorites 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserCharacterFavorites](
		[UserId] [int] NOT NULL,
		[CharacterId] [int] NOT NULL,
		[FavoriteTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_UserCharactersFavorites] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[CharacterId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_Characters]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_Users]
END 

GO

------------------------------------- 
-- dbo.UserCharacterInMovieRatings.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserCharacterInMovieRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserCharacterInMovieRatings](
		[UserId] [int] NOT NULL,
		[CharacterInMovieId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK_UserCharacterInMovieRatings] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[CharacterInMovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserCharacterInMovieRatings_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] ADD  CONSTRAINT [DF_UserCharacterInMovieRatings_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharacterInMovieRatings_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [FK_UserCharacterInMovieRatings_CharactersInMovies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharacterInMovieRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [FK_UserCharacterInMovieRatings_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserCharacterInMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserCharacterInMovieRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserCharacterInMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [CK_UserCharacterInMovieRatings_Rating]
END 

GO

------------------------------------- 
-- dbo.UserMovieFavorites.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieFavorites 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieFavorites](
		[MovieId] [int] NOT NULL,
		[UserId] [int] NOT NULL,
		[FavoriteTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_UserMovieFavorites] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[UserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_Users]
END 

GO

------------------------------------- 
-- dbo.UserMovieGenres.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieGenres](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
	 CONSTRAINT [PK_UserMovieGenres] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserMovieGenres_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] ADD  CONSTRAINT [DF_UserMovieGenres_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieGenres_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieGenres_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieGenres_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [CK_UserMovieGenres_Rating]
END 

GO

------------------------------------- 
-- dbo.UserMovieHashtags.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieHashtags 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieHashtags](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[Hashtag] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_UserMovieHashtags] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC,
		[Hashtag] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Hashtags]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Hashtags]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Users]
END 

GO

------------------------------------- 
-- dbo.UserMovieRatings.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieRatings](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[RatingTypeId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK_UserMovieRatings] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC,
		[RatingTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_UserMovieRatings] UNIQUE NONCLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserMovieRatings_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] ADD  CONSTRAINT [DF_UserMovieRatings_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_RatingTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [CK_UserMovieRatings_Rating]
END 

GO

------------------------------------- 
-- dbo.UserMovieReviews.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieReviews 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieReviews](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
		[Title] [nvarchar](140) NOT NULL,
		[Review] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_UserMovieReviews] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews] CHECK CONSTRAINT [FK_UserMovieReviews_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews] CHECK CONSTRAINT [FK_UserMovieReviews_Users]
END 

GO

------------------------------------- 
-- dbo.UserMovieWishlist.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserMovieWishlist 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserMovieWishlist](
		[UserId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
	 CONSTRAINT [PK_UserMovieWishlist] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserMovieWishlist_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] ADD  CONSTRAINT [DF_UserMovieWishlist_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieWishlist_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [FK_UserMovieWishlist_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieWishlist_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [FK_UserMovieWishlist_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieWishlist_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieWishlist_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserMovieWishlist_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [CK_UserMovieWishlist_Rating]
END 

GO

------------------------------------- 
-- dbo.UserPeopleFavorites.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserPeopleFavorites 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserPeopleFavorites](
		[UserId] [int] NOT NULL,
		[PersonId] [int] NOT NULL,
		[FavoriteTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_UserPeopleFavorites] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[PersonId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_People]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_Users]
END 

GO

------------------------------------- 
-- dbo.UserPersonInMovieRoleRatings.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserPersonInMovieRoleRatings 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserPersonInMovieRoleRatings](
		[UserId] [int] NOT NULL,
		[PersonInMovieRoleId] [int] NOT NULL,
		[Rating] [tinyint] NOT NULL,
		[CreatedDate] [date] NOT NULL,
	 CONSTRAINT [PK_UserPersonInMovieRoleRatings] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[PersonInMovieRoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_UserPersonInMovieRoleRatings_Rating]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] ADD  CONSTRAINT [DF_UserPersonInMovieRoleRatings_Rating]  DEFAULT ((0)) FOR [Rating]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPersonInMovieRoleRatings_PersonInMovieRoles]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [FK_UserPersonInMovieRoleRatings_PersonInMovieRoles]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPersonInMovieRoleRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [FK_UserPersonInMovieRoleRatings_Users]
END 

GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserPersonInMovieRoleRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserPersonInMovieRoleRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
END 

GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_UserPersonInMovieRoleRatings_Rating]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [CK_UserPersonInMovieRoleRatings_Rating]
END 

GO

------------------------------------- 
-- dbo.Users.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Users 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Users](
		[UserId] [int] IDENTITY(1,1) NOT NULL,
		[Username] [nvarchar](50) NOT NULL,
		[Password] [nvarchar](250) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Email] [nvarchar](250) NOT NULL,
	 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_Users_Email] UNIQUE NONCLUSTERED 
	(
		[Email] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_Users_Username] UNIQUE NONCLUSTERED 
	(
		[Username] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.UserSessions.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSessions 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSessions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserSessions](
		[UserSessionId] [uniqueidentifier] NOT NULL,
		[ClientIp] [nvarchar](50) NOT NULL,
		[UserId] [int] NOT NULL,
		[ActiveFrom] [datetime2](0) NOT NULL,
		[ActiveTo] [datetime2](0) NOT NULL,
	 CONSTRAINT [PK_UserSession] PRIMARY KEY CLUSTERED 
	(
		[UserSessionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSession_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSessions]'))
BEGIN
	ALTER TABLE [dbo].[UserSessions] CHECK CONSTRAINT [FK_UserSession_Users]
END 

GO

------------------------------------- 
-- dbo.Vendors.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Vendors 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vendors]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Vendors](
		[VendorId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_Vendors] PRIMARY KEY CLUSTERED 
	(
		[VendorId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.VoiceOverActors.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : VoiceOverActors 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[VoiceOverActors](
		[PersonId] [int] NOT NULL,
		[CharacterInMovieId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
	 CONSTRAINT [PK_VoiceOverActors] PRIMARY KEY CLUSTERED 
	(
		[PersonId] ASC,
		[CharacterInMovieId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors] CHECK CONSTRAINT [FK_VoiceOverActors_CharactersInMovies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors] CHECK CONSTRAINT [FK_VoiceOverActors_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors] CHECK CONSTRAINT [FK_VoiceOverActors_People]
END 

GO

------------------------------------- 
-- dbo.WatchTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : WatchTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WatchTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[WatchTypes](
		[WatchTypeId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_WatchTypes] PRIMARY KEY CLUSTERED 
	(
		[WatchTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- dbo.WatchTypeTitles.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : WatchTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[WatchTypeTitles](
		[WatchTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_WatchTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[WatchTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WatchTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[WatchTypeTitles] CHECK CONSTRAINT [FK_WatchTypeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WatchTypeTitles_WatchTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[WatchTypeTitles] CHECK CONSTRAINT [FK_WatchTypeTitles_WatchTypes]
END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------ Update tables --------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create TYPE ----------------------------------- 
------------------------------------------------------------ 
-- dbo.ExternalLookupIdCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalLookupIdCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'ExternalLookupIdCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[ExternalLookupIdCollection] AS TABLE(
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[ExternalSourceId] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalLookupIdCollection] 
grant execute on type :: [dbo].[ExternalLookupIdCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.ExternalRatingCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalRatingCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'ExternalRatingCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[ExternalRatingCollection] AS TABLE(
		[ExternalSourceId] [int] NOT NULL,
		[ExternalRating] [float] NOT NULL,
		[RatingCount] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[ExternalSourceId] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalRatingCollection] 
grant execute on type :: [dbo].[ExternalRatingCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.GuidCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GuidCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'GuidCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[GuidCollection] AS TABLE(
		[Id] [uniqueidentifier] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[GuidCollection] 
grant execute on type :: [dbo].[GuidCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.IdCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IdCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'IdCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[IdCollection] AS TABLE(
		[Id] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IdCollection] 
grant execute on type :: [dbo].[IdCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.IdOrderCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IdOrderCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'IdOrderCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[IdOrderCollection] AS TABLE(
		[Id] [int] NOT NULL,
		[Order] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IdOrderCollection] 
grant execute on type :: [dbo].[IdOrderCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.LanguageDescriptionCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageDescriptionCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'LanguageDescriptionCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[LanguageDescriptionCollection] AS TABLE(
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](500) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Language] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageDescriptionCollection] 
grant execute on type :: [dbo].[LanguageDescriptionCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.LanguagesTitlesCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguagesTitlesCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'LanguagesTitlesCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[LanguagesTitlesCollection] AS TABLE(
		[InLanguage] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[InLanguage] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguagesTitlesCollection] 
grant execute on type :: [dbo].[LanguagesTitlesCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.LanguageTitleCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageTitleCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'LanguageTitleCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[LanguageTitleCollection] AS TABLE(
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[Language] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageTitleCollection] 
grant execute on type :: [dbo].[LanguageTitleCollection] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RatingSystemValueCollection.TYPE.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemValueCollection 
-- Type     : userdefinedtabletype 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'RatingSystemValueCollection' AND ss.name = N'dbo')
BEGIN
	CREATE TYPE [dbo].[RatingSystemValueCollection] AS TABLE(
		[RatingTypeId] [int] NOT NULL,
		[Weight] [tinyint] NOT NULL,
		PRIMARY KEY CLUSTERED 
	(
		[RatingTypeId] ASC
	)WITH (IGNORE_DUP_KEY = OFF)
	)
END 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemValueCollection] 
grant execute on type :: [dbo].[RatingSystemValueCollection] TO [rCMDB] 
GO

------------------------------------- 
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
-- dbo.Acquisitions.FK_Acquisitions_MoviesOwnedByUser.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Acquisitions 
-- name     : FK_Acquisitions_MoviesOwnedByUser 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Acquisitions_MoviesOwnedByUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[Acquisitions]'))
BEGIN
	ALTER TABLE [dbo].[Acquisitions]  WITH CHECK ADD CONSTRAINT [FK_Acquisitions_MoviesOwnedByUser] FOREIGN KEY([MoviesOwnedByUserId])
	REFERENCES [dbo].[MoviesOwnedByUser] ([MoviesOwnedByUserId])

END 

GO

------------------------------------- 
-- dbo.Acquisitions.FK_Acquisitions_Vendors.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Acquisitions 
-- name     : FK_Acquisitions_Vendors 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Acquisitions_Vendors]') AND parent_object_id = OBJECT_ID(N'[dbo].[Acquisitions]'))
BEGIN
	ALTER TABLE [dbo].[Acquisitions]  WITH CHECK ADD CONSTRAINT [FK_Acquisitions_Vendors] FOREIGN KEY([VendorId])
	REFERENCES [dbo].[Vendors] ([VendorId])

END 

GO

------------------------------------- 
-- dbo.ActorsAsCharacters.FK_ActorsAsCharacters_CharactersInMovies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : ActorsAsCharacters 
-- name     : FK_ActorsAsCharacters_CharactersInMovies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActorsAsCharacters_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActorsAsCharacters]'))
BEGIN
	ALTER TABLE [dbo].[ActorsAsCharacters]  WITH CHECK ADD CONSTRAINT [FK_ActorsAsCharacters_CharactersInMovies] FOREIGN KEY([CharacterInMovieId])
	REFERENCES [dbo].[CharactersInMovies] ([CharacterInMovieId])

END 

GO

------------------------------------- 
-- dbo.ActorsAsCharacters.FK_ActorsAsCharacters_People.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : ActorsAsCharacters 
-- name     : FK_ActorsAsCharacters_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActorsAsCharacters_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActorsAsCharacters]'))
BEGIN
	ALTER TABLE [dbo].[ActorsAsCharacters]  WITH CHECK ADD CONSTRAINT [FK_ActorsAsCharacters_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
-- dbo.BoxOffice.FK_BoxOffice_BoxOfficeTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : BoxOffice 
-- name     : FK_BoxOffice_BoxOfficeTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOffice_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOffice]'))
BEGIN
	ALTER TABLE [dbo].[BoxOffice]  WITH CHECK ADD CONSTRAINT [FK_BoxOffice_BoxOfficeTypes] FOREIGN KEY([BoxOfficeTypeId])
	REFERENCES [dbo].[BoxOfficeTypes] ([BoxOfficeTypeId])

END 

GO

------------------------------------- 
-- dbo.BoxOffice.FK_BoxOffice_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : BoxOffice 
-- name     : FK_BoxOffice_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOffice_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOffice]'))
BEGIN
	ALTER TABLE [dbo].[BoxOffice]  WITH CHECK ADD CONSTRAINT [FK_BoxOffice_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.BoxOfficeTypeTitles.FK_BoxOfficeTypeTitles_BoxOfficeTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : BoxOfficeTypeTitles 
-- name     : FK_BoxOfficeTypeTitles_BoxOfficeTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOfficeTypeTitles_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[BoxOfficeTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_BoxOfficeTypeTitles_BoxOfficeTypes] FOREIGN KEY([BoxOfficeTypeId])
	REFERENCES [dbo].[BoxOfficeTypes] ([BoxOfficeTypeId])

END 

GO

------------------------------------- 
-- dbo.BoxOfficeTypeTitles.FK_BoxOfficeTypeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : BoxOfficeTypeTitles 
-- name     : FK_BoxOfficeTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOfficeTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[BoxOfficeTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_BoxOfficeTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.CharacterExternalLookup.FK_CharacterExternalLookup_Characters.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : CharacterExternalLookup 
-- name     : FK_CharacterExternalLookup_Characters 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharacterExternalLookup_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[CharacterExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_CharacterExternalLookup_Characters] FOREIGN KEY([CharacterId])
	REFERENCES [dbo].[Characters] ([CharacterId])

END 

GO

------------------------------------- 
-- dbo.CharacterExternalLookup.FK_CharacterExternalLookup_ExternalSources.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : CharacterExternalLookup 
-- name     : FK_CharacterExternalLookup_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharacterExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[CharacterExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_CharacterExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
-- dbo.CharactersInMovies.FK_CharactersInMovies_Characters.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : CharactersInMovies 
-- name     : FK_CharactersInMovies_Characters 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharactersInMovies_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharactersInMovies]'))
BEGIN
	ALTER TABLE [dbo].[CharactersInMovies]  WITH CHECK ADD CONSTRAINT [FK_CharactersInMovies_Characters] FOREIGN KEY([CharacterId])
	REFERENCES [dbo].[Characters] ([CharacterId])

END 

GO

------------------------------------- 
-- dbo.CharactersInMovies.FK_CharactersInMovies_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : CharactersInMovies 
-- name     : FK_CharactersInMovies_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharactersInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharactersInMovies]'))
BEGIN
	ALTER TABLE [dbo].[CharactersInMovies]  WITH CHECK ADD CONSTRAINT [FK_CharactersInMovies_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.DepartmentTitles.FK_DepartmentTitles_Departments.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : DepartmentTitles 
-- name     : FK_DepartmentTitles_Departments 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles]  WITH CHECK ADD CONSTRAINT [FK_DepartmentTitles_Departments] FOREIGN KEY([DepartmentId])
	REFERENCES [dbo].[Departments] ([DepartmentId])

END 

GO

------------------------------------- 
-- dbo.DepartmentTitles.FK_DepartmentTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : DepartmentTitles 
-- name     : FK_DepartmentTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles]  WITH CHECK ADD CONSTRAINT [FK_DepartmentTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.DepartmentTitles.FK_DepartmentTitles_Languages1.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : DepartmentTitles 
-- name     : FK_DepartmentTitles_Languages1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Languages1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles]  WITH CHECK ADD CONSTRAINT [FK_DepartmentTitles_Languages1] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.EpisodeExternalLookup.FK_EpisodeExternalLookup_Episodes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : EpisodeExternalLookup 
-- name     : FK_EpisodeExternalLookup_Episodes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeExternalLookup_Episodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_EpisodeExternalLookup_Episodes] FOREIGN KEY([EpisodeId])
	REFERENCES [dbo].[Episodes] ([EpisodeId])

END 

GO

------------------------------------- 
-- dbo.EpisodeExternalLookup.FK_EpisodeExternalLookup_ExternalSources.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : EpisodeExternalLookup 
-- name     : FK_EpisodeExternalLookup_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_EpisodeExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
-- dbo.Episodes.FK_Episodes_Seasons.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Episodes 
-- name     : FK_Episodes_Seasons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Episodes_Seasons]') AND parent_object_id = OBJECT_ID(N'[dbo].[Episodes]'))
BEGIN
	ALTER TABLE [dbo].[Episodes]  WITH CHECK ADD CONSTRAINT [FK_Episodes_Seasons] FOREIGN KEY([SeasonId])
	REFERENCES [dbo].[Seasons] ([SeasonId])

END 

GO

------------------------------------- 
-- dbo.EpisodeTitles.FK_EpisodeTitles_Episodes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : EpisodeTitles 
-- name     : FK_EpisodeTitles_Episodes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeTitles_Episodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeTitles]  WITH CHECK ADD CONSTRAINT [FK_EpisodeTitles_Episodes] FOREIGN KEY([EpisodeId])
	REFERENCES [dbo].[Episodes] ([EpisodeId])

END 

GO

------------------------------------- 
-- dbo.EpisodeTitles.FK_EpisodeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : EpisodeTitles 
-- name     : FK_EpisodeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeTitles]  WITH CHECK ADD CONSTRAINT [FK_EpisodeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.EpisodeTitles.FK_EpisodeTitles_Languages1.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : EpisodeTitles 
-- name     : FK_EpisodeTitles_Languages1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EpisodeTitles_Languages1]') AND parent_object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]'))
BEGIN
	ALTER TABLE [dbo].[EpisodeTitles]  WITH CHECK ADD CONSTRAINT [FK_EpisodeTitles_Languages1] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.Errors.FK_Exceptions_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Errors 
-- name     : FK_Exceptions_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Exceptions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Errors]'))
BEGIN
	ALTER TABLE [dbo].[Errors]  WITH CHECK ADD CONSTRAINT [FK_Exceptions_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.ExternalSourceMovieTypeAddress.FK_ExternalSourceMovieTypeAddress_ExternalSources.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : ExternalSourceMovieTypeAddress 
-- name     : FK_ExternalSourceMovieTypeAddress_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExternalSourceMovieTypeAddress_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExternalSourceMovieTypeAddress]'))
BEGIN
	ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress]  WITH CHECK ADD CONSTRAINT [FK_ExternalSourceMovieTypeAddress_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
-- dbo.ExternalSourceMovieTypeAddress.FK_ExternalSourceMovieTypeAddress_MovieTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : ExternalSourceMovieTypeAddress 
-- name     : FK_ExternalSourceMovieTypeAddress_MovieTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExternalSourceMovieTypeAddress_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExternalSourceMovieTypeAddress]'))
BEGIN
	ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress]  WITH CHECK ADD CONSTRAINT [FK_ExternalSourceMovieTypeAddress_MovieTypes] FOREIGN KEY([MovieTypeId])
	REFERENCES [dbo].[MovieTypes] ([MovieTypeId])

END 

GO

------------------------------------- 
-- dbo.FavoriteTypeTitles.FK_FavoriteTypeTitles_FavoriteTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : FavoriteTypeTitles 
-- name     : FK_FavoriteTypeTitles_FavoriteTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FavoriteTypeTitles_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[FavoriteTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_FavoriteTypeTitles_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
	REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])

END 

GO

------------------------------------- 
-- dbo.FavoriteTypeTitles.FK_FavoriteTypeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : FavoriteTypeTitles 
-- name     : FK_FavoriteTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FavoriteTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[FavoriteTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_FavoriteTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.GenreExternalLookup.FK_GenreExternalLookup_ExternalSources.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : GenreExternalLookup 
-- name     : FK_GenreExternalLookup_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[GenreExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_GenreExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
-- dbo.GenreExternalLookup.FK_GenreExternalLookup_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : GenreExternalLookup 
-- name     : FK_GenreExternalLookup_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreExternalLookup_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[GenreExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_GenreExternalLookup_Genres] FOREIGN KEY([GenreId])
	REFERENCES [dbo].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- dbo.GenreTitles.FK_GenreTitles_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : GenreTitles 
-- name     : FK_GenreTitles_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreTitles_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreTitles]'))
BEGIN
	ALTER TABLE [dbo].[GenreTitles]  WITH CHECK ADD CONSTRAINT [FK_GenreTitles_Genres] FOREIGN KEY([GenreId])
	REFERENCES [dbo].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- dbo.GenreTitles.FK_GenreTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : GenreTitles 
-- name     : FK_GenreTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreTitles]'))
BEGIN
	ALTER TABLE [dbo].[GenreTitles]  WITH CHECK ADD CONSTRAINT [FK_GenreTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.Icons.FK_Icons_IconTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Icons 
-- name     : FK_Icons_IconTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Icons_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Icons]'))
BEGIN
	ALTER TABLE [dbo].[Icons]  WITH CHECK ADD CONSTRAINT [FK_Icons_IconTypes] FOREIGN KEY([IconTypeId])
	REFERENCES [dbo].[IconTypes] ([IconTypeId])

END 

GO

------------------------------------- 
-- dbo.IconsInCharacters.FK_IconsInCharacters_Characters.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInCharacters 
-- name     : FK_IconsInCharacters_Characters 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCharacters_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]'))
BEGIN
	ALTER TABLE [dbo].[IconsInCharacters]  WITH CHECK ADD CONSTRAINT [FK_IconsInCharacters_Characters] FOREIGN KEY([CharacterId])
	REFERENCES [dbo].[Characters] ([CharacterId])

END 

GO

------------------------------------- 
-- dbo.IconsInCharacters.FK_IconsInCharacters_Icons.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInCharacters 
-- name     : FK_IconsInCharacters_Icons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCharacters_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]'))
BEGIN
	ALTER TABLE [dbo].[IconsInCharacters]  WITH CHECK ADD CONSTRAINT [FK_IconsInCharacters_Icons] FOREIGN KEY([IconId])
	REFERENCES [dbo].[Icons] ([IconId])

END 

GO

------------------------------------- 
-- dbo.IconsInMovies.FK_IconsInMovies_Icons.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovies 
-- name     : FK_IconsInMovies_Icons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies]  WITH CHECK ADD CONSTRAINT [FK_IconsInMovies_Icons] FOREIGN KEY([IconId])
	REFERENCES [dbo].[Icons] ([IconId])

END 

GO

------------------------------------- 
-- dbo.IconsInMovies.FK_IconsInMovies_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovies 
-- name     : FK_IconsInMovies_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies]  WITH CHECK ADD CONSTRAINT [FK_IconsInMovies_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.IconsInMovieSeries.FK_IconsInCollections_Collections.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovieSeries 
-- name     : FK_IconsInCollections_Collections 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries]  WITH CHECK ADD CONSTRAINT [FK_IconsInCollections_Collections] FOREIGN KEY([MovieSeriesId])
	REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])

END 

GO

------------------------------------- 
-- dbo.IconsInMovieSeries.FK_IconsInCollections_Icons.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovieSeries 
-- name     : FK_IconsInCollections_Icons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCollections_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovieSeries]  WITH CHECK ADD CONSTRAINT [FK_IconsInCollections_Icons] FOREIGN KEY([IconId])
	REFERENCES [dbo].[Icons] ([IconId])

END 

GO

------------------------------------- 
-- dbo.IconsInPeople.FK_IconsInPeople_Icons.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInPeople 
-- name     : FK_IconsInPeople_Icons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInPeople_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInPeople]'))
BEGIN
	ALTER TABLE [dbo].[IconsInPeople]  WITH CHECK ADD CONSTRAINT [FK_IconsInPeople_Icons] FOREIGN KEY([IconId])
	REFERENCES [dbo].[Icons] ([IconId])

END 

GO

------------------------------------- 
-- dbo.IconsInPeople.FK_IconsInPeople_People.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInPeople 
-- name     : FK_IconsInPeople_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInPeople_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInPeople]'))
BEGIN
	ALTER TABLE [dbo].[IconsInPeople]  WITH CHECK ADD CONSTRAINT [FK_IconsInPeople_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
-- dbo.IconTypeTitles.FK_IconTypeTitles_IconTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconTypeTitles 
-- name     : FK_IconTypeTitles_IconTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconTypeTitles_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[IconTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_IconTypeTitles_IconTypes] FOREIGN KEY([IconTypeId])
	REFERENCES [dbo].[IconTypes] ([IconTypeId])

END 

GO

------------------------------------- 
-- dbo.IconTypeTitles.FK_IconTypeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconTypeTitles 
-- name     : FK_IconTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[IconTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_IconTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.LanguageTitles.FK_LanguageTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : LanguageTitles 
-- name     : FK_LanguageTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LanguageTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[LanguageTitles]'))
BEGIN
	ALTER TABLE [dbo].[LanguageTitles]  WITH CHECK ADD CONSTRAINT [FK_LanguageTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.LanguageTitles.FK_LanguageTitles_LanguagesIn.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : LanguageTitles 
-- name     : FK_LanguageTitles_LanguagesIn 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LanguageTitles_LanguagesIn]') AND parent_object_id = OBJECT_ID(N'[dbo].[LanguageTitles]'))
BEGIN
	ALTER TABLE [dbo].[LanguageTitles]  WITH CHECK ADD CONSTRAINT [FK_LanguageTitles_LanguagesIn] FOREIGN KEY([InLanguage])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.MovieExternalLookup.FK_MovieExternalLookup_ExternalSources.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieExternalLookup 
-- name     : FK_MovieExternalLookup_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_MovieExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
-- dbo.MovieExternalLookup.FK_MovieExternalLookup_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieExternalLookup 
-- name     : FK_MovieExternalLookup_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_MovieExternalLookup_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.MovieExternalRatings.FK_MovieExternalRatings_ExternalSources.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieExternalRatings 
-- name     : FK_MovieExternalRatings_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalRatings_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD CONSTRAINT [FK_MovieExternalRatings_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
-- dbo.MovieExternalRatings.FK_MovieExternalRatings_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieExternalRatings 
-- name     : FK_MovieExternalRatings_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalRatings_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalRatings]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD CONSTRAINT [FK_MovieExternalRatings_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.Movies.FK_Movies_MovieTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Movies 
-- name     : FK_Movies_MovieTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Movies_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Movies]'))
BEGIN
	ALTER TABLE [dbo].[Movies]  WITH CHECK ADD CONSTRAINT [FK_Movies_MovieTypes] FOREIGN KEY([MovieTypeId])
	REFERENCES [dbo].[MovieTypes] ([MovieTypeId])

END 

GO

------------------------------------- 
-- dbo.MovieSeries.FK_Collections_CollectionTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeries 
-- name     : FK_Collections_CollectionTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Collections_CollectionTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeries]  WITH CHECK ADD CONSTRAINT [FK_Collections_CollectionTypes] FOREIGN KEY([MovieSeriesTypeId])
	REFERENCES [dbo].[MovieSeriesTypes] ([MovieSeriesTypeId])

END 

GO

------------------------------------- 
-- dbo.MovieSeriesTitles.FK_CollectionTitles_Collections.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTitles 
-- name     : FK_CollectionTitles_Collections 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionTitles_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD CONSTRAINT [FK_CollectionTitles_Collections] FOREIGN KEY([MovieSeriesId])
	REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])

END 

GO

------------------------------------- 
-- dbo.MovieSeriesTitles.FK_MovieSeriesTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTitles 
-- name     : FK_MovieSeriesTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieSeriesTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.MovieSeriesTitles.FK_MovieSeriesTitles_MovieSeriesTitles.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTitles 
-- name     : FK_MovieSeriesTitles_MovieSeriesTitles 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTitles_MovieSeriesTitles]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieSeriesTitles_MovieSeriesTitles] FOREIGN KEY([MovieSeriesId], [Language])
	REFERENCES [dbo].[MovieSeriesTitles] ([MovieSeriesId], [Language])

END 

GO

------------------------------------- 
-- dbo.MovieSeriesTypeTitles.FK_MovieSeriesTypeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTypeTitles 
-- name     : FK_MovieSeriesTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieSeriesTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.MovieSeriesTypeTitles.FK_MovieSeriesTypeTitles_MovieSeriesTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTypeTitles 
-- name     : FK_MovieSeriesTypeTitles_MovieSeriesTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTypeTitles_MovieSeriesTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieSeriesTypeTitles_MovieSeriesTypes] FOREIGN KEY([MovieSeriesTypeId])
	REFERENCES [dbo].[MovieSeriesTypes] ([MovieSeriesTypeId])

END 

GO

------------------------------------- 
-- dbo.MoviesInGenres.FK_MoviesInGenres_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesInGenres 
-- name     : FK_MoviesInGenres_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInGenres]  WITH CHECK ADD CONSTRAINT [FK_MoviesInGenres_Genres] FOREIGN KEY([GenreId])
	REFERENCES [dbo].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- dbo.MoviesInGenres.FK_MoviesInGenres_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesInGenres 
-- name     : FK_MoviesInGenres_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInGenres_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInGenres]  WITH CHECK ADD CONSTRAINT [FK_MoviesInGenres_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.MoviesInMovieSeries.FK_MoviesInCollection_Collections.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesInMovieSeries 
-- name     : FK_MoviesInCollection_Collections 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInCollection_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInMovieSeries]  WITH CHECK ADD CONSTRAINT [FK_MoviesInCollection_Collections] FOREIGN KEY([MovieSeriesId])
	REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])

END 

GO

------------------------------------- 
-- dbo.MoviesInMovieSeries.FK_MoviesInCollection_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesInMovieSeries 
-- name     : FK_MoviesInCollection_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInCollection_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInMovieSeries]  WITH CHECK ADD CONSTRAINT [FK_MoviesInCollection_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.MoviesOwnedByUser.FK_MoviesOwnedByUser_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesOwnedByUser 
-- name     : FK_MoviesOwnedByUser_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesOwnedByUser_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesOwnedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesOwnedByUser_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.MoviesOwnedByUser.FK_MoviesOwnedByUser_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesOwnedByUser 
-- name     : FK_MoviesOwnedByUser_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesOwnedByUser_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesOwnedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesOwnedByUser_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.MoviesWatchedByUser.FK_MoviesWatchedByUser_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesWatchedByUser 
-- name     : FK_MoviesWatchedByUser_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesWatchedByUser_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.MoviesWatchedByUser.FK_MoviesWatchedByUser_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesWatchedByUser 
-- name     : FK_MoviesWatchedByUser_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesWatchedByUser_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.MoviesWatchedByUser.FK_MoviesWatchedByUser_WatchTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesWatchedByUser 
-- name     : FK_MoviesWatchedByUser_WatchTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_WatchTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesWatchedByUser_WatchTypes] FOREIGN KEY([WatchTypeId])
	REFERENCES [dbo].[WatchTypes] ([WatchTypeId])

END 

GO

------------------------------------- 
-- dbo.MovieTitles.FK_MovieTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTitles 
-- name     : FK_MovieTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.MovieTitles.FK_MovieTitle_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTitles 
-- name     : FK_MovieTitle_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitle_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieTitle_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.MovieTypeTitles.FK_MovieTypeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTypeTitles 
-- name     : FK_MovieTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.MovieTypeTitles.FK_MovieTypeTitles_MovieTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTypeTitles 
-- name     : FK_MovieTypeTitles_MovieTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTypeTitles_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_MovieTypeTitles_MovieTypes] FOREIGN KEY([MovieTypeId])
	REFERENCES [dbo].[MovieTypes] ([MovieTypeId])

END 

GO

------------------------------------- 
-- dbo.PeopleInMovies.FK_PeopleInMovies_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PeopleInMovies 
-- name     : FK_PeopleInMovies_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PeopleInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[PeopleInMovies]'))
BEGIN
	ALTER TABLE [dbo].[PeopleInMovies]  WITH CHECK ADD CONSTRAINT [FK_PeopleInMovies_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.PeopleInMovies.FK_PeopleInMovies_People.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PeopleInMovies 
-- name     : FK_PeopleInMovies_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PeopleInMovies_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[PeopleInMovies]'))
BEGIN
	ALTER TABLE [dbo].[PeopleInMovies]  WITH CHECK ADD CONSTRAINT [FK_PeopleInMovies_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
-- dbo.PersonExternalLookup.FK_PersonExternalLookup_ExternalSources.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PersonExternalLookup 
-- name     : FK_PersonExternalLookup_ExternalSources 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[PersonExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_PersonExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
	REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])

END 

GO

------------------------------------- 
-- dbo.PersonExternalLookup.FK_PersonExternalLookup_People.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PersonExternalLookup 
-- name     : FK_PersonExternalLookup_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonExternalLookup_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[PersonExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_PersonExternalLookup_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
-- dbo.PersonInMovieRoles.FK_PersonInMovieRoles_PeopleInMovies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PersonInMovieRoles 
-- name     : FK_PersonInMovieRoles_PeopleInMovies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonInMovieRoles_PeopleInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]'))
BEGIN
	ALTER TABLE [dbo].[PersonInMovieRoles]  WITH CHECK ADD CONSTRAINT [FK_PersonInMovieRoles_PeopleInMovies] FOREIGN KEY([PersonInMovieId])
	REFERENCES [dbo].[PeopleInMovies] ([PersonInMovieId])

END 

GO

------------------------------------- 
-- dbo.PersonInMovieRoles.FK_PersonInMovieRoles_RolesInDepartments.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PersonInMovieRoles 
-- name     : FK_PersonInMovieRoles_RolesInDepartments 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonInMovieRoles_RolesInDepartments]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]'))
BEGIN
	ALTER TABLE [dbo].[PersonInMovieRoles]  WITH CHECK ADD CONSTRAINT [FK_PersonInMovieRoles_RolesInDepartments] FOREIGN KEY([RoleInDepartmentId])
	REFERENCES [dbo].[RolesInDepartments] ([RoleInDepartmentId])

END 

GO

------------------------------------- 
-- dbo.RatingSystemTitles.FK_RatingSystemTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingSystemTitles 
-- name     : FK_RatingSystemTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemTitles]  WITH CHECK ADD CONSTRAINT [FK_RatingSystemTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.RatingSystemTitles.FK_RatingSystemTitles_RatingSystems.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingSystemTitles 
-- name     : FK_RatingSystemTitles_RatingSystems 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemTitles_RatingSystems]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemTitles]  WITH CHECK ADD CONSTRAINT [FK_RatingSystemTitles_RatingSystems] FOREIGN KEY([RatingSystemId])
	REFERENCES [dbo].[RatingSystems] ([RatingSystemId])

END 

GO

------------------------------------- 
-- dbo.RatingSystemValues.FK_RatingSystemValues_RatingSystems.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingSystemValues 
-- name     : FK_RatingSystemValues_RatingSystems 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemValues_RatingSystems]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemValues]  WITH CHECK ADD CONSTRAINT [FK_RatingSystemValues_RatingSystems] FOREIGN KEY([RatingSystemId])
	REFERENCES [dbo].[RatingSystems] ([RatingSystemId])

END 

GO

------------------------------------- 
-- dbo.RatingSystemValues.FK_RatingSystemValues_RatingTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingSystemValues 
-- name     : FK_RatingSystemValues_RatingTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemValues_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemValues]  WITH CHECK ADD CONSTRAINT [FK_RatingSystemValues_RatingTypes] FOREIGN KEY([RatingTypeId])
	REFERENCES [dbo].[RatingTypes] ([RatingTypeId])

END 

GO

------------------------------------- 
-- dbo.RatingTypes.FK_RatingTypes_RatingTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingTypes 
-- name     : FK_RatingTypes_RatingTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypes_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypes]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypes]  WITH CHECK ADD CONSTRAINT [FK_RatingTypes_RatingTypes] FOREIGN KEY([ParentRatingTypeId])
	REFERENCES [dbo].[RatingTypes] ([RatingTypeId])

END 

GO

------------------------------------- 
-- dbo.RatingTypeTitles.FK_RatingTypeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingTypeTitles 
-- name     : FK_RatingTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_RatingTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.RatingTypeTitles.FK_RatingTypeTitles_RatingTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingTypeTitles 
-- name     : FK_RatingTypeTitles_RatingTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypeTitles_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_RatingTypeTitles_RatingTypes] FOREIGN KEY([RatingTypeId])
	REFERENCES [dbo].[RatingTypes] ([RatingTypeId])

END 

GO

------------------------------------- 
-- dbo.RolesInDepartments.FK_RolesInDepartments_Departments.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RolesInDepartments 
-- name     : FK_RolesInDepartments_Departments 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments]  WITH CHECK ADD CONSTRAINT [FK_RolesInDepartments_Departments] FOREIGN KEY([DepartmentId])
	REFERENCES [dbo].[Departments] ([DepartmentId])

END 

GO

------------------------------------- 
-- dbo.RolesInDepartments.FK_RolesInDepartments_Roles.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RolesInDepartments 
-- name     : FK_RolesInDepartments_Roles 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments]  WITH CHECK ADD CONSTRAINT [FK_RolesInDepartments_Roles] FOREIGN KEY([RoleId])
	REFERENCES [dbo].[Roles] ([RoleId])

END 

GO

------------------------------------- 
-- dbo.RoleTitles.FK_RoleTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RoleTitles 
-- name     : FK_RoleTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles]  WITH CHECK ADD CONSTRAINT [FK_RoleTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.RoleTitles.FK_RoleTitles_Roles.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RoleTitles 
-- name     : FK_RoleTitles_Roles 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles]  WITH CHECK ADD CONSTRAINT [FK_RoleTitles_Roles] FOREIGN KEY([RoleId])
	REFERENCES [dbo].[Roles] ([RoleId])

END 

GO

------------------------------------- 
-- dbo.RoleTitles.FK_RoleTitles_Roles1.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RoleTitles 
-- name     : FK_RoleTitles_Roles1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Roles1]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles]  WITH CHECK ADD CONSTRAINT [FK_RoleTitles_Roles1] FOREIGN KEY([RoleId])
	REFERENCES [dbo].[Roles] ([RoleId])

END 

GO

------------------------------------- 
-- dbo.Seasons.FK_Seasons_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Seasons 
-- name     : FK_Seasons_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Seasons_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[Seasons]'))
BEGIN
	ALTER TABLE [dbo].[Seasons]  WITH CHECK ADD CONSTRAINT [FK_Seasons_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.Seasons.FK_Seasons_Movies1.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Seasons 
-- name     : FK_Seasons_Movies1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Seasons_Movies1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Seasons]'))
BEGIN
	ALTER TABLE [dbo].[Seasons]  WITH CHECK ADD CONSTRAINT [FK_Seasons_Movies1] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.UserCharacterFavorites.FK_UserCharactersFavorites_Characters.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserCharacterFavorites 
-- name     : FK_UserCharactersFavorites_Characters 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserCharactersFavorites_Characters] FOREIGN KEY([CharacterId])
	REFERENCES [dbo].[Characters] ([CharacterId])

END 

GO

------------------------------------- 
-- dbo.UserCharacterFavorites.FK_UserCharactersFavorites_FavoriteTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserCharacterFavorites 
-- name     : FK_UserCharactersFavorites_FavoriteTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserCharactersFavorites_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
	REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])

END 

GO

------------------------------------- 
-- dbo.UserCharacterFavorites.FK_UserCharactersFavorites_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserCharacterFavorites 
-- name     : FK_UserCharactersFavorites_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharactersFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserCharactersFavorites_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserCharacterInMovieRatings.FK_UserCharacterInMovieRatings_CharactersInMovies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserCharacterInMovieRatings 
-- name     : FK_UserCharacterInMovieRatings_CharactersInMovies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharacterInMovieRatings_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD CONSTRAINT [FK_UserCharacterInMovieRatings_CharactersInMovies] FOREIGN KEY([CharacterInMovieId])
	REFERENCES [dbo].[CharactersInMovies] ([CharacterInMovieId])

END 

GO

------------------------------------- 
-- dbo.UserCharacterInMovieRatings.FK_UserCharacterInMovieRatings_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserCharacterInMovieRatings 
-- name     : FK_UserCharacterInMovieRatings_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharacterInMovieRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD CONSTRAINT [FK_UserCharacterInMovieRatings_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserMovieFavorites.FK_UserMovieFavorites_FavoriteTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieFavorites 
-- name     : FK_UserMovieFavorites_FavoriteTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserMovieFavorites_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
	REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])

END 

GO

------------------------------------- 
-- dbo.UserMovieFavorites.FK_UserMovieFavorites_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieFavorites 
-- name     : FK_UserMovieFavorites_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserMovieFavorites_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.UserMovieFavorites.FK_UserMovieFavorites_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieFavorites 
-- name     : FK_UserMovieFavorites_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserMovieFavorites_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserMovieGenres.FK_UserMovieGenres_Genres.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieGenres 
-- name     : FK_UserMovieGenres_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD CONSTRAINT [FK_UserMovieGenres_Genres] FOREIGN KEY([GenreId])
	REFERENCES [dbo].[Genres] ([GenreId])

END 

GO

------------------------------------- 
-- dbo.UserMovieGenres.FK_UserMovieGenres_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieGenres 
-- name     : FK_UserMovieGenres_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD CONSTRAINT [FK_UserMovieGenres_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.UserMovieGenres.FK_UserMovieGenres_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieGenres 
-- name     : FK_UserMovieGenres_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD CONSTRAINT [FK_UserMovieGenres_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserMovieHashtags.FK_UserMovieHashtags_Hashtags.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieHashtags 
-- name     : FK_UserMovieHashtags_Hashtags 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Hashtags]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags]  WITH CHECK ADD CONSTRAINT [FK_UserMovieHashtags_Hashtags] FOREIGN KEY([Hashtag])
	REFERENCES [dbo].[Hashtags] ([Hashtag])

END 

GO

------------------------------------- 
-- dbo.UserMovieHashtags.FK_UserMovieHashtags_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieHashtags 
-- name     : FK_UserMovieHashtags_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags]  WITH CHECK ADD CONSTRAINT [FK_UserMovieHashtags_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.UserMovieHashtags.FK_UserMovieHashtags_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieHashtags 
-- name     : FK_UserMovieHashtags_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags]  WITH CHECK ADD CONSTRAINT [FK_UserMovieHashtags_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserMovieRatings.FK_UserMovieRatings_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieRatings 
-- name     : FK_UserMovieRatings_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD CONSTRAINT [FK_UserMovieRatings_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.UserMovieRatings.FK_UserMovieRatings_RatingTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieRatings 
-- name     : FK_UserMovieRatings_RatingTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD CONSTRAINT [FK_UserMovieRatings_RatingTypes] FOREIGN KEY([RatingTypeId])
	REFERENCES [dbo].[RatingTypes] ([RatingTypeId])

END 

GO

------------------------------------- 
-- dbo.UserMovieRatings.FK_UserMovieRatings_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieRatings 
-- name     : FK_UserMovieRatings_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD CONSTRAINT [FK_UserMovieRatings_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserMovieReviews.FK_UserMovieReviews_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieReviews 
-- name     : FK_UserMovieReviews_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews]  WITH CHECK ADD CONSTRAINT [FK_UserMovieReviews_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.UserMovieReviews.FK_UserMovieReviews_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieReviews 
-- name     : FK_UserMovieReviews_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews]  WITH CHECK ADD CONSTRAINT [FK_UserMovieReviews_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserMovieWishlist.FK_UserMovieWishlist_Movies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieWishlist 
-- name     : FK_UserMovieWishlist_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieWishlist_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD CONSTRAINT [FK_UserMovieWishlist_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
-- dbo.UserMovieWishlist.FK_UserMovieWishlist_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieWishlist 
-- name     : FK_UserMovieWishlist_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieWishlist_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD CONSTRAINT [FK_UserMovieWishlist_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserPeopleFavorites.FK_UserPeopleFavorites_FavoriteTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserPeopleFavorites 
-- name     : FK_UserPeopleFavorites_FavoriteTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserPeopleFavorites_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
	REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])

END 

GO

------------------------------------- 
-- dbo.UserPeopleFavorites.FK_UserPeopleFavorites_People.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserPeopleFavorites 
-- name     : FK_UserPeopleFavorites_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserPeopleFavorites_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
-- dbo.UserPeopleFavorites.FK_UserPeopleFavorites_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserPeopleFavorites 
-- name     : FK_UserPeopleFavorites_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserPeopleFavorites_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserPersonInMovieRoleRatings.FK_UserPersonInMovieRoleRatings_PersonInMovieRoles.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserPersonInMovieRoleRatings 
-- name     : FK_UserPersonInMovieRoleRatings_PersonInMovieRoles 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPersonInMovieRoleRatings_PersonInMovieRoles]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD CONSTRAINT [FK_UserPersonInMovieRoleRatings_PersonInMovieRoles] FOREIGN KEY([PersonInMovieRoleId])
	REFERENCES [dbo].[PersonInMovieRoles] ([PersonInMovieRoleId])

END 

GO

------------------------------------- 
-- dbo.UserPersonInMovieRoleRatings.FK_UserPersonInMovieRoleRatings_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserPersonInMovieRoleRatings 
-- name     : FK_UserPersonInMovieRoleRatings_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPersonInMovieRoleRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD CONSTRAINT [FK_UserPersonInMovieRoleRatings_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.UserSessions.FK_UserSession_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserSessions 
-- name     : FK_UserSession_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSession_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSessions]'))
BEGIN
	ALTER TABLE [dbo].[UserSessions]  WITH CHECK ADD CONSTRAINT [FK_UserSession_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- dbo.VoiceOverActors.FK_VoiceOverActors_CharactersInMovies.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : VoiceOverActors 
-- name     : FK_VoiceOverActors_CharactersInMovies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD CONSTRAINT [FK_VoiceOverActors_CharactersInMovies] FOREIGN KEY([CharacterInMovieId])
	REFERENCES [dbo].[CharactersInMovies] ([CharacterInMovieId])

END 

GO

------------------------------------- 
-- dbo.VoiceOverActors.FK_VoiceOverActors_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : VoiceOverActors 
-- name     : FK_VoiceOverActors_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD CONSTRAINT [FK_VoiceOverActors_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.VoiceOverActors.FK_VoiceOverActors_People.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : VoiceOverActors 
-- name     : FK_VoiceOverActors_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD CONSTRAINT [FK_VoiceOverActors_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
-- dbo.WatchTypeTitles.FK_WatchTypeTitles_Languages.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : WatchTypeTitles 
-- name     : FK_WatchTypeTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WatchTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[WatchTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_WatchTypeTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
-- dbo.WatchTypeTitles.FK_WatchTypeTitles_WatchTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : WatchTypeTitles 
-- name     : FK_WatchTypeTitles_WatchTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WatchTypeTitles_WatchTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[WatchTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_WatchTypeTitles_WatchTypes] FOREIGN KEY([WatchTypeId])
	REFERENCES [dbo].[WatchTypes] ([WatchTypeId])

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
-- dbo.BoxOfficeTypeTitles.UX_BoxOfficeTypeTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : BoxOfficeTypeTitles 
-- name     : UX_BoxOfficeTypeTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]') AND name = N'UX_BoxOfficeTypeTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_BoxOfficeTypeTitles] ON [dbo].[BoxOfficeTypeTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.DepartmentTitles.UX_DepartmentTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : DepartmentTitles 
-- name     : UX_DepartmentTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]') AND name = N'UX_DepartmentTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_DepartmentTitles] ON [dbo].[DepartmentTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.Episodes.UX_EpisodeNumbers.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Episodes 
-- name     : UX_EpisodeNumbers 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Episodes]') AND name = N'UX_EpisodeNumbers')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_EpisodeNumbers] ON [dbo].[Episodes]
	(
		[SeasonId] ASC,
		[Number] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.EpisodeTitles.UX_EpisodeTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : EpisodeTitles 
-- name     : UX_EpisodeTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[EpisodeTitles]') AND name = N'UX_EpisodeTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_EpisodeTitles] ON [dbo].[EpisodeTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.GenreTitles.UX_GenreTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : GenreTitles 
-- name     : UX_GenreTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[GenreTitles]') AND name = N'UX_GenreTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_GenreTitles] ON [dbo].[GenreTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.IconTypeTitles.UX_IconTypeTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconTypeTitles 
-- name     : UX_IconTypeTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]') AND name = N'UX_IconTypeTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_IconTypeTitles] ON [dbo].[IconTypeTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.LanguageTitles.UX_LanguageTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : LanguageTitles 
-- name     : UX_LanguageTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LanguageTitles]') AND name = N'UX_LanguageTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_LanguageTitles] ON [dbo].[LanguageTitles]
	(
		[InLanguage] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.MovieSeriesTitles.UX_MovieSeriesTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTitles 
-- name     : UX_MovieSeriesTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]') AND name = N'UX_MovieSeriesTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieSeriesTitles] ON [dbo].[MovieSeriesTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.MovieSeriesTypeTitles.UX_MovieSeriesTypeTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTypeTitles 
-- name     : UX_MovieSeriesTypeTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]') AND name = N'UX_MovieSeriesTypeTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieSeriesTypeTitles] ON [dbo].[MovieSeriesTypeTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.MovieTitles.UX_MovieTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTitles 
-- name     : UX_MovieTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MovieTitles]') AND name = N'UX_MovieTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieTitles] ON [dbo].[MovieTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.MovieTypeTitles.UX_MovieTypeTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieTypeTitles 
-- name     : UX_MovieTypeTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]') AND name = N'UX_MovieTypeTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieTypeTitles] ON [dbo].[MovieTypeTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.RatingSystemTitles.UX_RatingSystemTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingSystemTitles 
-- name     : UX_RatingSystemTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemTitles]') AND name = N'UX_RatingSystemTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_RatingSystemTitles] ON [dbo].[RatingSystemTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.RatingTypeTitles.UX_RatingTypeTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingTypeTitles 
-- name     : UX_RatingTypeTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]') AND name = N'UX_RatingTypeTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_RatingTypeTitles] ON [dbo].[RatingTypeTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.RoleTitles.UX_RoleTitles.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RoleTitles 
-- name     : UX_RoleTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RoleTitles]') AND name = N'UX_RoleTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_RoleTitles] ON [dbo].[RoleTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.Seasons.UX_SeasonNumbers.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Seasons 
-- name     : UX_SeasonNumbers 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Seasons]') AND name = N'UX_SeasonNumbers')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_SeasonNumbers] ON [dbo].[Seasons]
	(
		[MovieId] ASC,
		[Number] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.Vendors.UX_Vendors.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Vendors 
-- name     : UX_Vendors 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendors]') AND name = N'UX_Vendors')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_Vendors] ON [dbo].[Vendors]
	(
		[Name] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
-- dbo.WatchTypes.UX_WatchTypes.UX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : WatchTypes 
-- name     : UX_WatchTypes 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WatchTypes]') AND name = N'UX_WatchTypes')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_WatchTypes] ON [dbo].[WatchTypes]
	(
		[Name] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
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
-- dbo.AcquisitionDelete.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : AcquisitionDelete 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.AcquisitionDelete
-- Date: 2016-10-21
-- Release: 1.0
-- Summary:
--   * Deletes the specified acquisition.
-- Param:
--   @acquisitionId int
--     * The id of the acquisition.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AcquisitionDelete]
	@acquisitionId int
as
begin
	delete
	from dbo.Acquisitions
	where AcquisitionId = @acquisitionId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AcquisitionDelete]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[AcquisitionDelete] 
GRANT EXECUTE ON [dbo].[AcquisitionDelete] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.AcquisitionSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : AcquisitionSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.AcquisitionSave
-- Date: 2016-10-20
-- Release: 1.0
-- Summary:
--   * Saves an acquisition.
-- Param:
--   @acquisitionId int
--     * The id of the acquisition.
--   @movieId int
--     * The id of the movie which was acquired.
--   @userId int
--     * The id of the user that acquired the movie.
--   @acquiredAt date
--     * The date of the acquisition.
--   @dateUncertain bit
--     * Whether or not the date of the acquisition is uncertain since the exact date is not known.
--   @vendorId int
--     * The id of the vendor from which the movie was acquired.
--   @price int
--     * The price payed for the movie.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AcquisitionSave]
	@acquisitionId int
	,@movieId int
	,@userId int
	,@acquiredAt date = null
	,@dateUncertain bit = 0
	,@vendorId int = null
	,@price float = 0
as
begin
	declare @outputIds table(AcquisitionId int);
	declare @moviesOwnedByUserId int = (
		select MoviesOwnedByUserId
		from dbo.MoviesOwnedByUser
		where UserId = @userId
			and MovieId = @movieId
	);

	if (@moviesOwnedByUserId is null)
	begin
		return;
	end;

	if (@acquisitionId = 0)
	begin
		insert dbo.Acquisitions
		output inserted.AcquisitionId into @outputIds
		select @moviesOwnedByUserId
			,@acquiredAt
			,@dateUncertain
			,@vendorId
			,@price;
	end
	else
	begin
		if not exists (
			select 1
			from dbo.Acquisitions
			where AcquisitionId = @acquisitionId
				and MoviesOwnedByUserId = @moviesOwnedByUserId
		)
		begin
			return;
		end;

		update dbo.Acquisitions
		set AcquiredAt = @acquiredAt
			,DateUncertain = @dateUncertain
			,VendorId = @vendorId
			,Price = @price
		output inserted.AcquisitionId into @outputIds
		where AcquisitionId = @acquisitionId;
	end;

	select a.AcquisitionId
		,a.MoviesOwnedByUserId
		,a.AcquiredAt
		,a.DateUncertain
		,a.VendorId
		,a.Price
	from dbo.Acquisitions as a
	inner join @outputIds as r on r.AcquisitionId = a.AcquisitionId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AcquisitionSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[AcquisitionSave] 
GRANT EXECUTE ON [dbo].[AcquisitionSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.CharacterGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharacterGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.CharacterGet
-- Date: 2018-04-23
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[CharacterGet]
	@characterIds dbo.IdCollection readonly
as
begin
	;with cteRatings as (
		select i.CharacterId
			,avg(r.Rating) as Rating
		from dbo.UserCharacterInMovieRatings as r
		inner join dbo.CharactersInMovies as i on i.CharacterInMovieId = r.CharacterInMovieId
		group by i.CharacterId
	)
	select c.CharacterId
		,c.Name
		,isnull(t.Rating, 0) as TotalRating
		,0 UserId
		,0 UserRating
	from dbo.Characters as c
	inner join @characterIds as r on r.Id = c.CharacterId
	left join cteRatings as t on t.CharacterId = c.CharacterId;

	select c.CharacterId
		,x.IconId
	from dbo.Characters as c
	inner join dbo.IconsInCharacters as x on x.CharacterId = c.CharacterId
	inner join @characterIds as r on r.Id = c.CharacterId
	order by x."Order" asc;

	select x.ExternalSourceId
		,x.ExternalId
	from dbo.Characters as c
	inner join dbo.CharacterExternalLookup as x on x.CharacterId = c.CharacterId
	inner join @characterIds as r on r.Id = c.CharacterId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[CharacterGet] 
GRANT EXECUTE ON [dbo].[CharacterGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.CharacterSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharacterSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.CharacterSave
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Saves a character.
-- Param:
--   @characterId int
--     * The id of the character.
--   @name nvarchar(100)
--     * The name of the character.
--   @icons dbo.IdOrderCollection
--     * The id of the icons of the character.
--   @externalLookups dbo.ExternalLookupIdCollection
--     * The id of the character in external sources.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[CharacterSave]
	@characterId int
	,@name nvarchar(100)
	,@icons dbo.IdOrderCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@characterId = 0)
		begin
			insert dbo.Characters
			output inserted.CharacterId into @output
			select @name;
		end
		else
		begin
			update dbo.Characters
			set Name = @name
			output inserted.CharacterId into @output
			where CharacterId = @characterId;
		end;

		merge dbo.IconsInCharacters as t
		using (
			select r.Id as CharacterId
				,x.Id as IconId
				,x."Order"
			from @icons as x
			inner join @output as r on 1 = 1
		) as s on s.CharacterId = t.CharacterId
			and s.IconId = t.IconId
		when not matched by target then
			insert (CharacterId, IconId, "Order")
			values (s.CharacterId, s.IconId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		merge dbo.CharacterExternalLookup as t
		using (
			select r.Id as CharacterId
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.CharacterId = t.CharacterId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (CharacterId, ExternalSourceId, ExternalId)
			values (s.CharacterId, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId
		when not matched by source then
			delete;

		exec dbo.CharacterGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[CharacterSave] 
GRANT EXECUTE ON [dbo].[CharacterSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.CharacterSearch.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharacterSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.CharacterSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[CharacterSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
	if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1)
	begin
		select top(@searchLimit) c.CharacterId
		from dbo.Characters as c
		where Name = @searchText;
	end
	else
	begin
		select top(@searchLimit) c.CharacterId
		from dbo.Characters as c
		where Name like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[CharacterSearch] 
GRANT EXECUTE ON [dbo].[CharacterSearch] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.DepartmentGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : DepartmentGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.DepartmentGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DepartmentGet]
	@departmentIds dbo.IdCollection readonly
as
begin
	select d.DepartmentId
		,d."Order"
	from dbo.Departments as d
	inner join @departmentIds as i on i.Id = d.DepartmentId;

	select d.DepartmentId
		,ti.Language
		,ti.Title
	from dbo.Departments d
	inner join dbo.DepartmentTitles as ti on ti.DepartmentId = d.DepartmentId
	inner join @departmentIds as i on i.Id = d.DepartmentId;

	select d.DepartmentId
		,rd.RoleId
	from dbo.Departments d
	inner join dbo.RolesInDepartments as rd on rd.DepartmentId = d.DepartmentId
	inner join @departmentIds as i on i.Id = d.DepartmentId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[DepartmentGet] 
GRANT EXECUTE ON [dbo].[DepartmentGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.DepartmentGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : DepartmentGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.DepartmentGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DepartmentGetAll]
as
begin
	select d.DepartmentId
		,d."Order"
	from dbo.Departments as d;

	select d.DepartmentId
		,ti.Language
		,ti.Title
	from dbo.Departments d
	inner join dbo.DepartmentTitles as ti on ti.DepartmentId = d.DepartmentId;

	select d.DepartmentId
		,rd.RoleId
	from dbo.Departments d
	inner join dbo.RolesInDepartments as rd on rd.DepartmentId = d.DepartmentId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[DepartmentGetAll] 
GRANT EXECUTE ON [dbo].[DepartmentGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.DepartmentSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : DepartmentSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.DepartmentSave
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DepartmentSave]
	@departmentId int
	,@order int = null
	,@titles dbo.LanguageTitleCollection readonly
	,@roles dbo.IdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@departmentId = 0)
		begin
			if (@order is null)
			begin
				select @order = isnull(max("Order"), -1) + 1
				from dbo.Departments
			end;

			insert dbo.Departments
			output inserted.DepartmentId into @output
			select @order;
		end
		else if (@order is not null)
		begin
			update dbo.Departments
			set "Order" = @order
			output inserted.DepartmentId into @output
			where DepartmentId = @departmentId;
		end;

		merge dbo.DepartmentTitles as t
		using (
			select r.Id as DepartmentId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.DepartmentId = t.DepartmentId
			and s.Language = t.Language
		when not matched by target then
			insert (DepartmentId, Language, Title)
			values (s.DepartmentId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.RolesInDepartments as t
		using (
			select r.Id as DepartmentId
				,dr.Id as RoleId
			from @roles as dr
			inner join @output as r on 1 = 1
		) as s on s.DepartmentId = t.DepartmentId
			and s.RoleId = t.RoleId
		when not matched by target then
			insert (DepartmentId, RoleId)
			values (s.DepartmentId, s.RoleId)
		when not matched by source then
			delete;

		exec dbo.DepartmentGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[DepartmentSave] 
GRANT EXECUTE ON [dbo].[DepartmentSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.EpisodeSearch.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : EpisodeSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.EpisodeSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[EpisodeSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1)
	begin
		select distinct top(@searchLimit) c.EpisodeId
		from dbo.Episodes as c
		inner join dbo.EpisodeTitles as ti on ti.EpisodeId = c.EpisodeId
		where ti.Title = @searchText;
	end
	else
	begin
		select distinct top(@searchLimit) c.EpisodeId
		from dbo.Episodes as c
		inner join dbo.EpisodeTitles as ti on ti.EpisodeId = c.EpisodeId
		where ti.Title like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EpisodeSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[EpisodeSearch] 
GRANT EXECUTE ON [dbo].[EpisodeSearch] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.ErrorGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ErrorGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ErrorGet
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Gets the specified error.
-- Param:
--   @errorIds dbo.IdCollection
--     * The list of ids of the errors to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ErrorGet]
	@errorIds dbo.IdCollection readonly
as
begin
	select e.ErrorId
		,e.UserId
		,e.Time
		,e.Type
		,e.Source
		,e.TargetSite
		,e.Message
		,e.StackTrace
	from dbo.Errors e
	inner join @errorIds as i on i.Id = e.ErrorId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ErrorGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ErrorGet] 
GRANT EXECUTE ON [dbo].[ErrorGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.ErrorSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ErrorSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ErrorSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ErrorSave]
	@errorId int
	,@userId int
	,@time datetime2(7)
	,@type nvarchar(100)
	,@source nvarchar(255)
	,@targetSite nvarchar(255)
	,@message nvarchar(255)
	,@stackTrace nvarchar(max) 
as
begin
		declare @output dbo.IdCollection;

		if (@errorId = 0)
		begin
			insert dbo.Errors
			output inserted.ErrorId into @output
			select @userId
				,@time
				,@type
				,@source
				,@targetSite
				,@message
				,@stackTrace;
		end
		else
		begin		
			update dbo.Errors
			set UserId = @userId
				,"Time" = @time
				,"Type" = @type
				,"Source" = @source
				,TargetSite = @targetSite
				,"Message" = @message
				,StackTrace = @stackTrace
			output inserted.ErrorId into @output
			where ErrorId = @errorId;
		end;

		exec dbo.ErrorGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ErrorSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ErrorSave] 
GRANT EXECUTE ON [dbo].[ErrorSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.ExternalSourceGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSourceGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ExternalSourceGet
-- Date: 2018-03-19
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ExternalSourceGet]
	@externalSourceIds dbo.IdCollection readonly
as
begin
	select e.ExternalSourceId
		,e.Name
		,e.BaseAddress
		,e.PeopleAddress
		,e.CharacterAddress
		,e.GenreAddress
		,e.EpisodeAddress
	from dbo.ExternalSources e
	inner join @externalSourceIds as i on i.Id = e.ExternalSourceId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSourceGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalSourceGet] 
GRANT EXECUTE ON [dbo].[ExternalSourceGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.ExternalSourceGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSourceGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ExternalSourceGetAll
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ExternalSourceGetAll]
as
begin
	select e.ExternalSourceId
		,e.Name
		,e.BaseAddress
		,e.PeopleAddress
		,e.CharacterAddress
		,e.GenreAddress
		,e.EpisodeAddress
	from dbo.ExternalSources e
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSourceGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalSourceGetAll] 
GRANT EXECUTE ON [dbo].[ExternalSourceGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.ExternalSourceSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSourceSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ExternalSourceSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ExternalSourceSave]
	@externalSourceId int
	,@name nvarchar(50)
	,@baseAddress nvarchar(250)
	,@peopleAddress nvarchar(150)
	,@characterAddress nvarchar(150)
	,@genreAddress nvarchar(150)
	,@episodeAddress nvarchar(150)
as
begin
		declare @output dbo.IdCollection;

		if (@externalSourceId = 0)
		begin
			insert dbo.ExternalSources
			output inserted.ExternalSourceId into @output
			select @name
				,@baseAddress
				,@peopleAddress
				,@characterAddress
				,@genreAddress
				,@episodeAddress;
		end
		else
		begin
			update dbo.ExternalSources
			set Name = @name
				,BaseAddress = @baseAddress
				,PeopleAddress = @peopleAddress
				,CharacterAddress = @characterAddress
				,GenreAddress = @genreAddress
				,EpisodeAddress = @episodeAddress
			output inserted.ExternalSourceId into @output
			where ExternalSourceId = @externalSourceId;
		end;

		exec dbo.ExternalSourceGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSourceSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalSourceSave] 
GRANT EXECUTE ON [dbo].[ExternalSourceSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.GenreGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.GenreGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstag. All rights reserveg.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GenreGet]
	@genreIds dbo.IdCollection readonly
as
begin
	select g.GenreId
	from dbo.Genres as g
	inner join @genreIds as i on i.Id = g.GenreId;

	select g.GenreId
		,ti.Language
		,ti.Title
	from dbo.Genres g
	inner join dbo.GenreTitles as ti on ti.GenreId = g.GenreId
	inner join @genreIds as i on i.Id = g.GenreId;

	select x.ExternalSourceId
		,x.ExternalId
	from dbo.Genres as g
	inner join dbo.GenreExternalLookup as x on x.GenreId = g.GenreId
	inner join @genreIds as r on r.Id = g.GenreId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[GenreGet] 
GRANT EXECUTE ON [dbo].[GenreGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.GenreGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.GenreGetAll
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstag. All rights reserveg.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GenreGetAll]
as
begin
	select g.GenreId
	from dbo.Genres as g;

	select g.GenreId
		,ti.Language
		,ti.Title
	from dbo.Genres g
	inner join dbo.GenreTitles as ti on ti.GenreId = g.GenreId;

	select x.ExternalSourceId
		,x.ExternalId
	from dbo.Genres as g
	inner join dbo.GenreExternalLookup as x on x.GenreId = g.GenreId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[GenreGetAll] 
GRANT EXECUTE ON [dbo].[GenreGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.GenreSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.GenreSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GenreSave]
	@genreId int
	,@titles dbo.LanguageTitleCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@genreId = 0)
		begin
			insert dbo.Genres
			output inserted.GenreId into @output
			default values;
		end
		else
		begin
			insert @output
			select @genreId;
		end;

		merge dbo.GenreTitles as t
		using (
			select r.Id as GenreId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.GenreId = t.GenreId
			and s.Language = t.Language
		when not matched by target then
			insert (GenreId, Language, Title)
			values (s.GenreId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.GenreExternalLookup as t
		using (
			select r.Id
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.Id = t.GenreId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (GenreId, ExternalSourceId, ExternalId)
			values (s.Id, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId
		when not matched by source then
			delete;

		exec dbo.GenreGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[GenreSave] 
GRANT EXECUTE ON [dbo].[GenreSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.IconGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconGet
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Gets the specified icon.
-- Param:
--   @iconIds dbo.IdCollection
--     * The list of ids of the icons to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconGet]
	@iconIds dbo.IdCollection readonly
as
begin
	select e.IconId
		,e.IconTypeId
		,e.IconUrl
		,e."Data"
		,e.DataSize
	from dbo.Icons e
	inner join @iconIds as i on i.Id = e.IconId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconGet] 
GRANT EXECUTE ON [dbo].[IconGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.IconSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconSave]
	@iconId int
	,@iconTypeId int
	,@iconUrl nvarchar(500)
	,@dataSize int
	,@data varbinary(max)
as
begin
		declare @output dbo.IdCollection;

		if (@iconId = 0)
		begin
			insert dbo.Icons
			output inserted.IconId into @output
			select @iconTypeId
				,@iconUrl
				,@dataSize
				,@data;
		end
		else
		begin		
			update dbo.Icons
			set IconTypeId = @iconTypeId
				,IconUrl = @iconUrl
				,DataSize = @dataSize
				,"Data" = @data
			output inserted.IconId into @output
			where IconId = @iconId;
		end;

		exec dbo.IconGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconSave] 
GRANT EXECUTE ON [dbo].[IconSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.IconTypeGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconTypeGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconTypeGet]
	@iconTypeIds dbo.IdCollection readonly
as
begin
	select t.IconTypeId
	from dbo.IconTypes as t
	inner join @iconTypeIds as i on i.Id = t.IconTypeId;

	select t.IconTypeId
		,ti.Language
		,ti.Title
	from dbo.IconTypes t
	inner join dbo.IconTypeTitles as ti on ti.IconTypeId = t.IconTypeId
	inner join @iconTypeIds as i on i.Id = t.IconTypeId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconTypeGet] 
GRANT EXECUTE ON [dbo].[IconTypeGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.IconTypeGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconTypeGetAll
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconTypeGetAll]
as
begin
	select t.IconTypeId
	from dbo.IconTypes as t;

	select t.IconTypeId
		,ti.Language
		,ti.Title
	from dbo.IconTypes t
	inner join dbo.IconTypeTitles as ti on ti.IconTypeId = t.IconTypeId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconTypeGetAll] 
GRANT EXECUTE ON [dbo].[IconTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.IconTypeSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.IconTypeSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconTypeSave]
	@iconTypeId int
	,@order int
	,@titles dbo.LanguageTitleCollection readonly
	,@roleIds dbo.IdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@iconTypeId = 0)
		begin
			insert dbo.IconTypes
			output inserted.IconTypeId into @output
			default values;
		end
		else
		begin
			insert @output
			select @iconTypeId;
		end;

		merge dbo.IconTypeTitles as t
		using (
			select r.Id as IconTypeId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.IconTypeId = t.IconTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (IconTypeId, Language, Title)
			values (s.IconTypeId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		exec dbo.IconTypeGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconTypeSave] 
GRANT EXECUTE ON [dbo].[IconTypeSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.LanguageGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.LanguageGet
-- Date: 2017-07-06
-- Release: 1.0
-- Summary:
--   * Gets all language titles the specified language, or en-US if missing in the native language.
-- Param:
--   @language varchar(8)
--     * The language to get the titles in.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[LanguageGet]
	@language varchar(8)
as
begin
	select l.Language
		,isnull(Specific.InLanguage, t.InLanguage) as InLanguage
		,isnull(Specific.Title, t.Title) as Title
	from dbo.Languages as l
	left join dbo.LanguageTitles as Specific on Specific.Language = l.Language
		and Specific.InLanguage = @language
	left join dbo.LanguageTitles as t on t.Language = l.Language
		and t.InLanguage = ''en-US''
	order by l.Language;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageGet] 
GRANT EXECUTE ON [dbo].[LanguageGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.LanguageGetNative.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageGetNative 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.LanguageGetNative
-- Date: 2017-07-06
-- Release: 1.0
-- Summary:
--   * Gets all language titles in their native language, or en-US if missing in the native language.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[LanguageGetNative]
as
begin
	;with Natives as (
		select t.*
		from dbo.Languages as l
		inner join dbo.LanguageTitles as t on t.Language = l.Language
			and t.InLanguage = l.Language
	)
	select l.Language
		,isnull(Natives.InLanguage, t.InLanguage) as InLanguage
		,isnull(Natives.Title, t.Title) as Title
	from dbo.Languages as l
	left join Natives on Natives.Language = l.Language
	left join dbo.LanguageTitles as t on t.Language = l.Language
		and t.InLanguage = ''en-US''
	order by l.Language;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageGetNative]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageGetNative] 
GRANT EXECUTE ON [dbo].[LanguageGetNative] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.LanguageSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.LanguageSave
-- Date: 2017-07-07
-- Release: 1.0
-- Summary:
--   * Saves a language and it''s titles.
-- Param:
--   @languageId nvarchar(8)
--     * The id of the language.
--   @titles dbo.LanguagesTitlesCollection
--     * The titles of the language.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[LanguageSave]
	@language nvarchar(8)
	,@titles dbo.LanguagesTitlesCollection readonly
as
begin
	if (select count(1) from @titles) = 0
	begin
		throw 50000, ''Invalid titles count. A language can''''t be saved witout any titles.'', 1;
	end;

	if not exists (select 1 from @titles where InLanguage = ''en-US'')
	begin
		throw 50000, ''Missing default title. A language can''''t be saved witout a title in the default language (en-US).'', 1;
	end;

	begin transaction;
	begin try
		declare @output table(Language nvarchar(8));

		if not exists (
			select 1
			from dbo.Languages
			where Language = @language
			)
		begin
			insert dbo.Languages
			output inserted.Language into @output
			default values;
		end
		else
		begin		
			insert @output
			select @language
		end;

		merge dbo.LanguageTitles as t
		using (
			select r.Language
				,lt.InLanguage
				,lt.Title
			from @titles as lt
			inner join @output as r on 1 = 1
		) as s on s.Language = t.Language
			and s.InLanguage = t.InLanguage
		when not matched by target then
			insert (Language, InLanguage, Title)
			values (s.Language, s.InLanguage, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		select l.Language
			,t.InLanguage
			,t.Title
		from dbo.Languages as l
		inner join dbo.LanguageTitles as t on t.Language = l.Language
		inner join @output as o on o.Language = l.Language;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageSave] 
GRANT EXECUTE ON [dbo].[LanguageSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSave
-- Date: 2017-07-07
-- Release: 1.0
-- Summary:
--   * Saves a movie.
-- Param:
--   @xxx
--     * xxx
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSave]
	@movieId int
	,@movieTypeId int
	,@year date
	,@titles dbo.LanguageTitleCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
	,@externalRatings dbo.ExternalRatingCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output table(MovieId int);

		if (@movieId = 0)
		begin
			insert dbo.Movies
			output inserted.MovieId into @output
			select @movieTypeId
				,@year;
		end
		else
		begin
			update dbo.Movies
			set MovieTypeId = @movieTypeId
				,"Year" = @year
			output inserted.MovieId into @output
			where MovieId = @movieId;
		end;

		merge dbo.MovieTitles as t
		using (
			select r.MovieId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieId = t.MovieId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieId, Language, Title)
			values (s.MovieId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.MovieExternalLookup as t
		using (
			select r.MovieId
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.MovieId = t.MovieId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (MovieId, ExternalSourceId, ExternalId)
			values (s.MovieId, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId;

		--select m.MovieId
		--	,
		--from dbo.Movies as m
		--inner join @output as r on r.MovieId = m.MovieId;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSave] 
GRANT EXECUTE ON [dbo].[MovieSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSearch.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1)
	begin
		select distinct top(@searchLimit) c.MovieId
		from dbo.Movies as c
		inner join dbo.MovieTitles as ti on ti.MovieId = c.MovieId
		where ti.Title = @searchText;
	end
	else
	begin
		select distinct top(@searchLimit) c.MovieId
		from dbo.Movies as c
		inner join dbo.MovieTitles as ti on ti.MovieId = c.MovieId
		where ti.Title like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSearch] 
GRANT EXECUTE ON [dbo].[MovieSearch] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSeriesGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesGet
-- Date: 2018-04-11
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesGet]
	@MovieSeriesIds dbo.IdCollection readonly
as
begin
	select s.MovieSeriesId
		,s.MovieSeriesTypeId
	from dbo.MovieSeries as s
	inner join @MovieSeriesIds as i on i.Id = s.MovieSeriesId;

	select s.MovieSeriesId
		,ti.Language
		,ti.Title
	from dbo.MovieSeries as s
	inner join dbo.MovieSeriesTitles as ti on ti.MovieSeriesId = s.MovieSeriesId
	inner join @MovieSeriesIds as i on i.Id = s.MovieSeriesId;

	select d.MovieSeriesId
		,m.MovieId
	from dbo.MovieSeries as d
	inner join dbo.MoviesInMovieSeries as m on m.MovieSeriesId = d.MovieSeriesId
	inner join @MovieSeriesIds as i on i.Id = d.MovieSeriesId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesGet] 
GRANT EXECUTE ON [dbo].[MovieSeriesGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSeriesSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.MovieSeriesSave
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesSave]
	@movieSeriesId int
	,@movieSeriesTypeId int
	,@titles dbo.LanguageTitleCollection readonly
	,@movieIds dbo.IdOrderCollection readonly
	,@icons dbo.IdOrderCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@movieSeriesId = 0)
		begin
			insert dbo.MovieSeries
			output inserted.MovieSeriesId into @output
			select @movieSeriesTypeId;
		end
		else
		begin
			update dbo.MovieSeries
			set MovieSeriesTypeId = @movieSeriesTypeId
			output inserted.MovieSeriesId into @output
			where MovieSeriesId = @movieSeriesId;
		end;

		merge dbo.MovieSeriesTitles as t
		using (
			select r.Id as MovieSeriesId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesId = t.MovieSeriesId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieSeriesId, Language, Title)
			values (s.MovieSeriesId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.MoviesInMovieSeries as t
		using (
			select r.Id as MovieSeriesId
				,dr.Id as MovieId
				,dr."Order"
			from @movieIds as dr
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesId = t.MovieSeriesId
			and s.MovieId = t.MovieId
		when not matched by target then
			insert (MovieSeriesId, MovieId, "Order")
			values (s.MovieSeriesId, s.MovieId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		merge dbo.IconsInMovieSeries as t
		using (
			select r.Id as MovieSeriesId
				,x.Id as IconId
				,x."Order"
			from @icons as x
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesId = t.MovieSeriesId
			and s.IconId = t.IconId
		when not matched by target then
			insert (MovieSeriesId, IconId, "Order")
			values (s.MovieSeriesId, s.IconId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		exec dbo.MovieSeriesGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesSave] 
GRANT EXECUTE ON [dbo].[MovieSeriesSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSeriesSearch.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1)
	begin
		select distinct top(@searchLimit) c.MovieSeriesId
		from dbo.MovieSeries as c
		inner join dbo.MovieSeriesTitles as ti on ti.MovieSeriesId = c.MovieSeriesId
		where ti.Title = @searchText;
	end
	else
	begin
		select distinct top(@searchLimit) c.MovieSeriesId
		from dbo.MovieSeries as c
		inner join dbo.MovieSeriesTitles as ti on ti.MovieSeriesId = c.MovieSeriesId
		where ti.Title like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesSearch] 
GRANT EXECUTE ON [dbo].[MovieSeriesSearch] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSeriesTypeGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesTypesGet
-- Date: 2018-04-10
-- Release: 1.0
-- Summary:
--   * Gets the specified movie series types.
-- Param:
--   @movieSeriesTypeIds dbo.IdCollection
--     * The list of ids of the movie series types to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesTypeGet]
	@movieSeriesTypeIds dbo.IdCollection readonly
as
begin
	select t.MovieSeriesTypeId
	from dbo.MovieSeriesTypes as t
	inner join @movieSeriesTypeIds as r on r.Id = t.MovieSeriesTypeId;

	select t.MovieSeriesTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieSeriesTypes t
	inner join dbo.MovieSeriesTypeTitles as ti on ti.MovieSeriesTypeId = t.MovieSeriesTypeId
	inner join @movieSeriesTypeIds as i on i.Id = t.MovieSeriesTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesTypeGet] 
GRANT EXECUTE ON [dbo].[MovieSeriesTypeGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSeriesTypeGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesTypesGetAll
-- Date: 2018-04-10
-- Release: 1.0
-- Summary:
--   * Gets all movie series types.
-- Param:
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesTypeGetAll]
as
begin
	select t.MovieSeriesTypeId
	from dbo.MovieSeriesTypes as t;

	select t.MovieSeriesTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieSeriesTypes t
	inner join dbo.MovieSeriesTypeTitles as ti on ti.MovieSeriesTypeId = t.MovieSeriesTypeId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesTypeGetAll] 
GRANT EXECUTE ON [dbo].[MovieSeriesTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieSeriesTypeSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesTypeSave
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Saves a movie.
-- Param:
--   @movieSeriesTypeId int
--     * The id of the movie series type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the movie series type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesTypeSave]
	@movieSeriesTypeId int
	,@titles dbo.LanguageTitleCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output table(MovieSeriesTypeId int);

		if (@movieSeriesTypeId = 0)
		begin
			insert dbo.MovieSeriesTypes
			output inserted.MovieSeriesTypeId into @output
			default values;
		end
		else
		begin		
			insert @output
			select @movieSeriesTypeId
		end;

		merge dbo.MovieSeriesTypeTitles as t
		using (
			select r.MovieSeriesTypeId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesTypeId = t.MovieSeriesTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieSeriesTypeId, Language, Title)
			values (s.MovieSeriesTypeId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		select m.MovieSeriesTypeId
			,t.Language
			,t.Title
		from dbo.MovieSeriesTypes as m
		inner join dbo.MovieSeriesTypeTitles as t on t.MovieSeriesTypeId = m.MovieSeriesTypeId
		inner join @output as o on o.MovieSeriesTypeId = m.MovieSeriesTypeId;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesTypeSave] 
GRANT EXECUTE ON [dbo].[MovieSeriesTypeSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MoviesOwnedByUserSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesOwnedByUserSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MoviesOwnedByUserSave
-- Date: 2016-09-10
-- Release: 1.0
-- Summary:
--   * Saves a movie as owned by a user.
-- Param:
--   @movieId int
--     * The id of the movie.
--   @userId int
--     * The id of the user.
--   @dvd bit
--     * If the user owns the movie on DVD.
--   @bd bit
--     * If the user owns the movie on Blu-Ray.
--   @digital bit
--     * If the user owns the movie in a digital format.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MoviesOwnedByUserSave]
	@movieId int
	,@userId int
	,@dvd bit = 0
	,@bd bit = 0
	,@digital bit = 0
as
begin
	declare @output table(MoviesOwnedByUserId int);

	if not exists (
		select 1
		from dbo.MoviesOwnedByUser
		where MovieId = @movieId
			and UserId = @userId
	)
	begin
		insert dbo.MoviesOwnedByUser
		output inserted.MoviesOwnedByUserId into @output
		select @movieId
			,@userId
			,@dvd
			,@bd
			,@digital;
	end
	else
	begin
		update dbo.MoviesOwnedByUser
		set DVD = @dvd
			,BD = @bd
			,Digital = @digital
		output inserted.MoviesOwnedByUserId into @output
		where MovieId = @movieId
			and UserId = @userId;
	end;

	select m.MovieId
		,m.UserId
		,m.DVD
		,m.BD
		,m.Digital
	from dbo.MoviesOwnedByUser as m
	inner join @output as o on o.MoviesOwnedByUserId = m.MoviesOwnedByUserId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUserSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MoviesOwnedByUserSave] 
GRANT EXECUTE ON [dbo].[MoviesOwnedByUserSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieTypeGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieTypesGet
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets the specified movie types.
-- Param:
--   @movieTypeIds dbo.IdCollection
--     * The list of ids of the movie types to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieTypeGet]
	@movieTypeIds dbo.IdCollection readonly
as
begin
	select t.MovieTypeId
	from dbo.MovieTypes t
	inner join @movieTypeIds as i on i.Id = t.MovieTypeId

	select t.MovieTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieTypes t
	inner join dbo.MovieTypeTitles as ti on ti.MovieTypeId = t.MovieTypeId
	inner join @movieTypeIds as i on i.Id = t.MovieTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieTypeGet] 
GRANT EXECUTE ON [dbo].[MovieTypeGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieTypeGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieTypesGetAll
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets all movie types.
-- Param:
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieTypeGetAll]
as
begin
	select t.MovieTypeId
	from dbo.MovieTypes t

	select t.MovieTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieTypes t
	inner join dbo.MovieTypeTitles as ti on ti.MovieTypeId = t.MovieTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieTypeGetAll] 
GRANT EXECUTE ON [dbo].[MovieTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.MovieTypeSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieTypeSave
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Saves a movie.
-- Param:
--   @movieTypeId int
--     * The id of the movie type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the movie type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieTypeSave]
	@movieTypeId int
	,@titles dbo.LanguageTitleCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection

		if (@movieTypeId = 0)
		begin
			insert dbo.MovieTypes
			output inserted.MovieTypeId into @output
			default values;
		end
		else
		begin
			insert @output
			select @movieTypeId
		end;

		merge dbo.MovieTypeTitles as t
		using (
			select r.Id as MovieTypeId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieTypeId = t.MovieTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieTypeId, Language, Title)
			values (s.MovieTypeId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		exec dbo.MovieTypeGet @output;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieTypeSave] 
GRANT EXECUTE ON [dbo].[MovieTypeSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.PersonGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.PersonGet
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PersonGet]
	@personIds dbo.IdCollection readonly
as
begin
	select p.PersonId
		,p.Name
	from dbo.People as p
	inner join @personIds as r on r.Id = p.PersonId;

	select p.PersonId
		,x.IconId
	from dbo.People as p
	inner join dbo.IconsInPeople as x on x.PersonId = p.PersonId
	inner join @personIds as r on r.Id = p.PersonId
	order by x."Order" asc;

	select x.ExternalSourceId
		,x.ExternalId
	from dbo.People as p
	inner join dbo.PersonExternalLookup as x on x.PersonId = p.PersonId
	inner join @personIds as r on r.Id = p.PersonId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[PersonGet] 
GRANT EXECUTE ON [dbo].[PersonGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.PersonSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.PersonSave
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Saves a person.
-- Param:
--   @personId int
--     * The id of the person.
--   @name nvarchar(100)
--     * The name of the person.
--   @icons dbo.IdOrderCollection
--     * The id of the icons of the person.
--   @externalLookups dbo.ExternalLookupIdCollection
--     * The id of the person in external sources.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PersonSave]
	@personId int
	,@name nvarchar(100)
	,@icons dbo.IdOrderCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@personId = 0)
		begin
			insert dbo.People
			output inserted.PersonId into @output
			select @name;
		end
		else
		begin
			update dbo.People
			set Name = @name
			output inserted.PersonId into @output
			where PersonId = @personId;
		end;

		merge dbo.IconsInPeople as t
		using (
			select r.Id as PersonId
				,x.Id as IconId
				,x."Order"
			from @icons as x
			inner join @output as r on 1 = 1
		) as s on s.PersonId = t.PersonId
			and s.IconId = t.IconId
		when not matched by target then
			insert (PersonId, IconId, "Order")
			values (s.PersonId, s.IconId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		merge dbo.PersonExternalLookup as t
		using (
			select r.Id as PersonId
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.PersonId = t.PersonId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (PersonId, ExternalSourceId, ExternalId)
			values (s.PersonId, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId
		when not matched by source then
			delete;

		exec dbo.PersonGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[PersonSave] 
GRANT EXECUTE ON [dbo].[PersonSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.PersonSearch.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.PersonSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PersonSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
	if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1)
	begin
		select top(@searchLimit) c.PersonId
		from dbo.People as c
		where Name = @searchText;
	end
	else
	begin
		select top(@searchLimit) c.PersonId
		from dbo.People as c
		where Name like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[PersonSearch] 
GRANT EXECUTE ON [dbo].[PersonSearch] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RatingSystemGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingSystemsGet
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets the specified rating systems.
-- Param:
--   @ratingSystemIds dbo.IdCollection
--     * The list of ids of the rating systems to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingSystemGet]
	@ratingSystemIds dbo.IdCollection readonly
as
begin
	select s.RatingSystemId
	from dbo.RatingSystems s
	inner join @ratingSystemIds as i on i.Id = s.RatingSystemId;

	select s.RatingSystemId
		,ti.Language
		,ti.Title
	from dbo.RatingSystems s
	inner join dbo.RatingSystemTitles as ti on ti.RatingSystemId = s.RatingSystemId
	inner join @ratingSystemIds as i on i.Id = s.RatingSystemId;

	select s.RatingSystemId
		,sv.RatingTypeId
		,sv.Weight
	from dbo.RatingSystems s
	inner join dbo.RatingSystemValues as sv on sv.RatingSystemId = s.RatingSystemId
	inner join @ratingSystemIds as i on i.Id = s.RatingSystemId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemGet] 
GRANT EXECUTE ON [dbo].[RatingSystemGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RatingSystemGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingSystemGetAll
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets all rating systems.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingSystemGetAll]
as
begin
	select s.RatingSystemId
	from dbo.RatingSystems s

	select s.RatingSystemId
		,ti.Language
		,ti.Title
	from dbo.RatingSystems s
	inner join dbo.RatingSystemTitles as ti on ti.RatingSystemId = s.RatingSystemId

	select s.RatingSystemId
		,sv.RatingTypeId
		,sv.Weight
	from dbo.RatingSystems s
	inner join dbo.RatingSystemValues as sv on sv.RatingSystemId = s.RatingSystemId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemGetAll] 
GRANT EXECUTE ON [dbo].[RatingSystemGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RatingSystemSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingSystemSave
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Saves a rating system and it''s values.
-- Param:
--   @ratingSystemId int
--     * The id of the movie type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the movie type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingSystemSave]
	@ratingSystemId int
	,@titles dbo.LanguageTitleCollection readonly
	,@values dbo.RatingSystemValueCollection readonly
as
begin
	if exists (
		select 1
		from (
			select ParentRatingTypeId
				,sum(v.Weight) as Weight
			from @values as v
			inner join dbo.RatingTypes as rt on rt.RatingTypeId = v.RatingTypeId
			group by ParentRatingTypeId
		) as x where x.Weight <> 100
	)
	begin
		;throw 50000, ''Invalid total sum. All rating system values for each parent group needs to sum to 100.'', 1;
	end;

	begin transaction;
	begin try
		declare @output dbo.IdCollection

		if (@ratingSystemId = 0)
		begin
			insert dbo.RatingSystems
			output inserted.RatingSystemId into @output
			default values;
		end
		else
		begin
			insert @output
			select @ratingSystemId
		end;

		merge dbo.RatingSystemTitles as t
		using (
			select r.Id as RatingSystemId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.RatingSystemId = t.RatingSystemId
			and s.Language = t.Language
		when not matched by target then
			insert (RatingSystemId, Language, Title)
			values (s.RatingSystemId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.RatingSystemValues as t
		using (
			select r.Id as RatingSystemId
				,v.RatingTypeId
				,v.Weight
			from @values as v
			inner join @output as r on 1 = 1
		) as s on s.RatingSystemId = t.RatingSystemId
			and s.RatingTypeId = t.RatingTypeId
		when not matched by target then
			insert (RatingSystemId, RatingTypeId, Weight)
			values (s.RatingSystemId, s.RatingTypeId, s.Weight)
		when matched then
			update
			set t.Weight = s.Weight;

		exec dbo.RatingSystemGet @output;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemSave] 
GRANT EXECUTE ON [dbo].[RatingSystemSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RatingTypeGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingTypesGet
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Gets the specified rating types.
-- Param:
--   @ratingTypeIds dbo.IdCollection
--     * The list of ids of the rating types to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingTypeGet]
	@ratingTypeIds dbo.IdCollection readonly
as
begin
	select t.RatingTypeId
	from dbo.RatingTypes t
	inner join @ratingTypeIds as i on i.Id = t.RatingTypeId

	select t.RatingTypeId
		,ti.Language
		,ti.Title
	from dbo.RatingTypes t
	inner join dbo.RatingTypeTitles as ti on ti.RatingTypeId = t.RatingTypeId
	inner join @ratingTypeIds as i on i.Id = t.RatingTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingTypeGet] 
GRANT EXECUTE ON [dbo].[RatingTypeGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RatingTypeGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingTypesGetAll
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Gets all rating types.
-- Param:
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingTypeGetAll]
as
begin
	select t.RatingTypeId
	from dbo.RatingTypes t

	select t.RatingTypeId
		,ti.Language
		,ti.Title
	from dbo.RatingTypes t
	inner join dbo.RatingTypeTitles as ti on ti.RatingTypeId = t.RatingTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingTypeGetAll] 
GRANT EXECUTE ON [dbo].[RatingTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RatingTypeSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingTypeSave
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Saves a rating.
-- Param:
--   @ratingTypeId int
--     * The id of the rating type.
--   @parentRatingTypeId int
--     * The id of the rating type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the rating type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingTypeSave]
	@ratingTypeId int
	,@parentRatingTypeId int = null
	,@titles dbo.LanguageDescriptionCollection readonly
as
begin
	if (select count(1) from @titles) = 0
	begin
		;throw 50000, ''Invalid titles count. A rating type can''''t be saved without any titles.'', 1;
	end;

	if not exists (select 1 from @titles where Language = ''en-US'')
	begin
		;throw 50000, ''Missing default title. A rating type can''''t be saved without a title in the default language (en-US).'', 1;
	end;

	begin transaction;
	begin try
		declare @output dbo.IdCollection

		if (@ratingTypeId = 0)
		begin
			insert dbo.RatingTypes
			output inserted.RatingTypeId into @output
			select @parentRatingTypeId;
		end
		else
		begin		
			insert @output
			select @ratingTypeId
		end;

		merge dbo.RatingTypeTitles as t
		using (
			select r.Id
				,mt.Language
				,mt.Title
				,mt.Description
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.Id = t.RatingTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (RatingTypeId, Language, Title, Description)
			values (s.Id, s.Language, s.Title, s.Description)
		when matched then
			update
			set t.Title = s.Title
				,t.Description = s.Description;

		exec dbo.RatingTypeGet @output;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingTypeSave] 
GRANT EXECUTE ON [dbo].[RatingTypeSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RoleGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RoleGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RoleGet
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RoleGet]
	@roleIds dbo.IdCollection readonly
as
begin
	select d.RoleId
	from dbo.Roles as d
	inner join @roleIds as i on i.Id = d.RoleId;

	select d.RoleId
		,ti.Language
		,ti.Title
	from dbo.Roles d
	inner join dbo.RoleTitles as ti on ti.RoleId = d.RoleId
	inner join @roleIds as i on i.Id = d.RoleId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RoleGet] 
GRANT EXECUTE ON [dbo].[RoleGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RoleGetAll.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RoleGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RoleGetAll
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RoleGetAll]
as
begin
	select d.RoleId
	from dbo.Roles as d;

	select d.RoleId
		,ti.Language
		,ti.Title
	from dbo.Roles d
	inner join dbo.RoleTitles as ti on ti.RoleId = d.RoleId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RoleGetAll] 
GRANT EXECUTE ON [dbo].[RoleGetAll] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.RoleSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RoleSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.RoleSave
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RoleSave]
	@roleId int
	,@titles dbo.LanguageTitleCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@roleId = 0)
		begin
			insert dbo.Roles
			output inserted.RoleId into @output
			default values;
		end
		else
		begin
			insert @output
			select @roleId;
		end;

		merge dbo.RoleTitles as t
		using (
			select r.Id as RoleId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.RoleId = t.RoleId
			and s.Language = t.Language
		when not matched by target then
			insert (RoleId, Language, Title)
			values (s.RoleId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		exec dbo.RoleGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RoleSave] 
GRANT EXECUTE ON [dbo].[RoleSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.UserGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserGet
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Gets the specified user.
-- Param:
--   @userIds dbo.IdCollection
--     * The list of ids of the users to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserGet]
	@userIds dbo.IdCollection readonly
as
begin
	select e.UserId
		,e.Username
		,e.Name
		,e.Email
	from dbo.Users e
	inner join @userIds as i on i.Id = e.UserId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserGet] 
GRANT EXECUTE ON [dbo].[UserGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.UserSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSave
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSave]
	@userId int
	,@userName nvarchar(50)
	,@name nvarchar(255)
	,@email nvarchar(255)
as
begin
		declare @output dbo.IdCollection;

		if (@userId = 0)
		begin
			insert dbo.Users
			output inserted.UserId into @output
			select @userName
				,''''
				,@name
				,@email;
		end
		else
		begin		
			update dbo.Users
			set Name = @name
				,Email = @email
			output inserted.UserId into @output
			where UserId = @userId;
		end;

		exec dbo.UserGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSave] 
GRANT EXECUTE ON [dbo].[UserSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.UserSearch.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSearch
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
	if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1 and @searchLimit = 1)
	begin
		select top 1 c.UserId
		from dbo.Users as c
		where Username = @searchText;
	end
	else if (@requireExactMatch = 1)
	begin
		select top(@searchLimit) c.UserId
		from dbo.Users as c
		where Name = @searchText
			or Username = @searchText;
	end
	else
	begin
		select top(@searchLimit) c.UserId
		from dbo.Users as c
		where Name like ''%'' + @searchText + ''%''
			or Username like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSearch] 
GRANT EXECUTE ON [dbo].[UserSearch] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.UserSessionGet.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSessionGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSessionGet
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Gets the specified user session(s).
-- Param:
--   @userSessionIds dbo.GuidCollection
--     * The list of ids of the user session(s) to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSessionGet]
	@userSessionIds dbo.GuidCollection readonly
as
begin
	select s.UserSessionId
		,s.ClientIp
		,UserId
		,ActiveFrom
		,ActiveTo
	from dbo.UserSessions s
	inner join @userSessionIds as i on i.Id = s.UserSessionId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSessionGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSessionGet] 
GRANT EXECUTE ON [dbo].[UserSessionGet] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.UserSessionSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSessionSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSessionSave
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Saves a user session.
-- Param:
--   @xxx int
--     * xxx.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSessionSave]
	@userSessionId uniqueidentifier = null
	,@clientIp nvarchar(50)
	,@userId int = null
	,@activeFrom datetime
	,@activeTo datetime
	,@username nvarchar(50) = null
	,@password nvarchar(250) = null
as
begin
		declare @output dbo.GuidCollection;
		declare @existingUserSessionId uniqueidentifier;

		select @existingUserSessionId = UserSessionId
			from dbo.UserSessions as s
			inner join dbo.Users as u on u.UserId = s.UserId
			where u.UserName = @username
				and u.Password = @password
				and s.ActiveTo > getdate();

		if (@existingUserSessionId is not null)
		begin
			insert @output
			select @existingUserSessionId

			update dbo.UserSessions
			set ActiveTo = @activeTo
		end
		else if (@userSessionId is null)
		begin
			select @userId = UserId
			from dbo.Users
			where UserName = @username
				and "Password" = @password;

			if (@userId is null)
			begin
				;throw 50000, ''The specified username and/or password is not valid.'', 1;
			end;

			insert dbo.UserSessions
			output inserted.UserSessionId into @output
			select newid()
				,@clientIp
				,@userId
				,@activeFrom
				,@activeTo
		end
		else
		begin
			update dbo.UserSessions
			set ClientIp = @clientIp
				,UserId = @userId
				,ActiveTo = @activeTo
			output inserted.UserSessionId into @output
			where UserSessionId = @userSessionId;
		end;

		exec dbo.UserSessionGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSessionSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSessionSave] 
GRANT EXECUTE ON [dbo].[UserSessionSave] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.UserSetPassword.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSetPassword 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSetPassword
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSetPassword]
	@userId int
	,@oldUserName nvarchar(50)
	,@oldPassword nvarchar(255)
	,@userName nvarchar(50)
	,@password nvarchar(255)
as
begin
		declare @output dbo.IdCollection;

		if not exists (
			select 1
			from dbo.Users
			where UserId = @userId
				and UserName = @oldUserName
				and "Password" = @oldPassword
				and cast("Password" as varbinary(4000)) = cast(@oldPassword as varbinary(4000))
		) and not exists (
			select 1
			from dbo.Users
			where UserId = @userId
				and UserName = @oldUserName
				and "Password" = ''''
		)
		begin
			;throw 50000, ''The specified username and/or password is not valid.'', 1;
		end;

		update dbo.Users
			set UserName = @userName
				,"Password" = @password
			output inserted.UserId into @output
			where UserId = @userId;

		exec dbo.UserGet @output;
end;' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSetPassword]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSetPassword] 
GRANT EXECUTE ON [dbo].[UserSetPassword] TO [rCMDB] 
GO

------------------------------------- 
-- dbo.VendorSave.PRC.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : VendorSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.VendorSave
-- Date: 2016-09-10
-- Release: 1.0
-- Summary:
--   * Saves a vendor.
-- Param:
--   @vendorId int
--     * The id of the vendor.
--   @name nvarchar(100)
--     * The name of the vendor.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[VendorSave]
	@vendorId int
	,@name nvarchar(100)
as
begin
	declare @output table(VendorId int);

	if (@vendorId = 0)
	begin
		insert dbo.Vendors
		output inserted.VendorId into @output
		select @name;
	end
	else
	begin
		update dbo.Vendors
		set Name = @name
		output inserted.VendorId into @output
		where VendorId = @vendorId;
	end;

	select v.VendorId
		,v.Name
	from dbo.Vendors as v
	inner join @output as o on o.VendorId = v.VendorId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VendorSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[VendorSave] 
GRANT EXECUTE ON [dbo].[VendorSave] TO [rCMDB] 
GO

------------------------------------- 
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
-- edu.CMDB.sql 
------------------------------------------------------------ 
------------------------------------------------------------ 
--- EyeDoc SQL script- CMDB.edu                       
------------------------------------------------------------ 
--- Scheme    : edu 
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
-- edu.schema.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : edu 
-- Type     : schema 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'edu')
BEGIN
	EXEC sys.sp_executesql N'CREATE SCHEMA [edu]'

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
-- edu.Bounties.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Bounties 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Bounties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Bounties](
		[BountyId] [int] IDENTITY(1,1) NOT NULL,
		[BountyTypeId] [int] NOT NULL,
		[BountyValue] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_Bounties] PRIMARY KEY CLUSTERED 
	(
		[BountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Bounties_BountyTypes]') AND parent_object_id = OBJECT_ID(N'[edu].[Bounties]'))
BEGIN
	ALTER TABLE [edu].[Bounties] CHECK CONSTRAINT [FK_Bounties_BountyTypes]
END 

GO

------------------------------------- 
-- edu.BountyTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : BountyTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[BountyTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[BountyTypes](
		[BountyTypeId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_BountyTypes] PRIMARY KEY CLUSTERED 
	(
		[BountyTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- edu.Subjects.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Subjects 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Subjects]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Subjects](
		[SubjectId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
	(
		[SubjectId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- edu.TaskBounties.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : TaskBounties 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[TaskBounties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[TaskBounties](
		[TaskBountyId] [int] IDENTITY(1,1) NOT NULL,
		[TaskId] [int] NOT NULL,
		[BountyId] [int] NOT NULL,
		[DurationInMinutes] [int] NOT NULL,
	 CONSTRAINT [PK_TaskBounties] PRIMARY KEY CLUSTERED 
	(
		[TaskBountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Bounties]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties] CHECK CONSTRAINT [FK_TaskBounties_Bounties]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Tasks]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties] CHECK CONSTRAINT [FK_TaskBounties_Tasks]
END 

GO

------------------------------------- 
-- edu.Tasks.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Tasks 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Tasks]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Tasks](
		[TaskId] [int] IDENTITY(1,1) NOT NULL,
		[SubjectId] [int] NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
	(
		[TaskId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Tasks_Subjects]') AND parent_object_id = OBJECT_ID(N'[edu].[Tasks]'))
BEGIN
	ALTER TABLE [edu].[Tasks] CHECK CONSTRAINT [FK_Tasks_Subjects]
END 

GO

------------------------------------- 
-- edu.UserTaskBounties.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : UserTaskBounties 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[UserTaskBounties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[UserTaskBounties](
		[UserTaskBountyId] [int] IDENTITY(1,1) NOT NULL,
		[UserId] [int] NOT NULL,
		[TaskBountyId] [int] NOT NULL,
		[AssignedBy] [int] NOT NULL,
		[AssignedAt] [datetime2](0) NOT NULL,
		[StartedAt] [datetime2](0) NULL,
		[CompletedAt] [datetime2](0) NULL,
		[AwardedAt] [datetime2](0) NULL,
		[AwardedBy] [int] NULL,
		[Result] [nvarchar](max) NULL,
	 CONSTRAINT [PK_UserTaskBounties] PRIMARY KEY CLUSTERED 
	(
		[UserTaskBountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_TaskBounties]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_TaskBounties]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users1]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users1]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users2]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users2]
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
-- edu.Bounties.FK_Bounties_BountyTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : Bounties 
-- name     : FK_Bounties_BountyTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Bounties_BountyTypes]') AND parent_object_id = OBJECT_ID(N'[edu].[Bounties]'))
BEGIN
	ALTER TABLE [edu].[Bounties]  WITH CHECK ADD CONSTRAINT [FK_Bounties_BountyTypes] FOREIGN KEY([BountyTypeId])
	REFERENCES [edu].[BountyTypes] ([BountyTypeId])

END 

GO

------------------------------------- 
-- edu.TaskBounties.FK_TaskBounties_Bounties.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : FK_TaskBounties_Bounties 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Bounties]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties]  WITH CHECK ADD CONSTRAINT [FK_TaskBounties_Bounties] FOREIGN KEY([BountyId])
	REFERENCES [edu].[Bounties] ([BountyId])

END 

GO

------------------------------------- 
-- edu.TaskBounties.FK_TaskBounties_Tasks.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : FK_TaskBounties_Tasks 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Tasks]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties]  WITH CHECK ADD CONSTRAINT [FK_TaskBounties_Tasks] FOREIGN KEY([TaskId])
	REFERENCES [edu].[Tasks] ([TaskId])

END 

GO

------------------------------------- 
-- edu.Tasks.FK_Tasks_Subjects.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : Tasks 
-- name     : FK_Tasks_Subjects 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Tasks_Subjects]') AND parent_object_id = OBJECT_ID(N'[edu].[Tasks]'))
BEGIN
	ALTER TABLE [edu].[Tasks]  WITH CHECK ADD CONSTRAINT [FK_Tasks_Subjects] FOREIGN KEY([SubjectId])
	REFERENCES [edu].[Subjects] ([SubjectId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_TaskBounties.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_TaskBounties 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_TaskBounties]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_TaskBounties] FOREIGN KEY([TaskBountyId])
	REFERENCES [edu].[TaskBounties] ([TaskBountyId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_Users1.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_Users1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users1]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_Users1] FOREIGN KEY([AssignedBy])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_Users2.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_Users2 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users2]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_Users2] FOREIGN KEY([AwardedBy])
	REFERENCES [dbo].[Users] ([UserId])

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
-- edu.TaskBounties.UX_TaskBounties.IX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : UX_TaskBounties 
-- Type     : nonuniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[edu].[TaskBounties]') AND name = N'UX_TaskBounties')
BEGIN
	CREATE NONCLUSTERED INDEX [UX_TaskBounties] ON [edu].[TaskBounties]
	(
		[TaskId] ASC,
		[BountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
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
-- music.CMDB.sql 
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
