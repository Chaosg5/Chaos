------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Icons 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Icons]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Icons](
		[IconId] [int] IDENTITY(1,1) NOT NULL,
		[IconTypeId] [int] NOT NULL,
		[IconUrl] [nvarchar](500) NULL,
		[DataSize] [int] NULL,
		[Data] [varbinary](max) NULL,
	 CONSTRAINT [PK_Icons] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Icons_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Icons]'))
BEGIN
	ALTER TABLE [dbo].[Icons] CHECK CONSTRAINT [FK_Icons_IconTypes]
END 

GO

------------------------------------- 
