------------------------------------- 
-- database : CMDB 
-- scheme   : lists 
-- name     : lists 
-- Type     : schema 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'lists')
BEGIN
	EXEC sys.sp_executesql N'CREATE SCHEMA [lists]'

END 

GO

------------------------------------- 
