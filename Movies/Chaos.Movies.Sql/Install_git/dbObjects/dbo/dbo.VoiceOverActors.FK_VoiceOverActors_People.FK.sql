------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : VoiceOverActors 
-- name     : FK_VoiceOverActors_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD CONSTRAINT [FK_VoiceOverActors_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
