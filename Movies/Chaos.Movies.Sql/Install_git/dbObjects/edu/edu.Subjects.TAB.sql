------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Subjects 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Subjects]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Subjects](
		[SubjectId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
	(
		[SubjectId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
