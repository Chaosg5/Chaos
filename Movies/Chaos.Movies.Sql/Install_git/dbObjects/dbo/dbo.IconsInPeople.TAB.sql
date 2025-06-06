------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInPeople 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInPeople]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInPeople](
		[IconId] [int] NOT NULL,
		[PersonId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInPeople] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[PersonId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInPeople_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInPeople]'))
BEGIN
	ALTER TABLE [dbo].[IconsInPeople] CHECK CONSTRAINT [FK_IconsInPeople_Icons]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInPeople_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInPeople]'))
BEGIN
	ALTER TABLE [dbo].[IconsInPeople] CHECK CONSTRAINT [FK_IconsInPeople_People]
END 

GO

------------------------------------- 
