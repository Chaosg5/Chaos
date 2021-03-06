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
