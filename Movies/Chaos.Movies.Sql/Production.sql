USE [master]
GO
/****** Object:  Database [CMDB]    Script Date: 2018-06-14 23:18:00 ******/
CREATE DATABASE [CMDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CMDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\CMDB.mdf' , SIZE = 16130048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CMDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\CMDB_log.ldf' , SIZE = 11276224KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [CMDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CMDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CMDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CMDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CMDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CMDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CMDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CMDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [CMDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CMDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CMDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CMDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CMDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CMDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CMDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CMDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CMDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CMDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CMDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CMDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CMDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CMDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CMDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CMDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CMDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CMDB] SET  MULTI_USER 
GO
ALTER DATABASE [CMDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CMDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CMDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CMDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CMDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CMDB] SET QUERY_STORE = OFF
GO
USE [CMDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [CMDB]
GO
/****** Object:  DatabaseRole [rCMDB]    Script Date: 2018-06-14 23:18:00 ******/
CREATE ROLE [rCMDB]
GO
/****** Object:  Schema [edu]    Script Date: 2018-06-14 23:18:00 ******/
CREATE SCHEMA [edu]
GO
/****** Object:  Schema [music]    Script Date: 2018-06-14 23:18:00 ******/
CREATE SCHEMA [music]
GO
/****** Object:  Schema [rCMDB]    Script Date: 2018-06-14 23:18:00 ******/
CREATE SCHEMA [rCMDB]
GO
/****** Object:  Schema [ximport]    Script Date: 2018-06-14 23:18:00 ******/
CREATE SCHEMA [ximport]
GO
/****** Object:  UserDefinedTableType [dbo].[ExternalLookupIdCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[ExternalLookupIdCollection] AS TABLE(
	[ExternalSourceId] [int] NOT NULL,
	[ExternalId] [varchar](50) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ExternalSourceId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[ExternalRatingCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[ExternalRatingCollection] AS TABLE(
	[ExternalSourceId] [int] NOT NULL,
	[ExternalRating] [float] NOT NULL,
	[RatingCount] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[ExternalSourceId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[GuidCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[GuidCollection] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[IdCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[IdCollection] AS TABLE(
	[Id] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[IdOrderCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[IdOrderCollection] AS TABLE(
	[Id] [int] NOT NULL,
	[Order] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[LanguageDescriptionCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[LanguageDescriptionCollection] AS TABLE(
	[Language] [varchar](8) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Language] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[LanguagesTitlesCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[LanguagesTitlesCollection] AS TABLE(
	[InLanguage] [varchar](8) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[InLanguage] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[LanguageTitleCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[LanguageTitleCollection] AS TABLE(
	[Language] [varchar](8) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Language] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[RatingSystemValueCollection]    Script Date: 2018-06-14 23:18:00 ******/
CREATE TYPE [dbo].[RatingSystemValueCollection] AS TABLE(
	[RatingTypeId] [int] NOT NULL,
	[Weight] [tinyint] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[RatingTypeId] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  Table [dbo].[Acquisitions]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[ActorsAsCharacters]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActorsAsCharacters](
	[ActorAsCharacterId] [int] IDENTITY(1,1) NOT NULL,
	[PersonInMovieId] [int] NOT NULL,
	[CharacterId] [int] NOT NULL,
 CONSTRAINT [PK_ActorsAsCharacters] PRIMARY KEY CLUSTERED 
(
	[ActorAsCharacterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_ActorsAsCharacters] UNIQUE NONCLUSTERED 
(
	[CharacterId] ASC,
	[PersonInMovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BoxOffice]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[BoxOfficeTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BoxOfficeTypes](
	[BoxOfficeTypeId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_BoxOfficeTypes] PRIMARY KEY CLUSTERED 
(
	[BoxOfficeTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BoxOfficeTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[CharacterExternalLookup]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CharacterExternalLookup](
	[CharacterId] [int] NOT NULL,
	[ExternalSourceId] [int] NOT NULL,
	[ExternalId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CharacterExternalLookup] PRIMARY KEY CLUSTERED 
(
	[CharacterId] ASC,
	[ExternalSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_CharacterExternalLookup] UNIQUE NONCLUSTERED 
(
	[CharacterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Characters]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Characters](
	[CharacterId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Characters] PRIMARY KEY CLUSTERED 
(
	[CharacterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepartmentTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[Errors]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[ExternalSourceMovieTypeAddress]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[ExternalSources]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[FavoriteTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FavoriteTypes](
	[FavoriteTypeId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_FavoriteTypes] PRIMARY KEY CLUSTERED 
(
	[FavoriteTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FavoriteTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FavoriteTypeTitles](
	[FavoriteTypeId] [int] NOT NULL,
	[Language] [varchar](8) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_FavoriteTypeTitles] PRIMARY KEY CLUSTERED 
(
	[FavoriteTypeId] ASC,
	[Language] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_FavoriteTypeTitles] UNIQUE NONCLUSTERED 
(
	[FavoriteTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GenreExternalLookup]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GenreExternalLookup](
	[GenreId] [int] NOT NULL,
	[ExternalSourceId] [int] NOT NULL,
	[ExternalId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_GenreExternalLookup] PRIMARY KEY CLUSTERED 
(
	[GenreId] ASC,
	[ExternalSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_GenreExternalLookup] UNIQUE NONCLUSTERED 
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genres]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genres](
	[GenreId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GenreTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[Hashtags]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hashtags](
	[Hashtag] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Hashtags] PRIMARY KEY CLUSTERED 
(
	[Hashtag] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Icons]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[IconsInCharacters]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[IconsInMovies]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[IconsInMovieSeries]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[IconsInPeople]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[IconTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IconTypes](
	[IconTypeId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_IconTypes] PRIMARY KEY CLUSTERED 
(
	[IconTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IconTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[ImportActorsInMovies]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportActorsInMovies](
	[tconst] [nvarchar](50) NULL,
	[ordering] [nvarchar](50) NULL,
	[nconst] [nvarchar](50) NULL,
	[category] [nvarchar](250) NULL,
	[job] [nvarchar](250) NULL,
	[characters] [nvarchar](1000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportEpisodes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportEpisodes](
	[tconst] [varchar](50) NULL,
	[parentTconst] [varchar](50) NULL,
	[seasonNumber] [varchar](50) NULL,
	[episodeNumber] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportLanguageTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportLanguageTitles](
	[titleId] [nvarchar](50) NULL,
	[ordering] [nvarchar](50) NULL,
	[title] [nvarchar](4000) NULL,
	[region] [nvarchar](50) NULL,
	[language] [nvarchar](50) NULL,
	[types] [nvarchar](250) NULL,
	[attributes] [nvarchar](500) NULL,
	[isOriginalTitle] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportMovies]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportMovies](
	[tconst] [varchar](50) NULL,
	[titleType] [varchar](50) NULL,
	[primaryTitle] [varchar](500) NULL,
	[originalTitle] [varchar](500) NULL,
	[isAdult] [varchar](50) NULL,
	[startYear] [varchar](50) NULL,
	[endYear] [varchar](50) NULL,
	[runtimeMinutes] [varchar](50) NULL,
	[genres] [varchar](250) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportWriterDirector]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportWriterDirector](
	[tconst] [varchar](50) NULL,
	[directors] [varchar](8000) NULL,
	[writers] [varchar](8000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[Language] [varchar](8) NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[Language] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LanguageTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MovieExternalLookup]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieExternalLookup](
	[MovieId] [int] NOT NULL,
	[ExternalSourceId] [int] NOT NULL,
	[ExternalId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MovieExternalLookup] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC,
	[ExternalSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_MovieExternalLookup] UNIQUE NONCLUSTERED 
(
	[ExternalSourceId] ASC,
	[ExternalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieExternalRatings]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[Movies]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movies](
	[MovieId] [int] IDENTITY(1,1) NOT NULL,
	[MovieTypeId] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[EndYear] [int] NOT NULL,
	[RunTime] [int] NOT NULL,
	[ImdbId] [nvarchar](50) NULL,
 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieSeries]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieSeries](
	[MovieSeriesId] [int] IDENTITY(1,1) NOT NULL,
	[MovieSeriesTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Collections] PRIMARY KEY CLUSTERED 
(
	[MovieSeriesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieSeriesTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MovieSeriesTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieSeriesTypes](
	[MovieSeriesTypeId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_CollectionTypes] PRIMARY KEY CLUSTERED 
(
	[MovieSeriesTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieSeriesTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MoviesInGenres]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MoviesInGenres](
	[MovieId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_MoviesInGenres] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MoviesInMovieSeries]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MoviesOwnedByUser]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MoviesWatchedByUser]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MovieTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MovieTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieTypes](
	[MovieTypeId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_MovieTypes] PRIMARY KEY CLUSTERED 
(
	[MovieTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[People]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[BirthDate] [date] NULL,
	[DeathDate] [date] NULL,
	[ImdbId] [nvarchar](50) NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeopleInMovies]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeopleInMovies](
	[PersonInMovieId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[MovieId] [int] NOT NULL,
	[RoleInDepartmentId] [int] NOT NULL,
 CONSTRAINT [PK_PeopleInMovies] PRIMARY KEY CLUSTERED 
(
	[PersonInMovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_PeopleInMovies] UNIQUE NONCLUSTERED 
(
	[PersonId] ASC,
	[MovieId] ASC,
	[RoleInDepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonExternalLookup]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonExternalLookup](
	[PersonId] [int] NOT NULL,
	[ExternalSourceId] [int] NOT NULL,
	[ExternalId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PersonExternalLookup] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC,
	[ExternalSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_PersonExternalLookup] UNIQUE NONCLUSTERED 
(
	[ExternalSourceId] ASC,
	[ExternalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RatingSystems]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RatingSystems](
	[RatingSystemId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_RatingSystems] PRIMARY KEY CLUSTERED 
(
	[RatingSystemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RatingSystemTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[RatingSystemValues]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[RatingTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RatingTypes](
	[RatingTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ParentRatingTypeId] [int] NULL,
 CONSTRAINT [PK_RatingTypes] PRIMARY KEY CLUSTERED 
(
	[RatingTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RatingTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolesInDepartments]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[RoleTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[Seasons]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seasons](
	[SeasonId] [int] IDENTITY(1,1) NOT NULL,
	[MovieId] [int] NOT NULL,
	[Number] [smallint] NOT NULL,
 CONSTRAINT [PK_Seasons] PRIMARY KEY CLUSTERED 
(
	[SeasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCharacterFavorites]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserCharacterInMovieRatings]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCharacterInMovieRatings](
	[UserId] [int] NOT NULL,
	[ActorAsCharacterId] [int] NOT NULL,
	[Rating] [tinyint] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_UserCharacterInMovieRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ActorAsCharacterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMovieFavorites]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserMovieGenres]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserMovieHashtags]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserMovieRatings]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserMovieReviews]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserMovieWishlist]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserPeopleFavorites]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserPersonInMovieRoleRatings]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPersonInMovieRoleRatings](
	[UserId] [int] NOT NULL,
	[PersonInMovieId] [int] NOT NULL,
	[Rating] [tinyint] NOT NULL,
	[CreatedDate] [date] NOT NULL,
 CONSTRAINT [PK_UserPersonInMovieRoleRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[PersonInMovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[UserSessions]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[WatchTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WatchTypes](
	[WatchTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_WatchTypes] PRIMARY KEY CLUSTERED 
(
	[WatchTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WatchTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WatchTypeTitles](
	[WatchTypeId] [int] NOT NULL,
	[Language] [varchar](8) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_WatchTypeTitles] PRIMARY KEY CLUSTERED 
(
	[WatchTypeId] ASC,
	[Language] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_WatchTypeTitles] UNIQUE NONCLUSTERED 
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors](
	[VendorId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Vendors] PRIMARY KEY CLUSTERED 
(
	[VendorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VoiceOverActors]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VoiceOverActors](
	[PersonId] [int] NOT NULL,
	[ActorAsCharacterId] [int] NOT NULL,
	[Language] [varchar](8) NOT NULL,
	[CharacterName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_VoiceOverActors] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC,
	[ActorAsCharacterId] ASC,
	[Language] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [edu].[Bounties]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [edu].[Bounties](
	[BountyId] [int] IDENTITY(1,1) NOT NULL,
	[BountyTypeId] [int] NOT NULL,
	[BountyValue] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Bounties] PRIMARY KEY CLUSTERED 
(
	[BountyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [edu].[BountyTypes]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [edu].[BountyTypes](
	[BountyTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_BountyTypes] PRIMARY KEY CLUSTERED 
(
	[BountyTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [edu].[Subjects]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [edu].[Subjects](
	[SubjectId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
(
	[SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [edu].[TaskBounties]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [edu].[Tasks]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [edu].[UserTaskBounties]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [music].[Albums]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [music].[Albums](
	[AlbumId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
(
	[AlbumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [music].[AlbumsInGenres]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [music].[AlbumsInGenres](
	[AlbumId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_AlbumsInGenres] PRIMARY KEY CLUSTERED 
(
	[AlbumId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [music].[Artists]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [music].[Artists](
	[ArtistId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED 
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [music].[ArtistsInGenres]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [music].[ArtistsInGenres](
	[ArtistId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_ArtistsInGenres] PRIMARY KEY CLUSTERED 
(
	[ArtistId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [music].[Genres]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [music].[Genres](
	[GenreId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [music].[GenreTitles]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [music].[Songs]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [music].[SongsInGenres]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [music].[SongsInGenres](
	[SongId] [int] NOT NULL,
	[GenreId] [int] NOT NULL,
 CONSTRAINT [PK_SongsInGenres] PRIMARY KEY CLUSTERED 
(
	[SongId] ASC,
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [music].[SongsInMovies]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [music].[SongsInMovies](
	[SongId] [int] NOT NULL,
	[MovieId] [int] NOT NULL,
 CONSTRAINT [PK_SongsInMovies] PRIMARY KEY CLUSTERED 
(
	[SongId] ASC,
	[MovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_BoxOfficeTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_BoxOfficeTypeTitles] ON [dbo].[BoxOfficeTypeTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Characters_Name]    Script Date: 2018-06-14 23:18:00 ******/
CREATE NONCLUSTERED INDEX [IX_Characters_Name] ON [dbo].[Characters]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_DepartmentTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_DepartmentTitles] ON [dbo].[DepartmentTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_GenreTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_GenreTitles] ON [dbo].[GenreTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_IconTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_IconTypeTitles] ON [dbo].[IconTypeTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ImportActorsInMoviesCharactersIndex]    Script Date: 2018-06-14 23:18:00 ******/
CREATE NONCLUSTERED INDEX [ImportActorsInMoviesCharactersIndex] ON [dbo].[ImportActorsInMovies]
(
	[category] ASC
)
INCLUDE ( 	[characters]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ImportActorsInMoviesPersonIndex]    Script Date: 2018-06-14 23:18:00 ******/
CREATE NONCLUSTERED INDEX [ImportActorsInMoviesPersonIndex] ON [dbo].[ImportActorsInMovies]
(
	[nconst] ASC
)
INCLUDE ( 	[tconst],
	[category]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_LanguageTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_LanguageTitles] ON [dbo].[LanguageTitles]
(
	[InLanguage] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Movies_ImdbId]    Script Date: 2018-06-14 23:18:00 ******/
CREATE NONCLUSTERED INDEX [IX_Movies_ImdbId] ON [dbo].[Movies]
(
	[ImdbId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_MovieSeriesTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieSeriesTitles] ON [dbo].[MovieSeriesTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_MovieSeriesTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieSeriesTypeTitles] ON [dbo].[MovieSeriesTypeTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_MovieTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieTypeTitles] ON [dbo].[MovieTypeTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_People_ImdbId]    Script Date: 2018-06-14 23:18:00 ******/
CREATE NONCLUSTERED INDEX [IX_People_ImdbId] ON [dbo].[People]
(
	[ImdbId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_RatingSystemTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_RatingSystemTitles] ON [dbo].[RatingSystemTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_RatingTypeTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_RatingTypeTitles] ON [dbo].[RatingTypeTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_RoleTitles]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_RoleTitles] ON [dbo].[RoleTitles]
(
	[Language] ASC,
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UX_SeasonNumbers]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_SeasonNumbers] ON [dbo].[Seasons]
(
	[MovieId] ASC,
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_WatchTypes]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_WatchTypes] ON [dbo].[WatchTypes]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Vendors]    Script Date: 2018-06-14 23:18:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Vendors] ON [dbo].[Vendors]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UX_TaskBounties]    Script Date: 2018-06-14 23:18:00 ******/
CREATE NONCLUSTERED INDEX [UX_TaskBounties] ON [edu].[TaskBounties]
(
	[TaskId] ASC,
	[BountyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Acquisitions] ADD  CONSTRAINT [DF_Acquisitions_DateUncertain]  DEFAULT ((0)) FOR [DateUncertain]
GO
ALTER TABLE [dbo].[MovieExternalRatings] ADD  CONSTRAINT [DF_MovieExternalRatings_ExternalRating]  DEFAULT ((0)) FOR [ExternalRating]
GO
ALTER TABLE [dbo].[Movies] ADD  CONSTRAINT [DF_Movies_ImdbId]  DEFAULT ('') FOR [ImdbId]
GO
ALTER TABLE [dbo].[MoviesWatchedByUser] ADD  CONSTRAINT [DF_MoviesWatchedByUser_DateUncertain]  DEFAULT ((0)) FOR [DateUncertain]
GO
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_People_ImdbId]  DEFAULT ('') FOR [ImdbId]
GO
ALTER TABLE [dbo].[UserCharacterInMovieRatings] ADD  CONSTRAINT [DF_UserCharacterInMovieRatings_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[UserMovieGenres] ADD  CONSTRAINT [DF_UserMovieGenres_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[UserMovieRatings] ADD  CONSTRAINT [DF_UserMovieRatings_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[UserMovieWishlist] ADD  CONSTRAINT [DF_UserMovieWishlist_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] ADD  CONSTRAINT [DF_UserPersonInMovieRoleRatings_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[Acquisitions]  WITH CHECK ADD  CONSTRAINT [FK_Acquisitions_MoviesOwnedByUser] FOREIGN KEY([MoviesOwnedByUserId])
REFERENCES [dbo].[MoviesOwnedByUser] ([MoviesOwnedByUserId])
GO
ALTER TABLE [dbo].[Acquisitions] CHECK CONSTRAINT [FK_Acquisitions_MoviesOwnedByUser]
GO
ALTER TABLE [dbo].[Acquisitions]  WITH CHECK ADD  CONSTRAINT [FK_Acquisitions_Vendors] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendors] ([VendorId])
GO
ALTER TABLE [dbo].[Acquisitions] CHECK CONSTRAINT [FK_Acquisitions_Vendors]
GO
ALTER TABLE [dbo].[ActorsAsCharacters]  WITH CHECK ADD  CONSTRAINT [FK_ActorsAsCharacters_Characters] FOREIGN KEY([CharacterId])
REFERENCES [dbo].[Characters] ([CharacterId])
GO
ALTER TABLE [dbo].[ActorsAsCharacters] CHECK CONSTRAINT [FK_ActorsAsCharacters_Characters]
GO
ALTER TABLE [dbo].[ActorsAsCharacters]  WITH CHECK ADD  CONSTRAINT [FK_ActorsAsCharacters_PeopleInMovies] FOREIGN KEY([PersonInMovieId])
REFERENCES [dbo].[PeopleInMovies] ([PersonInMovieId])
GO
ALTER TABLE [dbo].[ActorsAsCharacters] CHECK CONSTRAINT [FK_ActorsAsCharacters_PeopleInMovies]
GO
ALTER TABLE [dbo].[BoxOffice]  WITH CHECK ADD  CONSTRAINT [FK_BoxOffice_BoxOfficeTypes] FOREIGN KEY([BoxOfficeTypeId])
REFERENCES [dbo].[BoxOfficeTypes] ([BoxOfficeTypeId])
GO
ALTER TABLE [dbo].[BoxOffice] CHECK CONSTRAINT [FK_BoxOffice_BoxOfficeTypes]
GO
ALTER TABLE [dbo].[BoxOffice]  WITH CHECK ADD  CONSTRAINT [FK_BoxOffice_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[BoxOffice] CHECK CONSTRAINT [FK_BoxOffice_Movies]
GO
ALTER TABLE [dbo].[BoxOfficeTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_BoxOfficeTypeTitles_BoxOfficeTypes] FOREIGN KEY([BoxOfficeTypeId])
REFERENCES [dbo].[BoxOfficeTypes] ([BoxOfficeTypeId])
GO
ALTER TABLE [dbo].[BoxOfficeTypeTitles] CHECK CONSTRAINT [FK_BoxOfficeTypeTitles_BoxOfficeTypes]
GO
ALTER TABLE [dbo].[BoxOfficeTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_BoxOfficeTypeTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[BoxOfficeTypeTitles] CHECK CONSTRAINT [FK_BoxOfficeTypeTitles_Languages]
GO
ALTER TABLE [dbo].[CharacterExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_CharacterExternalLookup_Characters] FOREIGN KEY([CharacterId])
REFERENCES [dbo].[Characters] ([CharacterId])
GO
ALTER TABLE [dbo].[CharacterExternalLookup] CHECK CONSTRAINT [FK_CharacterExternalLookup_Characters]
GO
ALTER TABLE [dbo].[CharacterExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_CharacterExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])
GO
ALTER TABLE [dbo].[CharacterExternalLookup] CHECK CONSTRAINT [FK_CharacterExternalLookup_ExternalSources]
GO
ALTER TABLE [dbo].[DepartmentTitles]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentTitles_Departments] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([DepartmentId])
GO
ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Departments]
GO
ALTER TABLE [dbo].[DepartmentTitles]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Languages]
GO
ALTER TABLE [dbo].[DepartmentTitles]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentTitles_Languages1] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Languages1]
GO
ALTER TABLE [dbo].[Errors]  WITH CHECK ADD  CONSTRAINT [FK_Exceptions_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Errors] CHECK CONSTRAINT [FK_Exceptions_Users]
GO
ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress]  WITH CHECK ADD  CONSTRAINT [FK_ExternalSourceMovieTypeAddress_ExternalSources] FOREIGN KEY([ExternalSourceId])
REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])
GO
ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress] CHECK CONSTRAINT [FK_ExternalSourceMovieTypeAddress_ExternalSources]
GO
ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress]  WITH CHECK ADD  CONSTRAINT [FK_ExternalSourceMovieTypeAddress_MovieTypes] FOREIGN KEY([MovieTypeId])
REFERENCES [dbo].[MovieTypes] ([MovieTypeId])
GO
ALTER TABLE [dbo].[ExternalSourceMovieTypeAddress] CHECK CONSTRAINT [FK_ExternalSourceMovieTypeAddress_MovieTypes]
GO
ALTER TABLE [dbo].[FavoriteTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_FavoriteTypeTitles_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])
GO
ALTER TABLE [dbo].[FavoriteTypeTitles] CHECK CONSTRAINT [FK_FavoriteTypeTitles_FavoriteTypes]
GO
ALTER TABLE [dbo].[FavoriteTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_FavoriteTypeTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[FavoriteTypeTitles] CHECK CONSTRAINT [FK_FavoriteTypeTitles_Languages]
GO
ALTER TABLE [dbo].[GenreExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_GenreExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])
GO
ALTER TABLE [dbo].[GenreExternalLookup] CHECK CONSTRAINT [FK_GenreExternalLookup_ExternalSources]
GO
ALTER TABLE [dbo].[GenreExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_GenreExternalLookup_Genres] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genres] ([GenreId])
GO
ALTER TABLE [dbo].[GenreExternalLookup] CHECK CONSTRAINT [FK_GenreExternalLookup_Genres]
GO
ALTER TABLE [dbo].[GenreTitles]  WITH CHECK ADD  CONSTRAINT [FK_GenreTitles_Genres] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genres] ([GenreId])
GO
ALTER TABLE [dbo].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Genres]
GO
ALTER TABLE [dbo].[GenreTitles]  WITH CHECK ADD  CONSTRAINT [FK_GenreTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Languages]
GO
ALTER TABLE [dbo].[Icons]  WITH CHECK ADD  CONSTRAINT [FK_Icons_IconTypes] FOREIGN KEY([IconTypeId])
REFERENCES [dbo].[IconTypes] ([IconTypeId])
GO
ALTER TABLE [dbo].[Icons] CHECK CONSTRAINT [FK_Icons_IconTypes]
GO
ALTER TABLE [dbo].[IconsInCharacters]  WITH CHECK ADD  CONSTRAINT [FK_IconsInCharacters_Characters] FOREIGN KEY([CharacterId])
REFERENCES [dbo].[Characters] ([CharacterId])
GO
ALTER TABLE [dbo].[IconsInCharacters] CHECK CONSTRAINT [FK_IconsInCharacters_Characters]
GO
ALTER TABLE [dbo].[IconsInCharacters]  WITH CHECK ADD  CONSTRAINT [FK_IconsInCharacters_Icons] FOREIGN KEY([IconId])
REFERENCES [dbo].[Icons] ([IconId])
GO
ALTER TABLE [dbo].[IconsInCharacters] CHECK CONSTRAINT [FK_IconsInCharacters_Icons]
GO
ALTER TABLE [dbo].[IconsInMovies]  WITH CHECK ADD  CONSTRAINT [FK_IconsInMovies_Icons] FOREIGN KEY([IconId])
REFERENCES [dbo].[Icons] ([IconId])
GO
ALTER TABLE [dbo].[IconsInMovies] CHECK CONSTRAINT [FK_IconsInMovies_Icons]
GO
ALTER TABLE [dbo].[IconsInMovies]  WITH CHECK ADD  CONSTRAINT [FK_IconsInMovies_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[IconsInMovies] CHECK CONSTRAINT [FK_IconsInMovies_Movies]
GO
ALTER TABLE [dbo].[IconsInMovieSeries]  WITH CHECK ADD  CONSTRAINT [FK_IconsInCollections_Collections] FOREIGN KEY([MovieSeriesId])
REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])
GO
ALTER TABLE [dbo].[IconsInMovieSeries] CHECK CONSTRAINT [FK_IconsInCollections_Collections]
GO
ALTER TABLE [dbo].[IconsInMovieSeries]  WITH CHECK ADD  CONSTRAINT [FK_IconsInCollections_Icons] FOREIGN KEY([IconId])
REFERENCES [dbo].[Icons] ([IconId])
GO
ALTER TABLE [dbo].[IconsInMovieSeries] CHECK CONSTRAINT [FK_IconsInCollections_Icons]
GO
ALTER TABLE [dbo].[IconsInPeople]  WITH CHECK ADD  CONSTRAINT [FK_IconsInPeople_Icons] FOREIGN KEY([IconId])
REFERENCES [dbo].[Icons] ([IconId])
GO
ALTER TABLE [dbo].[IconsInPeople] CHECK CONSTRAINT [FK_IconsInPeople_Icons]
GO
ALTER TABLE [dbo].[IconsInPeople]  WITH CHECK ADD  CONSTRAINT [FK_IconsInPeople_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[IconsInPeople] CHECK CONSTRAINT [FK_IconsInPeople_People]
GO
ALTER TABLE [dbo].[IconTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_IconTypeTitles_IconTypes] FOREIGN KEY([IconTypeId])
REFERENCES [dbo].[IconTypes] ([IconTypeId])
GO
ALTER TABLE [dbo].[IconTypeTitles] CHECK CONSTRAINT [FK_IconTypeTitles_IconTypes]
GO
ALTER TABLE [dbo].[IconTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_IconTypeTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[IconTypeTitles] CHECK CONSTRAINT [FK_IconTypeTitles_Languages]
GO
ALTER TABLE [dbo].[LanguageTitles]  WITH CHECK ADD  CONSTRAINT [FK_LanguageTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[LanguageTitles] CHECK CONSTRAINT [FK_LanguageTitles_Languages]
GO
ALTER TABLE [dbo].[LanguageTitles]  WITH CHECK ADD  CONSTRAINT [FK_LanguageTitles_LanguagesIn] FOREIGN KEY([InLanguage])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[LanguageTitles] CHECK CONSTRAINT [FK_LanguageTitles_LanguagesIn]
GO
ALTER TABLE [dbo].[MovieExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_MovieExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])
GO
ALTER TABLE [dbo].[MovieExternalLookup] CHECK CONSTRAINT [FK_MovieExternalLookup_ExternalSources]
GO
ALTER TABLE [dbo].[MovieExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_MovieExternalLookup_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[MovieExternalLookup] CHECK CONSTRAINT [FK_MovieExternalLookup_Movies]
GO
ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD  CONSTRAINT [FK_MovieExternalRatings_ExternalSources] FOREIGN KEY([ExternalSourceId])
REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])
GO
ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [FK_MovieExternalRatings_ExternalSources]
GO
ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD  CONSTRAINT [FK_MovieExternalRatings_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [FK_MovieExternalRatings_Movies]
GO
ALTER TABLE [dbo].[Movies]  WITH CHECK ADD  CONSTRAINT [FK_Movies_MovieTypes] FOREIGN KEY([MovieTypeId])
REFERENCES [dbo].[MovieTypes] ([MovieTypeId])
GO
ALTER TABLE [dbo].[Movies] CHECK CONSTRAINT [FK_Movies_MovieTypes]
GO
ALTER TABLE [dbo].[MovieSeries]  WITH CHECK ADD  CONSTRAINT [FK_Collections_CollectionTypes] FOREIGN KEY([MovieSeriesTypeId])
REFERENCES [dbo].[MovieSeriesTypes] ([MovieSeriesTypeId])
GO
ALTER TABLE [dbo].[MovieSeries] CHECK CONSTRAINT [FK_Collections_CollectionTypes]
GO
ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD  CONSTRAINT [FK_CollectionTitles_Collections] FOREIGN KEY([MovieSeriesId])
REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])
GO
ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_CollectionTitles_Collections]
GO
ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieSeriesTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_MovieSeriesTitles_Languages]
GO
ALTER TABLE [dbo].[MovieSeriesTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieSeriesTitles_MovieSeriesTitles] FOREIGN KEY([MovieSeriesId], [Language])
REFERENCES [dbo].[MovieSeriesTitles] ([MovieSeriesId], [Language])
GO
ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_MovieSeriesTitles_MovieSeriesTitles]
GO
ALTER TABLE [dbo].[MovieSeriesTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieSeriesTypeTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[MovieSeriesTypeTitles] CHECK CONSTRAINT [FK_MovieSeriesTypeTitles_Languages]
GO
ALTER TABLE [dbo].[MovieSeriesTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieSeriesTypeTitles_MovieSeriesTypes] FOREIGN KEY([MovieSeriesTypeId])
REFERENCES [dbo].[MovieSeriesTypes] ([MovieSeriesTypeId])
GO
ALTER TABLE [dbo].[MovieSeriesTypeTitles] CHECK CONSTRAINT [FK_MovieSeriesTypeTitles_MovieSeriesTypes]
GO
ALTER TABLE [dbo].[MoviesInGenres]  WITH CHECK ADD  CONSTRAINT [FK_MoviesInGenres_Genres] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genres] ([GenreId])
GO
ALTER TABLE [dbo].[MoviesInGenres] CHECK CONSTRAINT [FK_MoviesInGenres_Genres]
GO
ALTER TABLE [dbo].[MoviesInGenres]  WITH CHECK ADD  CONSTRAINT [FK_MoviesInGenres_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[MoviesInGenres] CHECK CONSTRAINT [FK_MoviesInGenres_Movies]
GO
ALTER TABLE [dbo].[MoviesInMovieSeries]  WITH CHECK ADD  CONSTRAINT [FK_MoviesInCollection_Collections] FOREIGN KEY([MovieSeriesId])
REFERENCES [dbo].[MovieSeries] ([MovieSeriesId])
GO
ALTER TABLE [dbo].[MoviesInMovieSeries] CHECK CONSTRAINT [FK_MoviesInCollection_Collections]
GO
ALTER TABLE [dbo].[MoviesInMovieSeries]  WITH CHECK ADD  CONSTRAINT [FK_MoviesInCollection_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[MoviesInMovieSeries] CHECK CONSTRAINT [FK_MoviesInCollection_Movies]
GO
ALTER TABLE [dbo].[MoviesOwnedByUser]  WITH CHECK ADD  CONSTRAINT [FK_MoviesOwnedByUser_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[MoviesOwnedByUser] CHECK CONSTRAINT [FK_MoviesOwnedByUser_Movies]
GO
ALTER TABLE [dbo].[MoviesOwnedByUser]  WITH CHECK ADD  CONSTRAINT [FK_MoviesOwnedByUser_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[MoviesOwnedByUser] CHECK CONSTRAINT [FK_MoviesOwnedByUser_Users]
GO
ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD  CONSTRAINT [FK_MoviesWatchedByUser_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_Movies]
GO
ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD  CONSTRAINT [FK_MoviesWatchedByUser_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_Users]
GO
ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD  CONSTRAINT [FK_MoviesWatchedByUser_WatchTypes] FOREIGN KEY([WatchTypeId])
REFERENCES [dbo].[WatchTypes] ([WatchTypeId])
GO
ALTER TABLE [dbo].[MoviesWatchedByUser] CHECK CONSTRAINT [FK_MoviesWatchedByUser_WatchTypes]
GO
ALTER TABLE [dbo].[MovieTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieTitle_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[MovieTitles] CHECK CONSTRAINT [FK_MovieTitle_Movies]
GO
ALTER TABLE [dbo].[MovieTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[MovieTitles] CHECK CONSTRAINT [FK_MovieTitles_Languages]
GO
ALTER TABLE [dbo].[MovieTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieTypeTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[MovieTypeTitles] CHECK CONSTRAINT [FK_MovieTypeTitles_Languages]
GO
ALTER TABLE [dbo].[MovieTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_MovieTypeTitles_MovieTypes] FOREIGN KEY([MovieTypeId])
REFERENCES [dbo].[MovieTypes] ([MovieTypeId])
GO
ALTER TABLE [dbo].[MovieTypeTitles] CHECK CONSTRAINT [FK_MovieTypeTitles_MovieTypes]
GO
ALTER TABLE [dbo].[PeopleInMovies]  WITH CHECK ADD  CONSTRAINT [FK_PeopleInMovies_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[PeopleInMovies] CHECK CONSTRAINT [FK_PeopleInMovies_Movies]
GO
ALTER TABLE [dbo].[PeopleInMovies]  WITH CHECK ADD  CONSTRAINT [FK_PeopleInMovies_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[PeopleInMovies] CHECK CONSTRAINT [FK_PeopleInMovies_People]
GO
ALTER TABLE [dbo].[PeopleInMovies]  WITH CHECK ADD  CONSTRAINT [FK_PeopleInMovies_RolesInDepartments] FOREIGN KEY([RoleInDepartmentId])
REFERENCES [dbo].[RolesInDepartments] ([RoleInDepartmentId])
GO
ALTER TABLE [dbo].[PeopleInMovies] CHECK CONSTRAINT [FK_PeopleInMovies_RolesInDepartments]
GO
ALTER TABLE [dbo].[PersonExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_PersonExternalLookup_ExternalSources] FOREIGN KEY([ExternalSourceId])
REFERENCES [dbo].[ExternalSources] ([ExternalSourceId])
GO
ALTER TABLE [dbo].[PersonExternalLookup] CHECK CONSTRAINT [FK_PersonExternalLookup_ExternalSources]
GO
ALTER TABLE [dbo].[PersonExternalLookup]  WITH CHECK ADD  CONSTRAINT [FK_PersonExternalLookup_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[PersonExternalLookup] CHECK CONSTRAINT [FK_PersonExternalLookup_People]
GO
ALTER TABLE [dbo].[RatingSystemTitles]  WITH CHECK ADD  CONSTRAINT [FK_RatingSystemTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[RatingSystemTitles] CHECK CONSTRAINT [FK_RatingSystemTitles_Languages]
GO
ALTER TABLE [dbo].[RatingSystemTitles]  WITH CHECK ADD  CONSTRAINT [FK_RatingSystemTitles_RatingSystems] FOREIGN KEY([RatingSystemId])
REFERENCES [dbo].[RatingSystems] ([RatingSystemId])
GO
ALTER TABLE [dbo].[RatingSystemTitles] CHECK CONSTRAINT [FK_RatingSystemTitles_RatingSystems]
GO
ALTER TABLE [dbo].[RatingSystemValues]  WITH CHECK ADD  CONSTRAINT [FK_RatingSystemValues_RatingSystems] FOREIGN KEY([RatingSystemId])
REFERENCES [dbo].[RatingSystems] ([RatingSystemId])
GO
ALTER TABLE [dbo].[RatingSystemValues] CHECK CONSTRAINT [FK_RatingSystemValues_RatingSystems]
GO
ALTER TABLE [dbo].[RatingSystemValues]  WITH CHECK ADD  CONSTRAINT [FK_RatingSystemValues_RatingTypes] FOREIGN KEY([RatingTypeId])
REFERENCES [dbo].[RatingTypes] ([RatingTypeId])
GO
ALTER TABLE [dbo].[RatingSystemValues] CHECK CONSTRAINT [FK_RatingSystemValues_RatingTypes]
GO
ALTER TABLE [dbo].[RatingTypes]  WITH CHECK ADD  CONSTRAINT [FK_RatingTypes_RatingTypes] FOREIGN KEY([ParentRatingTypeId])
REFERENCES [dbo].[RatingTypes] ([RatingTypeId])
GO
ALTER TABLE [dbo].[RatingTypes] CHECK CONSTRAINT [FK_RatingTypes_RatingTypes]
GO
ALTER TABLE [dbo].[RatingTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_RatingTypeTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[RatingTypeTitles] CHECK CONSTRAINT [FK_RatingTypeTitles_Languages]
GO
ALTER TABLE [dbo].[RatingTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_RatingTypeTitles_RatingTypes] FOREIGN KEY([RatingTypeId])
REFERENCES [dbo].[RatingTypes] ([RatingTypeId])
GO
ALTER TABLE [dbo].[RatingTypeTitles] CHECK CONSTRAINT [FK_RatingTypeTitles_RatingTypes]
GO
ALTER TABLE [dbo].[RolesInDepartments]  WITH CHECK ADD  CONSTRAINT [FK_RolesInDepartments_Departments] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([DepartmentId])
GO
ALTER TABLE [dbo].[RolesInDepartments] CHECK CONSTRAINT [FK_RolesInDepartments_Departments]
GO
ALTER TABLE [dbo].[RolesInDepartments]  WITH CHECK ADD  CONSTRAINT [FK_RolesInDepartments_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[RolesInDepartments] CHECK CONSTRAINT [FK_RolesInDepartments_Roles]
GO
ALTER TABLE [dbo].[RoleTitles]  WITH CHECK ADD  CONSTRAINT [FK_RoleTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Languages]
GO
ALTER TABLE [dbo].[RoleTitles]  WITH CHECK ADD  CONSTRAINT [FK_RoleTitles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Roles]
GO
ALTER TABLE [dbo].[RoleTitles]  WITH CHECK ADD  CONSTRAINT [FK_RoleTitles_Roles1] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Roles1]
GO
ALTER TABLE [dbo].[Seasons]  WITH CHECK ADD  CONSTRAINT [FK_Seasons_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[Seasons] CHECK CONSTRAINT [FK_Seasons_Movies]
GO
ALTER TABLE [dbo].[Seasons]  WITH CHECK ADD  CONSTRAINT [FK_Seasons_Movies1] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[Seasons] CHECK CONSTRAINT [FK_Seasons_Movies1]
GO
ALTER TABLE [dbo].[UserCharacterFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserCharactersFavorites_Characters] FOREIGN KEY([CharacterId])
REFERENCES [dbo].[Characters] ([CharacterId])
GO
ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_Characters]
GO
ALTER TABLE [dbo].[UserCharacterFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserCharactersFavorites_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])
GO
ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_FavoriteTypes]
GO
ALTER TABLE [dbo].[UserCharacterFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserCharactersFavorites_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserCharacterFavorites] CHECK CONSTRAINT [FK_UserCharactersFavorites_Users]
GO
ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD  CONSTRAINT [FK_UserCharacterInMovieRatings_ActorsAsCharacters] FOREIGN KEY([ActorAsCharacterId])
REFERENCES [dbo].[ActorsAsCharacters] ([ActorAsCharacterId])
GO
ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [FK_UserCharacterInMovieRatings_ActorsAsCharacters]
GO
ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD  CONSTRAINT [FK_UserCharacterInMovieRatings_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [FK_UserCharacterInMovieRatings_Users]
GO
ALTER TABLE [dbo].[UserMovieFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieFavorites_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])
GO
ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_FavoriteTypes]
GO
ALTER TABLE [dbo].[UserMovieFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieFavorites_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_Movies]
GO
ALTER TABLE [dbo].[UserMovieFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieFavorites_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserMovieFavorites] CHECK CONSTRAINT [FK_UserMovieFavorites_Users]
GO
ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieGenres_Genres] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genres] ([GenreId])
GO
ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Genres]
GO
ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieGenres_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Movies]
GO
ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieGenres_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [FK_UserMovieGenres_Users]
GO
ALTER TABLE [dbo].[UserMovieHashtags]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieHashtags_Hashtags] FOREIGN KEY([Hashtag])
REFERENCES [dbo].[Hashtags] ([Hashtag])
GO
ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Hashtags]
GO
ALTER TABLE [dbo].[UserMovieHashtags]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieHashtags_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Movies]
GO
ALTER TABLE [dbo].[UserMovieHashtags]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieHashtags_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserMovieHashtags] CHECK CONSTRAINT [FK_UserMovieHashtags_Users]
GO
ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieRatings_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_Movies]
GO
ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieRatings_RatingTypes] FOREIGN KEY([RatingTypeId])
REFERENCES [dbo].[RatingTypes] ([RatingTypeId])
GO
ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_RatingTypes]
GO
ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieRatings_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [FK_UserMovieRatings_Users]
GO
ALTER TABLE [dbo].[UserMovieReviews]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieReviews_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[UserMovieReviews] CHECK CONSTRAINT [FK_UserMovieReviews_Movies]
GO
ALTER TABLE [dbo].[UserMovieReviews]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieReviews_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserMovieReviews] CHECK CONSTRAINT [FK_UserMovieReviews_Users]
GO
ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieWishlist_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [FK_UserMovieWishlist_Movies]
GO
ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD  CONSTRAINT [FK_UserMovieWishlist_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [FK_UserMovieWishlist_Users]
GO
ALTER TABLE [dbo].[UserPeopleFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserPeopleFavorites_FavoriteTypes] FOREIGN KEY([FavoriteTypeId])
REFERENCES [dbo].[FavoriteTypes] ([FavoriteTypeId])
GO
ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_FavoriteTypes]
GO
ALTER TABLE [dbo].[UserPeopleFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserPeopleFavorites_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_People]
GO
ALTER TABLE [dbo].[UserPeopleFavorites]  WITH CHECK ADD  CONSTRAINT [FK_UserPeopleFavorites_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserPeopleFavorites] CHECK CONSTRAINT [FK_UserPeopleFavorites_Users]
GO
ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD  CONSTRAINT [FK_UserPersonInMovieRoleRatings_PeopleInMovies] FOREIGN KEY([PersonInMovieId])
REFERENCES [dbo].[PeopleInMovies] ([PersonInMovieId])
GO
ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [FK_UserPersonInMovieRoleRatings_PeopleInMovies]
GO
ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD  CONSTRAINT [FK_UserPersonInMovieRoleRatings_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [FK_UserPersonInMovieRoleRatings_Users]
GO
ALTER TABLE [dbo].[UserSessions]  WITH CHECK ADD  CONSTRAINT [FK_UserSession_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserSessions] CHECK CONSTRAINT [FK_UserSession_Users]
GO
ALTER TABLE [dbo].[WatchTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_WatchTypeTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[WatchTypeTitles] CHECK CONSTRAINT [FK_WatchTypeTitles_Languages]
GO
ALTER TABLE [dbo].[WatchTypeTitles]  WITH CHECK ADD  CONSTRAINT [FK_WatchTypeTitles_WatchTypes] FOREIGN KEY([WatchTypeId])
REFERENCES [dbo].[WatchTypes] ([WatchTypeId])
GO
ALTER TABLE [dbo].[WatchTypeTitles] CHECK CONSTRAINT [FK_WatchTypeTitles_WatchTypes]
GO
ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD  CONSTRAINT [FK_VoiceOverActors_ActorsAsCharacters] FOREIGN KEY([ActorAsCharacterId])
REFERENCES [dbo].[ActorsAsCharacters] ([ActorAsCharacterId])
GO
ALTER TABLE [dbo].[VoiceOverActors] CHECK CONSTRAINT [FK_VoiceOverActors_ActorsAsCharacters]
GO
ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD  CONSTRAINT [FK_VoiceOverActors_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [dbo].[VoiceOverActors] CHECK CONSTRAINT [FK_VoiceOverActors_Languages]
GO
ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD  CONSTRAINT [FK_VoiceOverActors_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[VoiceOverActors] CHECK CONSTRAINT [FK_VoiceOverActors_People]
GO
ALTER TABLE [edu].[Bounties]  WITH CHECK ADD  CONSTRAINT [FK_Bounties_BountyTypes] FOREIGN KEY([BountyTypeId])
REFERENCES [edu].[BountyTypes] ([BountyTypeId])
GO
ALTER TABLE [edu].[Bounties] CHECK CONSTRAINT [FK_Bounties_BountyTypes]
GO
ALTER TABLE [edu].[TaskBounties]  WITH CHECK ADD  CONSTRAINT [FK_TaskBounties_Bounties] FOREIGN KEY([BountyId])
REFERENCES [edu].[Bounties] ([BountyId])
GO
ALTER TABLE [edu].[TaskBounties] CHECK CONSTRAINT [FK_TaskBounties_Bounties]
GO
ALTER TABLE [edu].[TaskBounties]  WITH CHECK ADD  CONSTRAINT [FK_TaskBounties_Tasks] FOREIGN KEY([TaskId])
REFERENCES [edu].[Tasks] ([TaskId])
GO
ALTER TABLE [edu].[TaskBounties] CHECK CONSTRAINT [FK_TaskBounties_Tasks]
GO
ALTER TABLE [edu].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Subjects] FOREIGN KEY([SubjectId])
REFERENCES [edu].[Subjects] ([SubjectId])
GO
ALTER TABLE [edu].[Tasks] CHECK CONSTRAINT [FK_Tasks_Subjects]
GO
ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD  CONSTRAINT [FK_UserTaskBounties_TaskBounties] FOREIGN KEY([TaskBountyId])
REFERENCES [edu].[TaskBounties] ([TaskBountyId])
GO
ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_TaskBounties]
GO
ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD  CONSTRAINT [FK_UserTaskBounties_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users]
GO
ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD  CONSTRAINT [FK_UserTaskBounties_Users1] FOREIGN KEY([AssignedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users1]
GO
ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD  CONSTRAINT [FK_UserTaskBounties_Users2] FOREIGN KEY([AwardedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users2]
GO
ALTER TABLE [music].[AlbumsInGenres]  WITH CHECK ADD  CONSTRAINT [FK_AlbumsInGenres_Albums] FOREIGN KEY([AlbumId])
REFERENCES [music].[Albums] ([AlbumId])
GO
ALTER TABLE [music].[AlbumsInGenres] CHECK CONSTRAINT [FK_AlbumsInGenres_Albums]
GO
ALTER TABLE [music].[AlbumsInGenres]  WITH CHECK ADD  CONSTRAINT [FK_AlbumsInGenres_Genres] FOREIGN KEY([GenreId])
REFERENCES [music].[Genres] ([GenreId])
GO
ALTER TABLE [music].[AlbumsInGenres] CHECK CONSTRAINT [FK_AlbumsInGenres_Genres]
GO
ALTER TABLE [music].[ArtistsInGenres]  WITH CHECK ADD  CONSTRAINT [FK_ArtistsInGenres_Artists] FOREIGN KEY([ArtistId])
REFERENCES [music].[Artists] ([ArtistId])
GO
ALTER TABLE [music].[ArtistsInGenres] CHECK CONSTRAINT [FK_ArtistsInGenres_Artists]
GO
ALTER TABLE [music].[ArtistsInGenres]  WITH CHECK ADD  CONSTRAINT [FK_ArtistsInGenres_Genres] FOREIGN KEY([GenreId])
REFERENCES [music].[Genres] ([GenreId])
GO
ALTER TABLE [music].[ArtistsInGenres] CHECK CONSTRAINT [FK_ArtistsInGenres_Genres]
GO
ALTER TABLE [music].[GenreTitles]  WITH CHECK ADD  CONSTRAINT [FK_GenreTitles_Genres] FOREIGN KEY([GenreId])
REFERENCES [music].[Genres] ([GenreId])
GO
ALTER TABLE [music].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Genres]
GO
ALTER TABLE [music].[GenreTitles]  WITH CHECK ADD  CONSTRAINT [FK_GenreTitles_Languages] FOREIGN KEY([Language])
REFERENCES [dbo].[Languages] ([Language])
GO
ALTER TABLE [music].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Languages]
GO
ALTER TABLE [music].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_Albums] FOREIGN KEY([AlbumId])
REFERENCES [music].[Albums] ([AlbumId])
GO
ALTER TABLE [music].[Songs] CHECK CONSTRAINT [FK_Songs_Albums]
GO
ALTER TABLE [music].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_Artists] FOREIGN KEY([ArtistId])
REFERENCES [music].[Artists] ([ArtistId])
GO
ALTER TABLE [music].[Songs] CHECK CONSTRAINT [FK_Songs_Artists]
GO
ALTER TABLE [music].[SongsInGenres]  WITH CHECK ADD  CONSTRAINT [FK_SongsInGenres_Genres] FOREIGN KEY([GenreId])
REFERENCES [music].[Genres] ([GenreId])
GO
ALTER TABLE [music].[SongsInGenres] CHECK CONSTRAINT [FK_SongsInGenres_Genres]
GO
ALTER TABLE [music].[SongsInGenres]  WITH CHECK ADD  CONSTRAINT [FK_SongsInGenres_Songs] FOREIGN KEY([SongId])
REFERENCES [music].[Songs] ([SongId])
GO
ALTER TABLE [music].[SongsInGenres] CHECK CONSTRAINT [FK_SongsInGenres_Songs]
GO
ALTER TABLE [music].[SongsInMovies]  WITH CHECK ADD  CONSTRAINT [FK_SongsInMovies_Movies] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([MovieId])
GO
ALTER TABLE [music].[SongsInMovies] CHECK CONSTRAINT [FK_SongsInMovies_Movies]
GO
ALTER TABLE [music].[SongsInMovies]  WITH CHECK ADD  CONSTRAINT [FK_SongsInMovies_Songs] FOREIGN KEY([SongId])
REFERENCES [music].[Songs] ([SongId])
GO
ALTER TABLE [music].[SongsInMovies] CHECK CONSTRAINT [FK_SongsInMovies_Songs]
GO
ALTER TABLE [dbo].[MovieExternalRatings]  WITH CHECK ADD  CONSTRAINT [CK_MovieExternalRatings_Rating] CHECK  (([ExternalRating]>=(0) AND [ExternalRating]<=(10)))
GO
ALTER TABLE [dbo].[MovieExternalRatings] CHECK CONSTRAINT [CK_MovieExternalRatings_Rating]
GO
ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserCharacterInMovieRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
GO
ALTER TABLE [dbo].[UserCharacterInMovieRatings] CHECK CONSTRAINT [CK_UserCharacterInMovieRatings_Rating]
GO
ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieGenres_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
GO
ALTER TABLE [dbo].[UserMovieGenres] CHECK CONSTRAINT [CK_UserMovieGenres_Rating]
GO
ALTER TABLE [dbo].[UserMovieRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
GO
ALTER TABLE [dbo].[UserMovieRatings] CHECK CONSTRAINT [CK_UserMovieRatings_Rating]
GO
ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD  CONSTRAINT [CK_UserMovieWishlist_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
GO
ALTER TABLE [dbo].[UserMovieWishlist] CHECK CONSTRAINT [CK_UserMovieWishlist_Rating]
GO
ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD  CONSTRAINT [CK_UserPersonInMovieRoleRatings_Rating] CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
GO
ALTER TABLE [dbo].[UserPersonInMovieRoleRatings] CHECK CONSTRAINT [CK_UserPersonInMovieRoleRatings_Rating]
GO
/****** Object:  StoredProcedure [dbo].[AcquisitionDelete]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[AcquisitionDelete]
	@acquisitionId int
as
begin
	delete
	from dbo.Acquisitions
	where AcquisitionId = @acquisitionId
end;

GO
/****** Object:  StoredProcedure [dbo].[AcquisitionSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[AcquisitionSave]
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

GO
/****** Object:  StoredProcedure [dbo].[CharacterGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.CharacterGet
-- Date: 2018-04-23
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[CharacterGet]
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

GO
/****** Object:  StoredProcedure [dbo].[CharacterSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

Create procedure [dbo].[CharacterSave]
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
GO
/****** Object:  StoredProcedure [dbo].[CharacterSearch]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.CharacterSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[CharacterSearch]
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
		where Name like '%' + @searchText + '%';
	end;
end;

GO
/****** Object:  StoredProcedure [dbo].[DepartmentGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.DepartmentGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[DepartmentGet]
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

GO
/****** Object:  StoredProcedure [dbo].[DepartmentGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.DepartmentGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[DepartmentGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[DepartmentSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.DepartmentSave
-- Date: 2018-05-28
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[DepartmentSave]
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
		end
		else
		begin
			insert @output
			select @departmentId
		end;
		
		set @departmentId = (select Id from @output);

		merge dbo.DepartmentTitles as t
		using (
			select @departmentId as DepartmentId
				,mt.Language
				,mt.Title
			from @titles as mt
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
			select @departmentId as DepartmentId
				,dr.Id as RoleId
			from @roles as dr
		) as s on s.DepartmentId = t.DepartmentId
			and s.RoleId = t.RoleId
		when not matched by target then
			insert (DepartmentId, RoleId)
			values (s.DepartmentId, s.RoleId)
		when not matched by source and t.DepartmentId = @departmentId then
			delete;

		exec dbo.DepartmentGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
GO
/****** Object:  StoredProcedure [dbo].[ErrorGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[ErrorGet]
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

GO
/****** Object:  StoredProcedure [dbo].[ErrorSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.ErrorSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[ErrorSave]
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

GO
/****** Object:  StoredProcedure [dbo].[ExternalSourceGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.ExternalSourceGet
-- Date: 2018-03-19
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[ExternalSourceGet]
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

GO
/****** Object:  StoredProcedure [dbo].[ExternalSourceGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.ExternalSourceGetAll
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[ExternalSourceGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[ExternalSourceSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.ExternalSourceSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[ExternalSourceSave]
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

GO
/****** Object:  StoredProcedure [dbo].[GenreGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.GenreGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstag. All rights reserveg.
------------------------------------------------------------------------

Create procedure [dbo].[GenreGet]
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

GO
/****** Object:  StoredProcedure [dbo].[GenreGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.GenreGetAll
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstag. All rights reserveg.
------------------------------------------------------------------------

Create procedure [dbo].[GenreGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[GenreSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------------------
-- Name: dbo.GenreSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[GenreSave]
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
GO
/****** Object:  StoredProcedure [dbo].[IconGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[IconGet]
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

GO
/****** Object:  StoredProcedure [dbo].[IconSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.IconSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[IconSave]
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

GO
/****** Object:  StoredProcedure [dbo].[IconTypeGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.IconTypeGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[IconTypeGet]
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

GO
/****** Object:  StoredProcedure [dbo].[IconTypeGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.IconTypeGetAll
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[IconTypeGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[IconTypeSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------------------
-- Name: dbo.IconTypeSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[IconTypeSave]
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
GO
/****** Object:  StoredProcedure [dbo].[LanguageGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[LanguageGet]
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
		and t.InLanguage = 'en-US'
	order by l.Language;
end;

GO
/****** Object:  StoredProcedure [dbo].[LanguageGetNative]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.LanguageGetNative
-- Date: 2017-07-06
-- Release: 1.0
-- Summary:
--   * Gets all language titles in their native language, or en-US if missing in the native language.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[LanguageGetNative]
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
		and t.InLanguage = 'en-US'
	order by l.Language;
end;

GO
/****** Object:  StoredProcedure [dbo].[LanguageSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.LanguageSave
-- Date: 2017-07-07
-- Release: 1.0
-- Summary:
--   * Saves a language and it's titles.
-- Param:
--   @languageId nvarchar(8)
--     * The id of the language.
--   @titles dbo.LanguagesTitlesCollection
--     * The titles of the language.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[LanguageSave]
	@language nvarchar(8)
	,@titles dbo.LanguagesTitlesCollection readonly
as
begin
	if (select count(1) from @titles) = 0
	begin
		throw 50000, 'Invalid titles count. A language can''t be saved witout any titles.', 1;
	end;

	if not exists (select 1 from @titles where InLanguage = 'en-US')
	begin
		throw 50000, 'Missing default title. A language can''t be saved witout a title in the default language (en-US).', 1;
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

GO
/****** Object:  StoredProcedure [dbo].[MovieSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.MovieSave
-- Date: 2018-05-11
-- Release: 1.0
-- Summary:
--   * Saves a movie.
-- Param:
--   @xxx
--     * xxx
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[MovieSave]
	@movieId int
	,@movieTypeId int
	,@year int
	,@endYear int
	,@runTime int
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
				,@year
				,@endYear
				,@runTime;
		end
		else
		begin
			update dbo.Movies
			set MovieTypeId = @movieTypeId
				,"Year" = @year
				,EndYear = @endYear
				,RunTime = @runTime
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

GO
/****** Object:  StoredProcedure [dbo].[MovieSearch]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.MovieSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[MovieSearch]
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
		where ti.Title like '%' + @searchText + '%';
	end;
end;

GO
/****** Object:  StoredProcedure [dbo].[MovieSeriesGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.MovieSeriesGet
-- Date: 2018-04-11
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[MovieSeriesGet]
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

GO
/****** Object:  StoredProcedure [dbo].[MovieSeriesSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesSave
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[MovieSeriesSave]
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
GO
/****** Object:  StoredProcedure [dbo].[MovieSeriesSearch]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.MovieSeriesSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[MovieSeriesSearch]
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
		where ti.Title like '%' + @searchText + '%';
	end;
end;

GO
/****** Object:  StoredProcedure [dbo].[MovieSeriesTypeGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[MovieSeriesTypeGet]
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

GO
/****** Object:  StoredProcedure [dbo].[MovieSeriesTypeGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[MovieSeriesTypeGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[MovieSeriesTypeSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[MovieSeriesTypeSave]
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

GO
/****** Object:  StoredProcedure [dbo].[MoviesOwnedByUserSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[MoviesOwnedByUserSave]
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

GO
/****** Object:  StoredProcedure [dbo].[MovieTypeGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[MovieTypeGet]
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

GO
/****** Object:  StoredProcedure [dbo].[MovieTypeGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[MovieTypeGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[MovieTypeSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[MovieTypeSave]
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

GO
/****** Object:  StoredProcedure [dbo].[PersonGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.PersonGet
-- Date: 2018-05-11
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[PersonGet]
	@personIds dbo.IdCollection readonly
as
begin
	select p.PersonId
		,p.Name
		,p.BirthDate
		,p.DeathDate
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

GO
/****** Object:  StoredProcedure [dbo].[PersonSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------------------
-- Name: dbo.PersonSave
-- Date: 2018-05-12
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

Create procedure [dbo].[PersonSave]
	@personId int
	,@name nvarchar(100)
	,@birthDate date
	,@deathDate date = null
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
			select @name
				,@birthDate
				,@deathDate
				,null;
		end
		else
		begin
			update dbo.People
			set Name = @name
				,BirthDate = @birthDate
				,DeathDate = @deathDate
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
			set t.ExternalId = s.ExternalId;

		exec dbo.PersonGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
GO
/****** Object:  StoredProcedure [dbo].[PersonSearch]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.PersonSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[PersonSearch]
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
		where Name like '%' + @searchText + '%';
	end;
end;

GO
/****** Object:  StoredProcedure [dbo].[RatingSystemGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[RatingSystemGet]
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

GO
/****** Object:  StoredProcedure [dbo].[RatingSystemGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.RatingSystemGetAll
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets all rating systems.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[RatingSystemGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[RatingSystemSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.RatingSystemSave
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Saves a rating system and it's values.
-- Param:
--   @ratingSystemId int
--     * The id of the movie type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the movie type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[RatingSystemSave]
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
		;throw 50000, 'Invalid total sum. All rating system values for each parent group needs to sum to 100.', 1;
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

GO
/****** Object:  StoredProcedure [dbo].[RatingTypeGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[RatingTypeGet]
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

GO
/****** Object:  StoredProcedure [dbo].[RatingTypeGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[RatingTypeGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[RatingTypeSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[RatingTypeSave]
	@ratingTypeId int
	,@parentRatingTypeId int = null
	,@titles dbo.LanguageDescriptionCollection readonly
as
begin
	if (select count(1) from @titles) = 0
	begin
		;throw 50000, 'Invalid titles count. A rating type can''t be saved without any titles.', 1;
	end;

	if not exists (select 1 from @titles where Language = 'en-US')
	begin
		;throw 50000, 'Missing default title. A rating type can''t be saved without a title in the default language (en-US).', 1;
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

GO
/****** Object:  StoredProcedure [dbo].[RoleGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.RoleGet
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[RoleGet]
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

GO
/****** Object:  StoredProcedure [dbo].[RoleGetAll]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.RoleGetAll
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[RoleGetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[RoleSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------------------
-- Name: dbo.RoleSave
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[RoleSave]
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
GO
/****** Object:  StoredProcedure [dbo].[UserGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[UserGet]
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

GO
/****** Object:  StoredProcedure [dbo].[UserSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.UserSave
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[UserSave]
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
				,''
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

GO
/****** Object:  StoredProcedure [dbo].[UserSearch]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.UserSearch
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[UserSearch]
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
		where Name like '%' + @searchText + '%'
			or Username like '%' + @searchText + '%';
	end;
end;

GO
/****** Object:  StoredProcedure [dbo].[UserSessionGet]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[UserSessionGet]
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

GO
/****** Object:  StoredProcedure [dbo].[UserSessionSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[UserSessionSave]
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
				;throw 50000, 'The specified username and/or password is not valid.', 1;
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

GO
/****** Object:  StoredProcedure [dbo].[UserSetPassword]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------------------
-- Name: dbo.UserSetPassword
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

Create procedure [dbo].[UserSetPassword]
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
				and "Password" = ''
		)
		begin
			;throw 50000, 'The specified username and/or password is not valid.', 1;
		end;

		update dbo.Users
			set UserName = @userName
				,"Password" = @password
			output inserted.UserId into @output
			where UserId = @userId;

		exec dbo.UserGet @output;
end;
GO
/****** Object:  StoredProcedure [dbo].[VendorSave]    Script Date: 2018-06-14 23:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

Create procedure [dbo].[VendorSave]
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

GO
USE [master]
GO
ALTER DATABASE [CMDB] SET  READ_WRITE 
GO
