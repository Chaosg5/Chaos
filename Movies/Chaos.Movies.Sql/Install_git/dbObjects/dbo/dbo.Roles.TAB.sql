------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Roles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Roles](
		[RoleId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
	(
		[RoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
