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
