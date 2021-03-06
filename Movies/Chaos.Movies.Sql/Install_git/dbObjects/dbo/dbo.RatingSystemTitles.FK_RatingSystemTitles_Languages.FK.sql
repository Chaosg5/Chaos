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
