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
