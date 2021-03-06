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
