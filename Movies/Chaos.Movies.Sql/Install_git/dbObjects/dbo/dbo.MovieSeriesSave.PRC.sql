------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.MovieSeriesSave
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesSave]
	@movieSeriesId int
	,@movieSeriesTypeId int
	,@titles dbo.LanguageTitleCollection readonly
	,@movieIds dbo.IdOrderCollection readonly
	,@icons dbo.IdOrderCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@movieSeriesId = 0)
		begin
			insert dbo.MovieSeries
			output inserted.MovieSeriesId into @output
			select @movieSeriesTypeId;
		end
		else
		begin
			update dbo.MovieSeries
			set MovieSeriesTypeId = @movieSeriesTypeId
			output inserted.MovieSeriesId into @output
			where MovieSeriesId = @movieSeriesId;
		end;

		merge dbo.MovieSeriesTitles as t
		using (
			select r.Id as MovieSeriesId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesId = t.MovieSeriesId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieSeriesId, Language, Title)
			values (s.MovieSeriesId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.MoviesInMovieSeries as t
		using (
			select r.Id as MovieSeriesId
				,dr.Id as MovieId
				,dr."Order"
			from @movieIds as dr
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesId = t.MovieSeriesId
			and s.MovieId = t.MovieId
		when not matched by target then
			insert (MovieSeriesId, MovieId, "Order")
			values (s.MovieSeriesId, s.MovieId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		merge dbo.IconsInMovieSeries as t
		using (
			select r.Id as MovieSeriesId
				,x.Id as IconId
				,x."Order"
			from @icons as x
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesId = t.MovieSeriesId
			and s.IconId = t.IconId
		when not matched by target then
			insert (MovieSeriesId, IconId, "Order")
			values (s.MovieSeriesId, s.IconId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		exec dbo.MovieSeriesGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesSave] 
GRANT EXECUTE ON [dbo].[MovieSeriesSave] TO [rCMDB] 
GO

------------------------------------- 
