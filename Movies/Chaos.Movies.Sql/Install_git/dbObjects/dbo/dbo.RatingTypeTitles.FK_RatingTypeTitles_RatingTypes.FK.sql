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
