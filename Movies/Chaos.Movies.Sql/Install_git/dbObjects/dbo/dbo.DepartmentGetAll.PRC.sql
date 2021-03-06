------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : DepartmentGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.DepartmentGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DepartmentGetAll]
as
begin
	select d.DepartmentId
		,d."Order"
	from dbo.Departments as d;

	select d.DepartmentId
		,ti.Language
		,ti.Title
	from dbo.Departments d
	inner join dbo.DepartmentTitles as ti on ti.DepartmentId = d.DepartmentId;

	select d.DepartmentId
		,rd.RoleId
	from dbo.Departments d
	inner join dbo.RolesInDepartments as rd on rd.DepartmentId = d.DepartmentId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[DepartmentGetAll] 
GRANT EXECUTE ON [dbo].[DepartmentGetAll] TO [rCMDB] 
GO

------------------------------------- 
