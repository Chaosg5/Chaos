------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesTypesGetAll
-- Date: 2018-04-10
-- Release: 1.0
-- Summary:
--   * Gets all movie series types.
-- Param:
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesTypeGetAll]
as
begin
	select t.MovieSeriesTypeId
	from dbo.MovieSeriesTypes as t;

	select t.MovieSeriesTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieSeriesTypes t
	inner join dbo.MovieSeriesTypeTitles as ti on ti.MovieSeriesTypeId = t.MovieSeriesTypeId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesTypeGetAll] 
GRANT EXECUTE ON [dbo].[MovieSeriesTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
