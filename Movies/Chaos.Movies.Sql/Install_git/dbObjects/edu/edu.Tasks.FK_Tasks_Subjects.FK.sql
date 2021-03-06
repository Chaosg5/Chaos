------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : Tasks 
-- name     : FK_Tasks_Subjects 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Tasks_Subjects]') AND parent_object_id = OBJECT_ID(N'[edu].[Tasks]'))
BEGIN
	ALTER TABLE [edu].[Tasks]  WITH CHECK ADD CONSTRAINT [FK_Tasks_Subjects] FOREIGN KEY([SubjectId])
	REFERENCES [edu].[Subjects] ([SubjectId])

END 

GO

------------------------------------- 
