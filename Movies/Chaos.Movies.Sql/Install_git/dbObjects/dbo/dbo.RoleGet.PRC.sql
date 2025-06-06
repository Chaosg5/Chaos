------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RoleGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RoleGet
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RoleGet]
	@roleIds dbo.IdCollection readonly
as
begin
	select d.RoleId
	from dbo.Roles as d
	inner join @roleIds as i on i.Id = d.RoleId;

	select d.RoleId
		,ti.Language
		,ti.Title
	from dbo.Roles d
	inner join dbo.RoleTitles as ti on ti.RoleId = d.RoleId
	inner join @roleIds as i on i.Id = d.RoleId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RoleGet] 
GRANT EXECUTE ON [dbo].[RoleGet] TO [rCMDB] 
GO

------------------------------------- 
