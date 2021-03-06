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
