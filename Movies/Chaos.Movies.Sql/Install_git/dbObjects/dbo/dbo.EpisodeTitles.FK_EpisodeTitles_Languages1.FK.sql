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
