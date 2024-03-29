------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSessions 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSessions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[UserSessions](
		[UserSessionId] [uniqueidentifier] NOT NULL,
		[ClientIp] [nvarchar](50) NOT NULL,
		[UserId] [int] NOT NULL,
		[ActiveFrom] [datetime2](0) NOT NULL,
		[ActiveTo] [datetime2](0) NOT NULL,
	 CONSTRAINT [PK_UserSession] PRIMARY KEY CLUSTERED 
	(
		[UserSessionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSession_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSessions]'))
BEGIN
	ALTER TABLE [dbo].[UserSessions] CHECK CONSTRAINT [FK_UserSession_Users]
END 

GO

------------------------------------- 
