------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : music 
-- Type     : schema 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'music')
BEGIN
	EXEC sys.sp_executesql N'CREATE SCHEMA [music]'

END 

GO

------------------------------------- 
