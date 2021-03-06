------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesTypesGet
-- Date: 2018-04-10
-- Release: 1.0
-- Summary:
--   * Gets the specified movie series types.
-- Param:
--   @movieSeriesTypeIds dbo.IdCollection
--     * The list of ids of the movie series types to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesTypeGet]
	@movieSeriesTypeIds dbo.IdCollection readonly
as
begin
	select t.MovieSeriesTypeId
	from dbo.MovieSeriesTypes as t
	inner join @movieSeriesTypeIds as r on r.Id = t.MovieSeriesTypeId;

	select t.MovieSeriesTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieSeriesTypes t
	inner join dbo.MovieSeriesTypeTitles as ti on ti.MovieSeriesTypeId = t.MovieSeriesTypeId
	inner join @movieSeriesTypeIds as i on i.Id = t.MovieSeriesTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesTypeGet] 
GRANT EXECUTE ON [dbo].[MovieSeriesTypeGet] TO [rCMDB] 
GO

------------------------------------- 
