------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesGet
-- Date: 2018-04-11
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesGet]
	@MovieSeriesIds dbo.IdCollection readonly
as
begin
	select s.MovieSeriesId
		,s.MovieSeriesTypeId
	from dbo.MovieSeries as s
	inner join @MovieSeriesIds as i on i.Id = s.MovieSeriesId;

	select s.MovieSeriesId
		,ti.Language
		,ti.Title
	from dbo.MovieSeries as s
	inner join dbo.MovieSeriesTitles as ti on ti.MovieSeriesId = s.MovieSeriesId
	inner join @MovieSeriesIds as i on i.Id = s.MovieSeriesId;

	select d.MovieSeriesId
		,m.MovieId
	from dbo.MovieSeries as d
	inner join dbo.MoviesInMovieSeries as m on m.MovieSeriesId = d.MovieSeriesId
	inner join @MovieSeriesIds as i on i.Id = d.MovieSeriesId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesGet] 
GRANT EXECUTE ON [dbo].[MovieSeriesGet] TO [rCMDB] 
GO

------------------------------------- 
