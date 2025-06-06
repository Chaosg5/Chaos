------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieTypesGetAll
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets all movie types.
-- Param:
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieTypeGetAll]
as
begin
	select t.MovieTypeId
	from dbo.MovieTypes t

	select t.MovieTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieTypes t
	inner join dbo.MovieTypeTitles as ti on ti.MovieTypeId = t.MovieTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieTypeGetAll] 
GRANT EXECUTE ON [dbo].[MovieTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
