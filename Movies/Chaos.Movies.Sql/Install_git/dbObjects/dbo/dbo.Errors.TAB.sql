------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Errors 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Errors]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Errors](
		[ErrorId] [int] IDENTITY(1,1) NOT NULL,
		[UserId] [int] NOT NULL,
		[Time] [datetime2](7) NOT NULL,
		[Type] [nvarchar](255) NOT NULL,
		[Source] [nvarchar](255) NOT NULL,
		[TargetSite] [nvarchar](255) NOT NULL,
		[Message] [nvarchar](255) NOT NULL,
		[StackTrace] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_Exceptions] PRIMARY KEY CLUSTERED 
	(
		[ErrorId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Exceptions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Errors]'))
BEGIN
	ALTER TABLE [dbo].[Errors] CHECK CONSTRAINT [FK_Exceptions_Users]
END 

GO

------------------------------------- 
