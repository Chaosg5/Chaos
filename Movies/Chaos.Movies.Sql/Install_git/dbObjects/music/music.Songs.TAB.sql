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
